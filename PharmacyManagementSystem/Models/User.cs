using System.ComponentModel.DataAnnotations;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public int EmployeeId { get; set; } // Foreign key

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } // Ideally hashed hona chahiye

        [Required]
        public int AccessLevel { get; set; } // 1=Admin, 2=Pharmacist, 3=Cashier

        public Employee Employee { get; set; }
    }
}
