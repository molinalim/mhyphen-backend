using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using mhyphen.Data;
using mhyphen.Models;
using mhyphen.GraphQL.Users;
using mhyphen.GraphQL.Bookings;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace mhyphen.GraphQL.Movies
{
    public class MovieType : ObjectType<Movie>
    {
        protected override void Configure(IObjectTypeDescriptor<Movie> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
            descriptor.Field(p => p.Title).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Plot).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.ImageURL).Type<NonNullType<StringType>>();

            descriptor
                .Field(p => p.Bookings)
                .ResolveWith<Resolvers>(r => r.GetBookings(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<ListType<NonNullType<BookingType>>>>();

            descriptor.Field(p => p.Runtime).Type<NonNullType<IntType>>();

        }


        private class Resolvers
        {
            public async Task<IEnumerable<Booking>> GetBookings(Movie movie, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Bookings.Where(c => c.MovieId == movie.Id).ToArrayAsync(cancellationToken);
            }
        }
    }
}
