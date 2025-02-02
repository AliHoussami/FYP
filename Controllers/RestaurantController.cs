﻿using Microsoft.AspNetCore.Mvc;
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
                return NotFound(); // Return 404 if the restaurant does not exist
            }

            var menuItems = _context.MenuItems
                                    .Where(m => m.RestaurantId == restaurantId)
                                    .ToList();

            ViewBag.Restaurant = restaurant; // Pass restaurant details to the view

            // Explicitly specify the view path
            return View("~/Views/Account/Menu.cshtml", menuItems);
        }      
    }
}