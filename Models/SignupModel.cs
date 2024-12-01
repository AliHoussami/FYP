using System.ComponentModel.DataAnnotations;

namespace TEST2.Models
{
    public class SignupModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}
