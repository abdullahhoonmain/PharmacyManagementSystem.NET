using System.ComponentModel.DataAnnotations;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string BatchNo { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public float PurchasePrice { get; set; }
        [Required]
        public float SalePrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int? SupplierId { get; set; }
       public Supplier? Supplier { get; set; }


    }
}
