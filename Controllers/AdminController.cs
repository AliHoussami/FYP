using Microsoft.AspNetCore.Mvc;
using TEST2.Models;
using System.Linq;

namespace TEST2.Controllers
{
    public class AdminController : Controller
    {
        private readonly YourDbContext _context;

        public AdminController(YourDbContext context)
        {
            _context = context;
        }

        // Combined Restaurant Management View
        public IActionResult RestaurantManagement(int? id)
        {
            ViewBag.Restaurants = _context.Restaurants.ToList();

            if (id.HasValue)
            {
                ViewBag.EditRestaurant = _context.Restaurants.Find(id);
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddRestaurant(RestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Restaurants.Add(model);
                _context.SaveChanges();
            }
            return RedirectToAction("RestaurantManagement");
        }

        [HttpPost]
        public IActionResult EditRestaurant(RestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Restaurants.Update(model);
                _context.SaveChanges();
            }
            return RedirectToAction("RestaurantManagement");
        }

        [HttpPost]
        public IActionResult DeleteRestaurant(int id)
        {
            var restaurant = _context.Restaurants.Find(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                _context.SaveChanges();
            }
            return RedirectToAction("RestaurantManagement");
        }
    }
}
