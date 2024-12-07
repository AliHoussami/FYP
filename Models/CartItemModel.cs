using System.ComponentModel.DataAnnotations;

namespace TEST2.Models
{
    public class CartItemModel
    {
        [Key]
        public int CartItemId { get; set; } // Auto-generated
        public int MenuItemId { get; set; }
        public string ItemName { get; set; } = string.Empty; // Default to an empty string
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1; // Default quantity is 1
        public string? Customizations { get; set; }
    }

}
//CHAT

//Please return only the full classes fixed code WITHOUT DOING ANY HARM Or CHANGING OLD CODE 
