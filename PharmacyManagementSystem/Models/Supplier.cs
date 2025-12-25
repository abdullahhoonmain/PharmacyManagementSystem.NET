using System.ComponentModel.DataAnnotations;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        [Required]
        public string SupplierName { get; set; }
        [Required]
        public string ContactPerson { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();

    }
}
