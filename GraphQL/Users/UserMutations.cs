using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using mhyphen.Models;
using mhyphen.Data;
using mhyphen.Extensions;
using mhyphen.GraphQL.Users;

namespace mhyphen.GraphQL.Users
{
    [ExtendObjectType(name: "Mutation")]
    public class UserMutations
    {
        [UseAppDbContext]
        public async Task<User> AddUserAsync(AddUserInput input,
        [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = input.Name,
                Password = input.Password,
            };

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }

        [UseAppDbContext]
        public async Task<User> EditUserAsync(EditUserInput input,
                [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(input.Name);

            user.Name = input.Name ?? user.Name;
            user.Password = input.Password ?? user.Password;

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}