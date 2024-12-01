using System.ComponentModel.DataAnnotations;

namespace TEST2.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string UserType { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }
    }
}
