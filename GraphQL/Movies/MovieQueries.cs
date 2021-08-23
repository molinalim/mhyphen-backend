using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using mhyphen.Data;
using mhyphen.Models;
using mhyphen.Extensions;

namespace mhyphen.GraphQL.Movies
{
    [ExtendObjectType(name: "Query")]
    public class MovieQueries
    {
        [UseAppDbContext]
        [UsePaging]
        public IQueryable<Movie> GetMovies([ScopedService] AppDbContext context)
        {
            return context.Movies.OrderBy(c => c.Title);
        }

        [UseAppDbContext]
        public Movie GetMovie(int id, [ScopedService] AppDbContext context)
        {
            return context.Movies.Find(id);
        }
    }
}
