using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Authorization;
using mhyphen.Models;
using mhyphen.Data;
using mhyphen.Extensions;

namespace mhyphen.GraphQL.Movies
{
    [ExtendObjectType(name: "Mutation")]
    public class MovieMutations
    {
        [UseAppDbContext]
        [Authorize]
        public async Task<Movie> AddMovieAsync(AddMovieInput input, ClaimsPrincipal claimsPrincipal,
            [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            //var userIdStr = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;
            var movie = new Movie
            {
                Title = input.Title,
                Plot = input.Plot,
                ImageURL = input.ImageURL,
                Rating = input.Rating,
                Genre = input.Genre,
                Runtime = input.Runtime
            };
            context.Movies.Add(movie);

            await context.SaveChangesAsync(cancellationToken);

            return movie;
        }

        [UseAppDbContext]
        [Authorize]
        public async Task<Movie> EditMovieAsync(EditMovieInput input, ClaimsPrincipal claimsPrincipal,
            [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var title = claimsPrincipal.Claims.First(c => c.Type == "title").Value;
            var movie = await context.Movies.FindAsync(int.Parse(input.MovieId));

            if (movie.Title != title)
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Movie does not exist")
                    .SetCode("AUTH_NOT_AUTHORIZED")
                    .Build());
            }

            movie.Title = input.Title ?? movie.Title;
            movie.Plot = input.Plot ?? movie.Plot;
            movie.ImageURL = input.ImageURL ?? movie.ImageURL;
            movie.Rating = input.Rating ?? movie.Rating;
            movie.Runtime = input.Runtime ?? movie.Runtime;
            movie.Genre = input.Genre ?? movie.Genre;

            await context.SaveChangesAsync(cancellationToken);

            return movie;
        }
    }
}
