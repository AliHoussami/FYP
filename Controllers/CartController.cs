using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEST2.Models;

namespace TEST2.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly YourDbContext _context;

        public CartController(YourDbContext context)
        {
            _context = context;
        }

        // Render the Cart view with cart items
        [HttpGet]
        public IActionResult Index()
        {
            // Fetch all cart items from the database
            var cartItems = _context.CartItems.ToList();
            return View(cartItems);
        }

        // Get all cart items as JSON (API Endpoint)
        [HttpGet("items")]
        public IActionResult GetCartItems()
        {
            var cartItems = _context.CartItems.ToList();
            return Ok(cartItems);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddToCart([FromBody] CartItemModel cartItem)
        {
            if (cartItem == null || cartItem.MenuItemId <= 0 || cartItem.Quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid cart item data." });
            }

            var menuItem = _context.MenuItems.Find(cartItem.MenuItemId);
            if (menuItem == null)
            {
                return NotFound(new { success = false, message = "Menu item not found." });
            }

            // Adding the item with customizations
            var newCartItem = new CartItemModel
            {
                MenuItemId = cartItem.MenuItemId,
                ItemName = menuItem.ItemName,
                Price = menuItem.Price,
                Quantity = cartItem.Quantity,
                Customizations = cartItem.Customizations
            };

            _context.CartItems.Add(newCartItem);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Item added to cart." });
        }

        [HttpGet("menu/{menuItemId}")]
        public IActionResult GetMenuItemDetails(int menuItemId)
        {
            var menuItem = _context.MenuItems.FirstOrDefault(m => m.MenuItemId == menuItemId);

            if (menuItem == null)
                return NotFound(new { success = false, message = "Menu item not found." });

            return Ok(new
            {
                menuItem.MenuItemId,
                menuItem.ItemName,
                menuItem.Description,
                menuItem.Price,
                menuItem.CustomizationOptions // Will be returned as a List<string>
            });
        }

        // Update item quantity in the cart
        [HttpPut("update/{cartItemId}")]
        public IActionResult UpdateCartItem(int cartItemId, [FromBody] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Quantity must be greater than zero." });
            }

            var cartItem = _context.CartItems.Find(cartItemId);
            if (cartItem == null)
            {
                return NotFound(new { success = false, message = "Cart item not found." });
            }

            cartItem.Quantity = quantity;
            _context.SaveChanges();

            return Ok(new { success = true, message = "Cart item updated." });
        }

        // Remove an item from the cart
        [HttpDelete("remove/{cartItemId}")]
        public IActionResult RemoveCartItem(int cartItemId)
        {
            var cartItem = _context.CartItems.Find(cartItemId);
            if (cartItem == null)
            {
                return NotFound(new { success = false, message = "Cart item not found." });
            }

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Cart item removed." });
        }

        // Clear the cart
        [HttpDelete("clear")]
        public IActionResult ClearCart()
        {
            var cartItems = _context.CartItems.ToList();    
            if (!cartItems.Any())
            {
                return BadRequest(new { success = false, message = "Cart is already empty." });
            }

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Cart cleared." });
        }
    }
}
