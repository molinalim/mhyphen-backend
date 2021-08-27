using HotChocolate.Types;

namespace mhyphen.GraphQL.Bookings
{
    public record EditBookingInput(
        [HotChocolate.GraphQLType(typeof(NonNullType<IdType>))]
        string BookingId,
        string? MovieId);
}
