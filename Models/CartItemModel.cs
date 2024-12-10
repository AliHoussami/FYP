using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace TEST2.Models
{
    public class CartItemModel
    {
        [Key]
        public int CartItemId { get; set; } // Auto-generated

        public int MenuItemId { get; set; }

        public string ItemName { get; set; } = string.Empty; // Default to an empty string

        public decimal? Price { get; set; }

        public int Quantity { get; set; } = 1; // Default quantity is 1

        public string? Customizations { get; set; } // Added to store customizations
    }
}

//CHAT

//Please return only the full classes fixed code WITHOUT DOING ANY HARM Or CHANGING OLD CODE 
//i need to add an admin panel interface, (Button in the LoginSignup.cshtml helping access the admin interface), where admins can add, edit, update and delete restaurants, restaurant menus, menu items or deliver and staff.
//Please create the model as well as its database table(MySQL workbench), the view and the controller needed for this task.