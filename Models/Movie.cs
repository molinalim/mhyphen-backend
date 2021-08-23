using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mhyphen.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Plot { get; set; } = null!;

        [Required]
        public double Rating { get; set; } 

        [Required]
        public string Genre { get; set; } = null!;

        [Required]
        public string ImageURL { get; set; } = null!;

        [Required]
        public int Runtime { get; set; }


        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
