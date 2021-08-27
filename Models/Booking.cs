using System;
using System.ComponentModel.DataAnnotations;

namespace mhyphen.Models
{
    public enum Theater
    {
        Small, Medium, Large, Extra_Large
    }

    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Theater Theater { get; set; }

        [Required]
        public int MovieId { get; set; }

        public Movie Movie { get; set; } = default!;

        [Required]
        public double Price { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = default!;

        public DateTime Booked { get; set; }

        public DateTime Created { get; set; }
    }
}