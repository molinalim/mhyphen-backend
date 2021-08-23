using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Types;
using mhyphen.Data;
using mhyphen.Models;
using mhyphen.GraphQL.Movies;
using mhyphen.GraphQL.Bookings;

namespace mhyphen.GraphQL.Users
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(s => s.Id).Type<NonNullType<IdType>>();
            descriptor.Field(s => s.Name).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.Password).Type<NonNullType<StringType>>();

            descriptor
                .Field(s => s.Movies)
                .ResolveWith<Resolvers>(r => r.GetMovies(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<ListType<NonNullType<MovieType>>>>();

            descriptor
                .Field(s => s.Bookings)
                .ResolveWith<Resolvers>(r => r.GetBookings(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<ListType<NonNullType<BookingType>>>>();
        }

        private class Resolvers
        {
            public async Task<IEnumerable<Movie>> GetMovies(User user, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Movies.Where(c => c.Id == user.Id).ToArrayAsync(cancellationToken);
            }

            public async Task<IEnumerable<Booking>> GetBookings(User user, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Bookings.Where(c => c.Id == user.Id).ToArrayAsync(cancellationToken);
            }
        }
    }
}