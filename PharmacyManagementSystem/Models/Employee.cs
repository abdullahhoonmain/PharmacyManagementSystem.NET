using System.ComponentModel.DataAnnotations;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; } // Admin / Pharmacist / Cashier
        [Required]
        public string Phone { get; set; }
        public List<Bill> Bills { get; set; } = new List<Bill>();
        public User User { get; set; }

    }
}
