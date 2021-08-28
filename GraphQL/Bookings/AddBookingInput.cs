using System;
using HotChocolate;
using HotChocolate.Types;
using mhyphen.Models;

namespace mhyphen.GraphQL.Bookings
{
    public record AddBookingInput(
        [GraphQLType(typeof(NonNullType<IdType>))]
        string MovieId,
        DateTime Booked
        );
}
