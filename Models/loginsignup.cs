using System.ComponentModel.DataAnnotations;

namespace TEST2.Models
{
    public class loginsignup
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        // Only required for signup
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }

        // Indicate whether this is a signup or login request
        public bool IsSignup { get; set; }
    }
}
