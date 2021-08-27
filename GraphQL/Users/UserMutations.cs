using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using mhyphen.Models;
using mhyphen.Data;
using mhyphen.Extensions;
using Octokit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Authorization;

namespace mhyphen.GraphQL.Users
{
    [ExtendObjectType(name: "Mutation")]
    public class UserMutations
    {
        [UseAppDbContext]
        [Authorize]
        public async Task<Models.User> EditSelfAsync(EditSelfInput input, ClaimsPrincipal claimsPrincipal,
                [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var userIdStr = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;
            var user = await context.Users.FindAsync(int.Parse(userIdStr), cancellationToken);

            user.Name = input.Name ?? user.Name;
            user.ImageURI = input.ImageURI ?? user.ImageURI;

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }

        [UseAppDbContext]
        public async Task<LoginPayload> LoginAsync(LoginInput input, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var client = new GitHubClient(new ProductHeaderValue("mhyphen"));

            var request = new OauthTokenRequest(Startup.Configuration["Github:ClientId"], Startup.Configuration["Github:ClientSecret"], input.Code);
            var tokenInfo = await client.Oauth.CreateAccessToken(request);

            if (tokenInfo.AccessToken == null)
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Bad code")
                    .SetCode("AUTH_NOT_AUTHENTICATED")
                    .Build());
            }

            client.Credentials = new Credentials(tokenInfo.AccessToken);
            var user = await client.User.Current();

            var u = await context.Users.FirstOrDefaultAsync(s => s.GitHub == user.Login, cancellationToken);

            if (u == null)
            {

                u = new Models.User
                {
                    Name = user.Name ?? user.Login,
                    GitHub = user.Login,
                    ImageURI = user.AvatarUrl,
                };

                context.Users.Add(u);
                await context.SaveChangesAsync(cancellationToken);
            }

            // authentication successful so generate jwt token
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>{
                new Claim("userId", u.Id.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                "mhyphen",
                "mhyphen-user",
                claims,
                expires: DateTime.Now.AddDays(90),
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new LoginPayload(u, token);
        }
    }
}