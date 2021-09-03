using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using mhyphen.Data;
using mhyphen.Models;
using mhyphen.Extensions;

namespace mhyphen.GraphQL.Bookings
{
    [ExtendObjectType(name: "Query")]
    public class BookingQueries
    {
        [UseAppDbContext]
        [UsePaging]
        public IQueryable<Booking> GetBookings([ScopedService] AppDbContext context)
        {
            return context.Bookings.OrderBy(c => c.Created);
        }

        [UseAppDbContext]
        public Booking GetBooking(int id, [ScopedService] AppDbContext context)
        {
            return context.Bookings.Find(id);
        }

        [UseAppDbContext]
        [UsePaging]
        public IQueryable<Booking> GetBookingsByUserId(int userId, [ScopedService] AppDbContext context)
        {
            return context.Bookings.Where(b => b.UserId == userId).OrderBy(c => c.Created); 
        }
    }
}
