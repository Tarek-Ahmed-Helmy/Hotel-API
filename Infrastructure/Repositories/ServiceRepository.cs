using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class ServiceRepository : GenericRepository<Service>, IServiceRepository
{
    private readonly ApplicationDbContext _context;
    public ServiceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
