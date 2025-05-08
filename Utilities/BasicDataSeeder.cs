using Domain.Interfaces;
using Domain.Models;

namespace Utilities;

public class BasicDataSeeder
{
    public static async Task SeedBasicDataAsync(IUnitOfWork unitOfWork)
    {
        var existingStatuses = await unitOfWork.Status.GetAllAsync();
        if (!existingStatuses.Any())
        {
            var statuses = new List<Status>
            {
                new() { NameAr = "مفتوح", NameEn = "Open" },
                new() { NameAr = "قيد المراجعة", NameEn = "In Review" },
                new() { NameAr = "تم التخصيص", NameEn = "Assigned" },
                new() { NameAr = "مكتمل", NameEn = "Completed" },
                new() { NameAr = "ملغي", NameEn = "Rejected" }
            };
            await unitOfWork.Status.AddRangeAsync(statuses);
        }


        var existingRoomTypes = await unitOfWork.RoomType.GetAllAsync();
        if (!existingRoomTypes.Any())
        {
            var roomTypes = new List<RoomType>
            {
                new() { NameAr = "غرفة", NameEn = "Room" },
                new() { NameAr = "شقة", NameEn = "Flat"}
            };
            await unitOfWork.RoomType.AddRangeAsync(roomTypes);
        }
        

        var existingUseTypes = await unitOfWork.UseType.GetAllAsync();
        if (!existingUseTypes.Any())
        {
            var useTypes = new List<UseType>
            {
                new() { NameAr = "قيد الاستخدام", NameEn = "In Use"},
                new() { NameAr = "تحت التنظيف", NameEn = "Under Cleaning"},
                new() { NameAr = "في الإصلاح", NameEn = "In Repair"}
            };
            await unitOfWork.UseType.AddRangeAsync(useTypes);
        }
        

        var existingServices = await unitOfWork.Service.GetAllAsync();
        if (!existingServices.Any())
        {
            var services = new List<Service>
            {
                new() { Id = 1, NameAr = "الصيانة", NameEn = "Maintenance", ServiceLevel = ServiceLevel.MainService, SuperServiceId = null },
                new() { Id = 2, NameAr = "المكيفات", NameEn = "Air conditioners", ServiceLevel = ServiceLevel.SubService, SuperServiceId = 1 },
                new() { Id = 3, NameAr = "عطل", NameEn = "Damage", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 2 },
                new() { Id = 4, NameAr = "صيانه", NameEn = "Maintenance", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 2 },
                new() { Id = 5, NameAr = "السباكه", NameEn = "Plumbing", ServiceLevel = ServiceLevel.SubService, SuperServiceId = 1 },
                new() { Id = 6, NameAr = "طلب سباك لصيانة الحمامات", NameEn = "Request a plumber for bathroom maintenance", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 5 },
                new() { Id = 7, NameAr = "طلب السباك لاصلاح عطل", NameEn = "Request a plumber to fix a fault", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 5 },
                new() { Id = 8, NameAr = "النظافة", NameEn = "Cleanliness", ServiceLevel = ServiceLevel.SubService, SuperServiceId = 1 },
                new() { Id = 9, NameAr = "تغير مفارش", NameEn = "Change the sheets", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 8},
                new() { Id = 10, NameAr = "نظافة عامة", NameEn = "General cleaning", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 8 },
                new() { Id = 11, NameAr = "طلب مكنسة كهربائية", NameEn = "Vacuum cleaner order", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 8 },
                new() { Id = 12, NameAr = "الصيانة العامة", NameEn = "General maintenance", ServiceLevel = ServiceLevel.SubService, SuperServiceId = 1 },
                new() { Id = 13, NameAr = "طلبات خاصة تتعلق بصيانة الوحدة", NameEn = "Special requests related to unit maintenance", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 12 },
                new() { Id = 14, NameAr = "التوصيل", NameEn = "Delivery", ServiceLevel = ServiceLevel.MainService, SuperServiceId = null },
                new() { Id = 15, NameAr = "طلبات التوصيل", NameEn = "Delivery requests", ServiceLevel = ServiceLevel.SubService, SuperServiceId = 14 },
                new() { Id = 16, NameAr = "طلبات البقالة و السوبر ماركت", NameEn = "Grocery and supermarket orders", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 15 },
                new() { Id = 17, NameAr = "طلبات المطاعم", NameEn = "Restaurant orders", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 15 },
                new() { Id = 18, NameAr = "المغسلة", NameEn = "The sink", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 15 },
                new() { Id = 19, NameAr = "مشوار خاص", NameEn = "Special Journey", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 15 },
                new() { Id = 20, NameAr = "طلب نقل داخلي", NameEn = "Internal transfer request", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 15 },
                new() { Id = 21, NameAr = "توصيل الي المطار", NameEn = "Airport transfer", ServiceLevel = ServiceLevel.ServiceComponent, SuperServiceId = 15 },
            };
            await unitOfWork.Service.AddRangeAsync(services);
        }
        await unitOfWork.SaveChangesAsync();
    }
}
