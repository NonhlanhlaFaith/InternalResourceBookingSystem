using System;
using System.ComponentModel.DataAnnotations;

namespace InternalResourceBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string BookedBy { get; set; }

        [Required]
        [StringLength(200)]
        public string Purpose { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        // Foreign key
        [Required]
        public int ResourceId { get; set; }

        // Navigation property
        public Resource? Resource { get; set; }
    }
}
