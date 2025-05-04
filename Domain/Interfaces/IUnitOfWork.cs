namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository Company { get; }
    ICountryRepository Country { get; }
    IDepartmentRepository Department { get; }
    IHotelRepository Hotel { get; }
    IRequestDetailsRepository RequestDetails { get; }
    IRequestHeaderRepository RequestHeader { get; }
    IRoomRepository Room { get; }
    IRoomTypeRepository RoomType { get; }
    IServiceRepository Service { get; }
    IStatusRepository Status { get; }
    IUseTypeRepository UseType { get; }
    Task<int> SaveChangesAsync();
}
