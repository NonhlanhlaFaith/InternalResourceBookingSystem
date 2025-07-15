using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternalResourceBookingSystem.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!; // use null-forgiving operator or initialize to avoid nullable warnings

        public string? Description { get; set; }

        public string? Location { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; }

        // Initialize the collection to avoid null reference exceptions
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
