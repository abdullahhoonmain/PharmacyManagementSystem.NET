namespace PharmacyManagementSystem.Services
{
    public class MedicineBadge
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int RecentSalesCount { get; set; }
        public int CurrentStock { get; set; }
        public string Status { get; set; }
        public string BadgeColor { get; set; }
    }
}
