using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PharmacyManagementSystem.Models;
using System;
using System.Collections.Generic;
namespace PharmacyManagementSystem.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }
        [Required]
        public int CusId { get; set; }
        [Required]
        public int EmpId { get; set; }
        [Required]
        public DateTime BillDate { get; set; }
        [Required]
        public float TotalAmount { get; set; }
        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
        public List<BillDetails> BillItems { get; set; } = new List<BillDetails>();
        
    }
}
