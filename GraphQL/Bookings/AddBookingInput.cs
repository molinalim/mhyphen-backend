﻿using System;
using mhyphen.Models;

namespace mhyphen.GraphQL.Bookings
{
    public record AddBookingInput(
        string MovieId,
        string UserId,
        double Price,
        DateTime Booked,
        DateTime Created,
        Theater Theater
        );
}