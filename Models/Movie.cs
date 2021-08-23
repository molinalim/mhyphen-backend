using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mhyphen.Models
{
    public enum Year
    {
        YEAR_2021 = 2021,
        YEAR_2020 = 2020,
        YEAR_2019 = 2019,
        YEAR_2008 = 2008,
        YEAR_2016 = 2016,
        YEAR_2014 = 2014,
        YEAR_1995 = 1995,
        YEAR_1988 = 1988
    }

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
        public Year Year { get; set; }

        [Required]
        public int Runtime { get; set; }


        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}