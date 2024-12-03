using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEST2.Models;

namespace TEST2.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly YourDbContext _context;
        public RestaurantController(YourDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/restaurants")]
        public IActionResult GetRestaurants()
        {
            var restaurants = _context.Restaurants.ToList();
            return Ok(restaurants);
        }
        [HttpGet]
        [Route("api/restaurants/trending")]
        public IActionResult GetTrendingRestaurants()
        {
            var trendingRestaurants = _context.Restaurants
                                               .OrderByDescending(r => r.Rating)
                                               .Take(3)
                                               .Select(r => new
                                               {
                                                   r.RestaurantId,
                                                   r.RestaurantName,
                                                   r.Description,
                                                   r.Rating,
                                                   r.ImageUrl // Include ImageUrl here
                                               })
                                               .ToList();

            return Ok(trendingRestaurants);
        }


        [HttpGet]
        [Route("api/restaurants/Menu{restaurantId}")]
        public IActionResult Menu(int restaurantId)
        {
            var restaurant = _context.Restaurants.Find(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            var menuItems = _context.MenuItems
                                    .Where(m => m.RestaurantId == restaurantId)
                                    .ToList();

            // Add example customizations for each menu item
            foreach (var item in menuItems)
            {
                item.CustomizationOptions = new List<CustomizationOption>
        {
            new CustomizationOption { Name = "Extra Cheese", IsOptional = true, IsExtra = true },
            new CustomizationOption { Name = "No Sauce", IsOptional = true, IsExtra = false },
            new CustomizationOption { Name = "Spicy", IsOptional = true, IsExtra = true }
        };
            }

            ViewBag.Restaurant = restaurant;
            return View("~/Views/Account/Menu.cshtml", menuItems);
        }
    }
}