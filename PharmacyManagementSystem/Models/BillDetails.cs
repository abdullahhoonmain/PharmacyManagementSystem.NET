using System.ComponentModel.DataAnnotations;    
namespace PharmacyManagementSystem.Models
{
    public class BillDetails
    {
        [Key]
        public int BillItemId { get; set; }
        [Required]
        public int BillId { get; set; }
        [Required]
        public int MedId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float UnitPrice { get; set; }
        [Required]
        public float Total { get; set; }
        public Bill Bill { get; set; }
        public Medicine Medicine { get; set; }
    }
}
