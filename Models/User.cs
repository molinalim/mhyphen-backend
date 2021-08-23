﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mhyphen.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}