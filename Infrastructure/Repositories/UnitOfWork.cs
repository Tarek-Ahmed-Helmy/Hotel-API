using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public ICompanyRepository Company { get; }
    public ICountryRepository Country { get; }
    public IDepartmentRepository Department { get; }
    public IHotelRepository Hotel { get; }
    public IRequestDetailsRepository RequestDetails { get; }
    public IRequestHeaderRepository RequestHeader { get; }
    public IRoomRepository Room { get; }
    public IRoomTypeRepository RoomType { get; }
    public IServiceRepository Service { get; }
    public IStatusRepository Status { get; }
    public IUseTypeRepository UseType { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Department = new DepartmentRepository(context);
        Country = new CountryRepository(context);
        Company = new CompanyRepository(context);
        Hotel = new HotelRepository(context);
        RequestDetails = new RequestDetailsRepository(context);
        RequestHeader = new RequestHeaderRepository(context);
        Room = new RoomRepository(context);
        RoomType = new RoomTypeRepository(context);
        Service = new ServiceRepository(context);
        Status = new StatusRepository(context);
        UseType = new UseTypeRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
