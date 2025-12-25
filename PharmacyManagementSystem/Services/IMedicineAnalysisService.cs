namespace PharmacyManagementSystem.Services
{
    public interface IMedicineAnalysisService
    {
        
            Task<List<MedicineBadge>> GetMedicineBadgesAsync();
            Task<MedicineBadge> GetMedicineBadgeAsync(int medicineId);
            Task<string> AnalyzeMedicineAsync(string query);
        
    }
}
