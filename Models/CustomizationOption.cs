using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST2.Models
{
    public class CustomizationOption
    {
        [Key]
        public int CustomizationId { get; set; }

        [Required]
        public int MenuItemId { get; set; } // Foreign key linking to the MenuItemModel

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // The name of the customization (e.g., "Extra Cheese")

        public bool IsOptional { get; set; } // Indicates if the option is optional

        public bool IsExtra { get; set; } // Indicates if it adds additional cost

        [Range(0, 100)]
        public decimal? ExtraPrice { get; set; } // Additional price for this customization

        // Navigation property back to MenuItemModel
        [ForeignKey("MenuItemId")]
        public MenuItemModel MenuItem { get; set; }
    }
}
