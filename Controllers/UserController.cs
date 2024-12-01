using Microsoft.AspNetCore.Mvc;
using TEST2.Models;

namespace TEST2.Controllers
{
    public class UserController : Controller
    {
        private readonly YourDbContext _context;

        public UserController(YourDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Users> users = _context.Users.ToList(); // Fetch users from the database
            return View(users);
        }
    }
}
