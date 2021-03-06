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

namespace mhyphen.GraphQL.Bookings
{
    [ExtendObjectType(name: "Mutation")]
    public class BookingMutations
    {
        [UseAppDbContext]
        [Authorize]
        public async Task<Booking> AddBookingAsync(AddBookingInput input, ClaimsPrincipal claimsPrincipal,
        [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var userIdStr = claimsPrincipal.Claims.First(b => b.Type == "userId").Value;
            var booking = new Booking
            {
                Price = 15.00,
                MovieId = int.Parse(input.MovieId),
                UserId = int.Parse(userIdStr),
                Booked = input.Booked,
                Created = DateTime.Now
            };
            context.Bookings.Add(booking);

            await context.SaveChangesAsync(cancellationToken);

            return booking;
        }

        [UseAppDbContext]
        [Authorize]
        public async Task<Booking> EditBookingAsync(EditBookingInput input, ClaimsPrincipal claimsPrincipal,
                [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var userIdStr = claimsPrincipal.Claims.First(b => b.Type == "userId").Value;
            var booking = await context.Bookings.FindAsync(int.Parse(input.BookingId));

            if (booking.UserId != int.Parse(userIdStr))
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Not booked by user")
                    .SetCode("AUTH_NOT_AUTHORIZED")
                    .Build());
            }
            if (input.MovieId != null) {
                booking.MovieId = int.Parse((input.MovieId) ?? (booking.MovieId).ToString());
            }
            
            await context.SaveChangesAsync(cancellationToken);

            return booking;
        }
    }
}
