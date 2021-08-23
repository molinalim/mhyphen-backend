namespace mhyphen.GraphQL.Bookings
{
    public record EditBookingInput(
        string BookingId,
        double? Price);
}
