using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services;

namespace PharmacyManagementSystem.Services
{
    public class MedicineAnalysisService : IMedicineAnalysisService
    {
        private readonly AppDbContext _context;
        private const int LOW_STOCK_THRESHOLD = 10;
        private const int HOT_SELLING_THRESHOLD = 5;

        public MedicineAnalysisService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<int> GetRecentSalesCountAsync(int medicineId)
        {
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            return await _context.BillDetails
                .Where(bd => bd.MedId == medicineId && bd.Bill.BillDate >= sevenDaysAgo)
                .SumAsync(bd => bd.Quantity);
        }

        private string GetStatus(int recentSales, int currentStock, out string badgeColor)
        {
            badgeColor = "success";

            if (recentSales >= HOT_SELLING_THRESHOLD && currentStock < LOW_STOCK_THRESHOLD)
            {
                badgeColor = "danger";
                return "CRITICAL";
            }

            if (recentSales >= HOT_SELLING_THRESHOLD)
            {
                badgeColor = "info";
                return "HOT_SELLING";
            }

            if (currentStock < LOW_STOCK_THRESHOLD)
            {
                badgeColor = "warning";
                return "LOW_STOCK";
            }

            badgeColor = "success";
            return "NORMAL";
        }

        public async Task<List<MedicineBadge>> GetMedicineBadgesAsync()
        {
            var medicines = await _context.Medicines.ToListAsync();
            var badges = new List<MedicineBadge>();

            foreach (var medicine in medicines)
            {
                var recentSales = await GetRecentSalesCountAsync(medicine.MedicineId);
                var status = GetStatus(recentSales, medicine.Quantity, out string color);

                badges.Add(new MedicineBadge
                {
                    MedicineId = medicine.MedicineId,
                    MedicineName = medicine.Name,
                    RecentSalesCount = recentSales,
                    CurrentStock = medicine.Quantity,
                    Status = status,
                    BadgeColor = color
                });
            }

            return badges;
        }

        public async Task<MedicineBadge> GetMedicineBadgeAsync(int medicineId)
        {
            var medicine = await _context.Medicines.FindAsync(medicineId);
            if (medicine == null) return null;

            var recentSales = await GetRecentSalesCountAsync(medicineId);
            var status = GetStatus(recentSales, medicine.Quantity, out string color);

            return new MedicineBadge
            {
                MedicineId = medicine.MedicineId,
                MedicineName = medicine.Name,
                RecentSalesCount = recentSales,
                CurrentStock = medicine.Quantity,
                Status = status,
                BadgeColor = color
            };
        }

        public async Task<string> AnalyzeMedicineAsync(string query)
        {
            query = query.ToLower();

            if (query.Contains("low stock") || query.Contains("reorder"))
            {
                var lowStockMeds = await _context.Medicines
                    .Where(m => m.Quantity < LOW_STOCK_THRESHOLD)
                    .ToListAsync();

                if (lowStockMeds.Count == 0)
                    return "All medicines have sufficient stock.";

                return $"Low stock medicines ({lowStockMeds.Count}):\n" +
                       string.Join("\n", lowStockMeds.Select(m =>
                           $"- {m.Name}: {m.Quantity} units"));
            }

            if (query.Contains("hot selling") || query.Contains("popular") || query.Contains("selling"))
            {
                var badges = await GetMedicineBadgesAsync();
                var hotSelling = badges.Where(b => b.Status == "HOT_SELLING" || b.Status == "CRITICAL").ToList();

                if (hotSelling.Count == 0)
                    return "No hot-selling medicines at the moment.";

                return $"Hot-selling medicines ({hotSelling.Count}):\n" +
                       string.Join("\n", hotSelling.Select(b =>
                           $"- {b.MedicineName}: {b.RecentSalesCount} sold, {b.CurrentStock} stock"));
            }

            if (query.Contains("critical"))
            {
                var badges = await GetMedicineBadgesAsync();
                var critical = badges.Where(b => b.Status == "CRITICAL").ToList();

                if (critical.Count == 0)
                    return "No critical medicines.";

                return $"Critical medicines ({critical.Count}):\n" +
                       string.Join("\n", critical.Select(b =>
                           $"- {b.MedicineName}: {b.CurrentStock} left (High demand)"));
            }

            if (query.Contains("stock") && query.Contains("status"))
            {
                var badges = await GetMedicineBadgesAsync();
                return $"Stock Status:\nTotal: {badges.Count}\nCritical: {badges.Count(b => b.Status == "CRITICAL")}\n" +
                       $"Hot-selling: {badges.Count(b => b.Status == "HOT_SELLING")}\n" +
                       $"Low stock: {badges.Count(b => b.Status == "LOW_STOCK")}\n" +
                       $"Normal: {badges.Count(b => b.Status == "NORMAL")}";
            }

            if (query.Contains("today") || query.Contains("sales"))
            {
                var today = DateTime.Now.Date;
                var todaysSales = await _context.Bills
                    .Where(b => b.BillDate.Date == today)
                    .ToListAsync();

                if (todaysSales.Count == 0)
                    return "No sales today.";

                var totalSales = todaysSales.Sum(b => b.TotalAmount);
                return $"Today's Sales:\nBills: {todaysSales.Count}\nRevenue: Rs. {totalSales}\n" +
                       $"Average: Rs. {(totalSales / todaysSales.Count):F2}";
            }

            return "Ask me about: low stock, hot selling, critical medicines, stock status, or today's sales";
        }
    }
}
