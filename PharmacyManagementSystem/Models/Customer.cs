using System.ComponentModel.DataAnnotations;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int CusId { get; set; }
        [Required]
        public string CusName { get; set; }
        [Required]
        public string Phone { get; set; }

        public List<Bill> Bills { get; set; } = new List<Bill>();
    }
}
