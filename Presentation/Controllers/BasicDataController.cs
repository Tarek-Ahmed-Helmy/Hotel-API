using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasicDataController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public BasicDataController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region Company

    [HttpPost("AddCompany")]
    public async Task<IActionResult> AddCompany(string lang, [FromBody] CreateOrUpdateCompanyDto company)
    {
        if (company == null) return BadRequest(new { Message = lang == "EN" ? "Company cannot be null." : "خطأ في الإدخال" });
        var newCompany = new Company
        {
            Name = company.Name,
            CountryId = company.CountryId,
        };
        await _unitOfWork.Company.AddAsync(newCompany);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newCompany);
    }

    [HttpGet("GetCompany/{id}")]
    public async Task<IActionResult> GetCompany(string lang, int id)
    {
        var company = await _unitOfWork.Company.FindAsync(c => c.Id == id, ["Country", "Hotels"]);
        if (company == null) return NotFound(new { Message = lang == "EN" ? "Company not found." : "الشركة غير موجودة" });
        var companyDto = new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            CountryName = company.Country?.Name ?? "N/A",
            CreatedAt = company.CreatedAt,
            UpdatedAt = company.UpdatedAt,
            Hotels = company.Hotels?.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name
            }).ToList()
        };
        return Ok(companyDto);
    }

    [HttpPut("EditCompany/{id}")]
    public async Task<IActionResult> EditCompany(string lang, int id, [FromBody] CreateOrUpdateCompanyDto updated)
    {
        var company = await _unitOfWork.Company.GetByIdAsync(id);
        if (company == null) return NotFound(new { Message = lang == "EN" ? "Company not found." : "الشركة غير موجودة" });
        company.Name = updated.Name;
        await _unitOfWork.Company.UpdateAsync(company);
        await _unitOfWork.SaveChangesAsync();
        return Ok(company);
    }

    [HttpDelete("RemoveCompany/{id}")]
    public async Task<IActionResult> RemoveCompany(string lang, int id)
    {
        var company = await _unitOfWork.Company.GetByIdAsync(id);
        if (company == null) return NotFound(new { Message = lang == "EN" ? "Company not found." : "الشركة غير موجودة" });
        await _unitOfWork.Company.DeleteAsync(company);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllCompanies")]
    public async Task<IActionResult> GetAllCompanies(string lang)
    {
        var companies = await _unitOfWork.Company.FindAllAsync(includes: ["Country"]);
        if (companies == null || !companies.Any()) return NotFound(new { Message = lang == "EN" ? "No companies found." : "لم يتم ايجاد شركات" });
        var companyDtos = companies.Select(c => new CompanyDto
        {
            Id = c.Id,
            Name = c.Name,
            CountryName = c.Country?.Name ?? "N/A",
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();
        return Ok(companyDtos);
    }

    [HttpGet("GenerateCompanies")]
    public async Task<IActionResult> GenerateCompanies(string lang)
    {
        var companies = await _unitOfWork.Company.GetAllAsync();
        if (companies == null || !companies.Any()) return NotFound(new { Message = lang == "EN" ? "No companies found." : "لم يتم ايجاد شركات" });
        var companyDtos = companies.Select(c => new SelectCompanyDTO
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
        return Ok(companyDtos);
    }

    #endregion

    #region Country

    [HttpPost("AddCountry")]
    public async Task<IActionResult> AddCountry(string lang, [FromBody] CreateOrUpdateCountryDto country)
    {
        if (country == null) return BadRequest(new { Message = lang == "EN" ? "Country cannot be null." : "خطأ في الإدخال" });
        var newCountry = new Country
        {
            Name = country.Name
        };
        await _unitOfWork.Country.AddAsync(newCountry);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newCountry);
    }

    [HttpGet("GetCountry/{id}")]
    public async Task<IActionResult> GetCountry(string lang, int id)
    {
        var country = await _unitOfWork.Country.FindAsync(c =>c.Id==id, ["Companies"]);
        if (country == null) return NotFound(new { Message = lang == "EN" ? "Country not found." : "الدولة غير موجودة" });
        var countryDto = new CountryDto
        {
            Id = country.Id,
            Name = country.Name,
            CreatedAt = country.CreatedAt,
            UpdatedAt = country.UpdatedAt,
            Companies = country.Companies?.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList()
        };
        return Ok(countryDto);
    }

    [HttpPut("EditCountry/{id}")]
    public async Task<IActionResult> EditCountry(string lang, int id, [FromBody] CreateOrUpdateCountryDto updated)
    {
        var country = await _unitOfWork.Country.GetByIdAsync(id);
        if (country == null) return NotFound(new { Message = lang == "EN" ? "Country not found." : "الدولة غير موجودة" });
        country.Name = updated.Name;
        await _unitOfWork.Country.UpdateAsync(country);
        await _unitOfWork.SaveChangesAsync();
        return Ok(country);
    }

    [HttpDelete("RemoveCountry/{id}")]
    public async Task<IActionResult> RemoveCountry(string lang, int id)
    {
        var country = await _unitOfWork.Country.GetByIdAsync(id);
        if (country == null) return NotFound(new { Message = lang == "EN" ? "Country not found." : "الدولة غير موجودة" });
        await _unitOfWork.Country.DeleteAsync(country);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllCountries")]
    public async Task<IActionResult> GetAllCountries(string lang)
    {
        var countries = await _unitOfWork.Country.GetAllAsync();
        if (countries == null || !countries.Any()) return NotFound(new { Message = lang == "EN" ? "No countries found." : "لم يتم ايجاد دول" });
        var countryDtos = countries.Select(c => new CountryDto
        {
            Id = c.Id,
            Name = c.Name,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();
        return Ok(countries);
    }

    [HttpGet("GenerateCountries")]
    public async Task<IActionResult> GenerateCountries(string lang)
    {
        var countries = await _unitOfWork.Country.GetAllAsync();
        if (countries == null || !countries.Any()) return NotFound(new { Message = lang == "EN" ? "No countries found." : "لم يتم ايجاد دول" });
        var countryDtos = countries.Select(c => new SelectCountryDTO
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
        return Ok(countryDtos);
    }

    #endregion

    #region Department

    [HttpPost("AddDepartment")]
    public async Task<IActionResult> AddDepartment(string lang, [FromBody] CreateOrUpdateDepartmentDto department)
    {
        if (department == null) return BadRequest(new { Message = lang == "EN" ? "Department cannot be null." : "خطأ في الإدخال" });
        var newDepartment = new Department
        {
            Name = department.Name
        };
        await _unitOfWork.Department.AddAsync(newDepartment);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newDepartment);
    }

    [HttpGet("GetDepartment/{id}")]
    public async Task<IActionResult> GetDepartment(string lang, int id)
    {
        var department = await _unitOfWork.Department.FindAsync(d => d.Id == id, ["Users"]);
        if (department == null) return NotFound(new { Message = lang == "EN" ? "Department not found." : "القسم غير موجود" });
        var departmentDto = new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt,
            Users = department.Users?.Select(u => new ApplicationUser
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
            }).ToList()
        };
        return Ok(departmentDto);
    }

    [HttpPut("EditDepartment/{id}")]
    public async Task<IActionResult> EditDepartment(string lang, int id, [FromBody] CreateOrUpdateDepartmentDto updated)
    {
        var department = await _unitOfWork.Department.GetByIdAsync(id);
        if (department == null) return NotFound(new { Message = lang == "EN" ? "Department not found." : "القسم غير موجود" });
        department.Name = updated.Name;
        await _unitOfWork.Department.UpdateAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return Ok(department);
    }

    [HttpDelete("RemoveDepartment/{id}")]
    public async Task<IActionResult> RemoveDepartment(string lang, int id)
    {
        var department = await _unitOfWork.Department.GetByIdAsync(id);
        if (department == null) return NotFound(new { Message = lang == "EN" ? "Department not found." : "القسم غير موجود" });
        await _unitOfWork.Department.DeleteAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllDepartments")]
    public async Task<IActionResult> GetAllDepartments(string lang)
    {
        var departments = await _unitOfWork.Department.GetAllAsync();
        if (departments == null || !departments.Any()) return NotFound(new { Message = lang == "EN" ? "No departments found." : "لم يتم ايجاد اقسام" });
        var departmentDtos = departments.Select(d => new DepartmentDto
        {
            Id = d.Id,
            Name = d.Name,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt
        }).ToList();
        return Ok(departments);
    }

    [HttpGet("GenerateDepartments")]
    public async Task<IActionResult> GenerateDepartments(string lang)
    {
        var departments = await _unitOfWork.Department.GetAllAsync();
        if (departments == null || !departments.Any()) return NotFound(new { Message = lang == "EN" ? "No departments found." : "لم يتم ايجاد اقسام" });
        var departmentDtos = departments.Select(d => new SelectDepartmentDTO
        {
            Id = d.Id,
            Name = d.Name
        }).ToList();
        return Ok(departmentDtos);
    }

    #endregion

    #region Hotel

    [HttpPost("AddHotel")]
    public async Task<IActionResult> AddHotel(string lang, [FromBody] CreateOrUpdateHotelDto hotel)
    {
        if (hotel == null) return BadRequest(new { Message = lang == "EN" ? "Hotel cannot be null." : "خطأ في الإدخال" });
        var newHotel = new Hotel
        {
            Name = hotel.Name,
            CompanyId = hotel.CompanyId
        };
        await _unitOfWork.Hotel.AddAsync(newHotel);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newHotel);
    }

    [HttpGet("GetHotel/{id}")]
    public async Task<IActionResult> GetHotel(string lang, int id)
    {
        var hotel = await _unitOfWork.Hotel.FindAsync(hotel => hotel.Id == id, ["Company", "Rooms"]);
        if (hotel == null) return NotFound(new { Message = lang == "EN" ? "Hotel not found." : "الفندق / العمارة غير موجودة" });
        var hotelDto = new HotelDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            CompanyName = hotel.Company?.Name ?? "N/A",
            CreatedAt = hotel.CreatedAt,
            UpdatedAt = hotel.UpdatedAt,
            Rooms = hotel.Rooms?.Select(r => new RoomDto
            {
                Id = r.Id,
                Number = r.Number
            }).ToList()
        };
        return Ok(hotelDto);
    }

    [HttpPut("EditHotel/{id}")]
    public async Task<IActionResult> EditHotel(string lang, int id, [FromBody] CreateOrUpdateHotelDto updated)
    {
        var hotel = await _unitOfWork.Hotel.GetByIdAsync(id);
        if (hotel == null) return NotFound(new { Message = lang == "EN" ? "Hotel not found." : "الفندق / العمارة غير موجودة" });
        hotel.Name = updated.Name;
        hotel.CompanyId = updated.CompanyId;
        await _unitOfWork.Hotel.UpdateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();
        return Ok(hotel);
    }

    [HttpDelete("RemoveHotel/{id}")]
    public async Task<IActionResult> RemoveHotel(string lang, int id)
    {
        var hotel = await _unitOfWork.Hotel.GetByIdAsync(id);
        if (hotel == null) return NotFound(new { Message = lang == "EN" ? "Hotel not found." : "الفندق / العمارة غير موجودة" });
        await _unitOfWork.Hotel.DeleteAsync(hotel);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllHotels")]
    public async Task<IActionResult> GetAllHotels(string lang)
    {
        var hotels = await _unitOfWork.Hotel.FindAllAsync(includes: ["Company"]);
        if (hotels == null || !hotels.Any()) return NotFound(new { Message = lang == "EN" ? "No hotels found." : "لم يتم ايجاد فنادق" });
        var hotelDtos = hotels.Select(h => new HotelDto
        {
            Id = h.Id,
            Name = h.Name,
            CompanyName = h.Company?.Name ?? "N/A",
            CreatedAt = h.CreatedAt,
            UpdatedAt = h.UpdatedAt
        }).ToList();
        return Ok(hotelDtos);
    }

    [HttpGet("GenerateHotels")]
    public async Task<IActionResult> GenerateHotels(string lang)
    {
        var hotels = await _unitOfWork.Hotel.GetAllAsync();
        if (hotels == null || !hotels.Any()) return NotFound(new { Message = lang == "EN" ? "No hotels found." : "لم يتم ايجاد فنادق" });
        var hotelDtos = hotels.Select(h => new SelectHotelDTO
        {
            Id = h.Id,
            Name = h.Name
        }).ToList();
        return Ok(hotelDtos);
    }

    #endregion

    #region Room

    [HttpPost("AddRoom")]
    public async Task<IActionResult> AddRoom(string lang, [FromBody] CreateOrUpdateRoomDto room)
    {
        if (room == null) return BadRequest(new { Message = lang == "EN" ? "Room cannot be null." : "خطأ في الإدخال" });
        var newRoom = new Room
        {
            Number = room.Number,
            UseTypeId = room.UseTypeId,
            RoomTypeId = room.RoomTypeId,
            HotelId = room.HotelId
        };
        await _unitOfWork.Room.AddAsync(newRoom);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newRoom);
    }

    [HttpGet("GetRoom/{id}")]
    public async Task<IActionResult> GetRoom(string lang, int id)
    {
        var room = await _unitOfWork.Room.FindAsync(r => r.Id == id, ["UseType", "RoomType", "Hotel"]);
        if (room == null) return NotFound(new { Message = lang == "EN" ? "Room not found." : "الغرفة غير موجودة" });
        var roomDto = new RoomDto
        {
            Id = room.Id,
            Number = room.Number,
            IsAvailable = room.IsAvailable,
            UseTypeNameAr = room.UseType?.NameAr ?? "N/A",
            UseTypeNameEn = room.UseType?.NameEn ?? "N/A",
            RoomTypeNameAr = room.RoomType?.NameAr ?? "N/A",
            RoomTypeNameEn = room.RoomType?.NameEn ?? "N/A",
            HotelName = room.Hotel?.Name ?? "N/A",
            CreatedAt = room.CreatedAt,
            UpdatedAt = room.UpdatedAt,
            UseTypeId = room.UseTypeId,
            RoomTypeId = room.RoomTypeId,
            HotelId = room.HotelId
        };
        return Ok(roomDto);
    }

    [HttpPut("EditRoom/{id}")]
    public async Task<IActionResult> EditRoom(string lang, int id, [FromBody] CreateOrUpdateRoomDto updated)
    {
        var room = await _unitOfWork.Room.GetByIdAsync(id);
        if (room == null) return NotFound(new { Message = lang == "EN" ? "Room not found." : "الغرفة غير موجودة" });
        room.Number = updated.Number;
        room.UseTypeId = updated.UseTypeId;
        room.RoomTypeId = updated.RoomTypeId;
        room.HotelId = updated.HotelId;
        await _unitOfWork.Room.UpdateAsync(room);
        await _unitOfWork.SaveChangesAsync();
        return Ok(room);
    }

    [HttpDelete("RemoveRoom/{id}")]
    public async Task<IActionResult> RemoveRoom(string lang, int id)
    {
        var room = await _unitOfWork.Room.GetByIdAsync(id);
        if (room == null) return NotFound(new { Message = lang == "EN" ? "Room not found." : "الغرفة غير موجودة" });
        await _unitOfWork.Room.DeleteAsync(room);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllRooms")]
    public async Task<IActionResult> GetAllRooms(string lang)
    {
        var rooms = await _unitOfWork.Room.FindAllAsync(includes: ["UseType", "RoomType", "Hotel"]);
        if (rooms == null || !rooms.Any()) return NotFound(new { Message = lang == "EN" ? "No rooms found." : "لم يتم ايجاد غرف" });
        var roomDtos = rooms.Select(r => new RoomDto
        {
            Id = r.Id,
            Number = r.Number,
            IsAvailable = r.IsAvailable,
            UseTypeNameAr = r.UseType?.NameAr ?? "N/A",
            UseTypeNameEn = r.UseType?.NameEn ?? "N/A",
            RoomTypeNameAr = r.RoomType?.NameAr ?? "N/A",
            RoomTypeNameEn = r.RoomType?.NameEn ?? "N/A",
            HotelName = r.Hotel?.Name ?? "N/A",
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
            UseTypeId = r.UseTypeId,
            RoomTypeId = r.RoomTypeId,
            HotelId = r.HotelId
        }).ToList();
        return Ok(roomDtos);
    }

    [HttpGet("GenerateRooms")]
    public async Task<IActionResult> GenerateRooms(string lang, int hotelId)
    {
        var rooms = await _unitOfWork.Room.FindAllAsync(r=>r.HotelId == hotelId);
        if (rooms == null || !rooms.Any()) return NotFound(new { Message = lang == "EN" ? "No rooms found." : "لم يتم ايجاد غرف" });
        var roomDtos = rooms.Select(r => new SelectRoomDTO
        {
            Id = r.Id,
            Number = r.Number
        }).ToList();
        return Ok(roomDtos);
    }

    #endregion

    #region RoomType

    [HttpPost("AddRoomType")]
    public async Task<IActionResult> AddRoomType(string lang, [FromBody] CreateOrUpdateRoomTypeDto roomType)
    {
        if (roomType == null) return BadRequest(new { Message = lang == "EN" ? "RoomType cannot be null." : "خطأ في الإدخال" });
        var newRoomType = new RoomType
        {
            NameAr = roomType.NameAr,
            NameEn = roomType.NameEn
        };
        await _unitOfWork.RoomType.AddAsync(newRoomType);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newRoomType);
    }

    [HttpGet("GetRoomType/{id}")]
    public async Task<IActionResult> GetRoomType(string lang, int id)
    {
        var roomType = await _unitOfWork.RoomType.FindAsync(r => r.Id == id, ["Rooms"]);
        if (roomType == null) return NotFound(new { Message = lang == "EN" ? "RoomType not found." : "نوع الغرفة غير موجود" });
        var roomTypeDto = new RoomTypeDto
        {
            Id = roomType.Id,
            NameAr = roomType.NameAr,
            NameEn = roomType.NameEn,
            CreatedAt = roomType.CreatedAt,
            UpdatedAt = roomType.UpdatedAt,
            Rooms = roomType.Rooms?.Select(r => new RoomDto
            {
                Id = r.Id,
                Number = r.Number
            }).ToList()
        };
        return Ok(roomTypeDto);
    }

    [HttpPut("EditRoomType/{id}")]
    public async Task<IActionResult> EditRoomType(string lang, int id, [FromBody] CreateOrUpdateRoomTypeDto updated)
    {
        var roomType = await _unitOfWork.RoomType.GetByIdAsync(id);
        if (roomType == null) return NotFound(new { Message = lang == "EN" ? "RoomType not found." : "نوع الغرفة غير موجود" });
        roomType.NameAr = updated.NameAr;
        roomType.NameEn = updated.NameEn;
        await _unitOfWork.RoomType.UpdateAsync(roomType);
        await _unitOfWork.SaveChangesAsync();
        return Ok(roomType);
    }

    [HttpDelete("RemoveRoomType/{id}")]
    public async Task<IActionResult> RemoveRoomType(string lang, int id)
    {
        var roomType = await _unitOfWork.RoomType.GetByIdAsync(id);
        if (roomType == null) return NotFound(new { Message = lang == "EN" ? "RoomType not found." : "نوع الغرفة غير موجود" });
        await _unitOfWork.RoomType.DeleteAsync(roomType);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllRoomTypes")]
    public async Task<IActionResult> GetAllRoomTypes(string lang)
    {
        var roomTypes = await _unitOfWork.RoomType.GetAllAsync();
        if (roomTypes == null || !roomTypes.Any()) return NotFound(new { Message = lang == "EN" ? "No room types found." : "لم يتم ايجاد انواع غرف" });
        var roomTypeDtos = roomTypes.Select(r => new RoomTypeDto
        {
            Id = r.Id,
            NameAr = r.NameAr,
            NameEn = r.NameEn,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
            Rooms = r.Rooms?.Select(r => new RoomDto
            {
                Id = r.Id,
                Number = r.Number
            }).ToList()
        }).ToList();
        return Ok(roomTypeDtos);
    }

    [HttpGet("GenerateRoomTypes")]
    public async Task<IActionResult> GenerateRoomTypes(string lang)
    {
        var roomTypes = await _unitOfWork.RoomType.GetAllAsync();
        if (roomTypes == null || !roomTypes.Any()) return NotFound(new { Message = lang == "EN" ? "No room types found." : "لم يتم ايجاد انواع غرف" });
        var roomTypeDtos = roomTypes.Select(r => new SelectRoomTypeDTO
        {
            Id = r.Id,
            Name = lang == "EN" ? r.NameEn : r.NameAr,
        }).ToList();
        return Ok(roomTypeDtos);
    }

    #endregion

    #region Service

    [HttpPost("AddService")]
    public async Task<IActionResult> AddService(string lang, [FromBody] CreateOrUpdateServiceDto service)
    {
        if (service == null) return BadRequest(new { Message = lang == "EN" ? "Service cannot be null." : "خطأ في الإدخال" });
        var newService = new Service
        {
            NameAr = service.NameAr,
            NameEn = service.NameEn,
            ServiceLevel = service.ServiceLevel,
            SuperServiceId = service.SuperServiceId
        };
        await _unitOfWork.Service.AddAsync(newService);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newService);
    }

    [HttpGet("GetService/{id}")]
    public async Task<IActionResult> GetService(string lang, int id)
    {
        var service = await _unitOfWork.Service.FindAsync(s => s.Id == id, ["SuperService", "RequestDetails"]);
        if (service == null) return NotFound(new { Message = lang == "EN" ? "Service not found." : "الخدمة غير موجودة" });
        var serviceDto = new ServiceDto
        {
            Id = service.Id,
            NameAr = service.NameAr,
            NameEn = service.NameEn,
            SuperServiceId = service.SuperServiceId,
            SuperServiceNameAr = service.SuperService?.NameAr ?? "N/A",
            SuperServiceNameEn = service.SuperService?.NameEn ?? "N/A",
            CreatedAt = service.CreatedAt,
            UpdatedAt = service.UpdatedAt,
            //RequestDetails = service.RequestDetails?.Select(rd => new RequestDetailsDto
            //{
            //    Id = rd.Id,
            //    RequestHeaderId = rd.RequestHeaderId
            //}).ToList()
        };
        return Ok(serviceDto);
    }

    [HttpPut("EditService/{id}")]
    public async Task<IActionResult> EditService(string lang, int id, [FromBody] CreateOrUpdateServiceDto updated)
    {
        var service = await _unitOfWork.Service.GetByIdAsync(id);
        if (service == null) return NotFound(new { Message = lang == "EN" ? "Service not found." : "الخدمة غير موجودة" });
        service.NameAr = updated.NameAr;
        service.NameEn = updated.NameEn;
        service.SuperServiceId = updated.SuperServiceId;
        await _unitOfWork.Service.UpdateAsync(service);
        await _unitOfWork.SaveChangesAsync();
        return Ok(service);
    }

    [HttpDelete("RemoveService/{id}")]
    public async Task<IActionResult> RemoveService(string lang, int id)
    {
        var service = await _unitOfWork.Service.GetByIdAsync(id);
        if (service == null) return NotFound(new { Message = lang == "EN" ? "Service not found." : "الخدمة غير موجودة" });
        await _unitOfWork.Service.DeleteAsync(service);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllServices")]
    public async Task<IActionResult> GetAllServices(string lang)
    {
        var services = await _unitOfWork.Service.FindAllAsync(includes: ["SuperService"]);
        if (services == null || !services.Any()) return NotFound(new { Message = lang == "EN" ? "No services found." : "لم يتم ايجاد خدمات" });
        var serviceDtos = services.Select(s => new ServiceDto
        {
            Id = s.Id,
            NameAr = s.NameAr,
            NameEn = s.NameEn,
            SuperServiceId = s.SuperServiceId,
            SuperServiceNameAr = s.SuperService?.NameAr ?? "N/A",
            SuperServiceNameEn = s.SuperService?.NameEn ?? "N/A",
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();
        return Ok(services);
    }

    [HttpGet("GenerateLevelServices")]
    public async Task<IActionResult> GenerateLevelServices(string lang, [FromQuery] ServiceLevel serviceLevel)
    {
        var services = await _unitOfWork.Service.FindAllAsync(s => s.ServiceLevel == serviceLevel);
        if (services == null || !services.Any()) return NotFound(new { Message = lang == "EN" ? "No services found." : "لم يتم ايجاد خدمات" });
        var serviceDtos = services.Select(s => new SelectServiceDTO
        {
            Id = s.Id,
            Name = lang == "EN" ? s.NameEn : s.NameAr
        }).ToList();
        return Ok(serviceDtos);
    }

    [HttpGet("GenerateServices")]
    public async Task<IActionResult> GenerateServices(string lang)
    {
        // Get all level 2 services and include their parent and children
        var level2Services = await _unitOfWork.Service.FindAllAsync(
            s => s.ServiceLevel == ServiceLevel.SubService,
            includes: new[] { "SuperService", "SubServices" }
        );

        if (level2Services == null || !level2Services.Any())
            return NotFound(new { Message = lang == "EN" ? "No services found." : "لم يتم ايجاد خدمات" });

        // Group by level 1 parent
        var grouped = new Dictionary<int, SelectServiceDTO>();

        foreach (var level2 in level2Services)
        {
            var parent = level2.SuperService;
            var childs = level2.SubServices;
            if (parent == null) continue;
            //if (childs == null || !childs.Any()) continue;

            // Ensure the level 1 parent exists in the dictionary
            if (!grouped.ContainsKey(parent.Id))
            {
                grouped[parent.Id] = new SelectServiceDTO
                {
                    Id = parent.Id,
                    Name = lang == "EN" ? parent.NameEn : parent.NameAr,
                    SubServices = new List<SelectServiceDTO>()
                };
            }

            // Build level 2 service DTO
            var level2Dto = new SelectServiceDTO
            {
                Id = level2.Id,
                Name = lang == "EN" ? level2.NameEn : level2.NameAr,
                SubServices = childs
                    .Select(child => new SelectServiceDTO
                    {
                        Id = child.Id,
                        Name = lang == "EN" ? child.NameEn : child.NameAr
                    })
                    .ToList()
            };

            grouped[parent.Id].SubServices.Add(level2Dto);
        }

        return Ok(grouped.Values.ToList());
    }



    #endregion

    #region Status

    [HttpPost("AddStatus")]
    public async Task<IActionResult> AddStatus(string lang, [FromBody] CreateOrUpdateStatusDto status)
    {
        if (status == null) return BadRequest(new { Message = lang == "EN" ? "Status cannot be null." : "خطأ في الإدخال" });
        var newStatus = new Status
        {
            NameAr = status.NameAr,
            NameEn = status.NameEn
        };
        await _unitOfWork.Status.AddAsync(newStatus);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newStatus);
    }

    [HttpGet("GetStatus/{id}")]
    public async Task<IActionResult> GetStatus(string lang, int id)
    {
        var status = await _unitOfWork.Status.FindAsync(s => s.Id == id, ["RequestHeaders", "RequestDetails"]);
        if (status == null) return NotFound(new { Message = lang == "EN" ? "Status not found." : "الحالة غير موجودة" });
        var statusDto = new StatusDto
        {
            Id = status.Id,
            NameAr = status.NameAr,
            NameEn = status.NameEn,
            CreatedAt = status.CreatedAt,
            UpdatedAt = status.UpdatedAt,
            //RequestHeaders = status.RequestHeaders?.Select(rh => new RequestHeaderDto
            //{
            //    Id = rh.Id,
            //    RequestTypeId = rh.RequestTypeId
            //}).ToList()
        };
        return Ok(statusDto);
    }

    [HttpPut("EditStatus/{id}")]
    public async Task<IActionResult> EditStatus(string lang, int id, [FromBody] CreateOrUpdateStatusDto updated)
    {
        var status = await _unitOfWork.Status.GetByIdAsync(id);
        if (status == null) return NotFound(new { Message = lang == "EN" ? "Status not found." : "الحالة غير موجودة" });
        status.NameAr = updated.NameAr;
        status.NameEn = updated.NameEn;
        await _unitOfWork.Status.UpdateAsync(status);
        await _unitOfWork.SaveChangesAsync();
        return Ok(status);
    }

    [HttpDelete("RemoveStatus/{id}")]
    public async Task<IActionResult> RemoveStatus(string lang, int id)
    {
        var status = await _unitOfWork.Status.GetByIdAsync(id);
        if (status == null) return NotFound(new { Message = lang == "EN" ? "Status not found." : "الحالة غير موجودة" });
        await _unitOfWork.Status.DeleteAsync(status);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllStatuses")]
    public async Task<IActionResult> GetAllStatuses(string lang)
    {
        var statuses = await _unitOfWork.Status.FindAllAsync(includes: ["RequestHeaders"]);
        if (statuses == null || !statuses.Any()) return NotFound(new { Message = lang == "EN" ? "No statuses found." : "لم يتم ايجاد حالات" });
        var statusDtos = statuses.Select(s => new StatusDto
        {
            Id = s.Id,
            NameAr = s.NameAr,
            NameEn = s.NameEn,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();
        return Ok(statusDtos);
    }

    [HttpGet("GenerateStatuses")]
    public async Task<IActionResult> GenerateStatuses(string lang)
    {
        var statuses = await _unitOfWork.Status.GetAllAsync();
        if (statuses == null || !statuses.Any()) return NotFound(new { Message = lang == "EN" ? "No statuses found." : "لم يتم ايجاد حالات" });
        var statusDtos = statuses.Select(s => new SelectStatusDTO
        {
            Id = s.Id,
            Name = lang == "EN" ? s.NameEn : s.NameAr
        }).ToList();
        return Ok(statusDtos);
    }

    #endregion

    #region UseType

    [HttpPost("AddUseType")]
    public async Task<IActionResult> AddUseType(string lang, [FromBody] CreateOrUpdateUseTypeDto useType)
    {
        if (useType == null) return BadRequest(new { Message = lang == "EN" ? "UseType cannot be null." : "خطأ في الإدخال" });
        var newUseType = new UseType
        {
            NameAr = useType.NameAr,
            NameEn = useType.NameEn
        };
        await _unitOfWork.UseType.AddAsync(newUseType);
        await _unitOfWork.SaveChangesAsync();
        return Ok(newUseType);
    }

    [HttpGet("GetUseType/{id}")]
    public async Task<IActionResult> GetUseType(string lang, int id)
    {
        var useType = await _unitOfWork.UseType.FindAsync(u => u.Id == id, ["Rooms"]);
        if (useType == null) return NotFound(new { Message = lang == "EN" ? "UseType not found." : "نوع الاستخدام غير موجود" });
        var useTypeDto = new UseTypeDto
        {
            Id = useType.Id,
            NameAr = useType.NameAr,
            NameEn = useType.NameEn,
            CreatedAt = useType.CreatedAt,
            UpdatedAt = useType.UpdatedAt,
            Rooms = useType.Rooms?.Select(r => new RoomDto
            {
                Id = r.Id,
                Number = r.Number
            }).ToList()
        };
        return useType == null ? NotFound() : Ok(useType);
    }

    [HttpPut("EditUseType/{id}")]
    public async Task<IActionResult> EditUseType(string lang, int id, [FromBody] CreateOrUpdateUseTypeDto updated)
    {
        var useType = await _unitOfWork.UseType.GetByIdAsync(id);
        if (useType == null) return NotFound(new { Message = lang == "EN" ? "UseType not found." : "نوع الاستخدام غير موجود" });
        useType.NameAr = updated.NameAr;
        useType.NameEn = updated.NameEn;
        await _unitOfWork.UseType.UpdateAsync(useType);
        await _unitOfWork.SaveChangesAsync();
        return Ok(useType);
    }

    [HttpDelete("RemoveUseType/{id}")]
    public async Task<IActionResult> RemoveUseType(string lang, int id)
    {
        var useType = await _unitOfWork.UseType.GetByIdAsync(id);
        if (useType == null) return NotFound(new { Message = lang == "EN" ? "UseType not found." : "نوع الاستخدام غير موجود" });
        await _unitOfWork.UseType.DeleteAsync(useType);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("GetAllUseTypes")]
    public async Task<IActionResult> GetAllUseTypes(string lang)
    {
        var useTypes = await _unitOfWork.UseType.GetAllAsync();
        if (useTypes == null || !useTypes.Any()) return NotFound(new { Message = lang == "EN" ? "No use types found." : "لا يوجد انواع استخدام" });
        var useTypeDtos = useTypes.Select(u => new UseTypeDto
        {
            Id = u.Id,
            NameAr = u.NameAr,
            NameEn = u.NameEn,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt
        }).ToList();
        return Ok(useTypes);
    }

    [HttpGet("GenerateUseTypes")]
    public async Task<IActionResult> GenerateUseTypes(string lang)
    {
        var useTypes = await _unitOfWork.UseType.GetAllAsync();
        if (useTypes == null || !useTypes.Any()) return NotFound(new { Message = lang == "EN" ? "No use types found." : "لم يتم ايجاد انواع استخدام" });
        var useTypeDtos = useTypes.Select(u => new SelectUseTypeDTO
        {
            Id = u.Id,
            Name = lang == "EN" ? u.NameEn : u.NameAr
        }).ToList();
        return Ok(useTypeDtos);
    }

    #endregion
}
