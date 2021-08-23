using System.Threading.Tasks;
using System.Threading;
using HotChocolate;
using HotChocolate.Types;
using mhyphen.Data;
using mhyphen.Models;
using mhyphen.GraphQL.Movies;

namespace mhyphen.GraphQL.Bookings
{
    public class BookingType : ObjectType<Booking>
    {
        protected override void Configure(IObjectTypeDescriptor<Booking> descriptor)
        {
            descriptor.Field(b => b.Id).Type<NonNullType<IdType>>();
            descriptor.Field(b => b.Price).Type<NonNullType<StringType>>();

            descriptor
                .Field(b => b.Movie)
                .ResolveWith<Resolvers>(r => r.GetMovie(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<BookingType>>();

            descriptor
                .Field(b => b.User)
                .ResolveWith<Resolvers>(r => r.GetUser(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<BookingType>>();

            descriptor.Field(p => p.Booked).Type<NonNullType<DateTimeType>>();
            descriptor.Field(p => p.Created).Type<NonNullType<DateTimeType>>();

        }

        private class Resolvers
        {
            public async Task<Movie> GetMovie(Booking booking, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Movies.FindAsync(new object[] { booking.MovieId }, cancellationToken);
            }

            public async Task<User> GetUser(Booking booking, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Users.FindAsync(new object[] { booking.UserId }, cancellationToken);
            }
        }
    }
}
