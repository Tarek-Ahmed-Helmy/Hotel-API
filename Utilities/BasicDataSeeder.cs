using Domain.Interfaces;
using Domain.Models;

namespace Utilities;

public class BasicDataSeeder
{
    public static async Task SeedBasicDataAsync(IUnitOfWork unitOfWork)
    {
        var existingStatuses = await unitOfWork.Status.GetAllAsync();
        if (existingStatuses.Any()) return;

        var statuses = new List<Status>
        {
            new() { NameAr = "مفتوح", NameEn = "Open" },
            new() { NameAr = "قيد المراجعة", NameEn = "In Review" },
            new() { NameAr = "تم التخصيص", NameEn = "Assigned" },
            new() { NameAr = "مكتمل", NameEn = "Completed" },
            new() { NameAr = "ملغي", NameEn = "Rejected" }
        };

        await unitOfWork.Status.AddRangeAsync(statuses);
        await unitOfWork.SaveChangesAsync();
    }
}
