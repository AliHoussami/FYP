using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST2.Models
{
    public class MenuItemModel
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; }

        public bool Availability { get; set; }

        public string? ImageUrl { get; set; }

        // Navigation property to link menu items with their customization options
        public List<CustomizationOption> CustomizationOptions { get; set; } = new List<CustomizationOption>();
    }
}
