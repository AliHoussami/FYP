using System.ComponentModel.DataAnnotations;

namespace TEST2.Models
{
    public class RestaurantModel
    {
        [Key]
        public int RestaurantId { get; set; }
        [Required]
        [StringLength(100)]
        public string RestaurantName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(100)]
        public string OpeningHours { get; set; }

        [Range(0,5)]
        public decimal Rating { get; set; }
        public string ImageUrl { get; set; }
    }
}
