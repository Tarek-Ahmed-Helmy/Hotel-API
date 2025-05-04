using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class StatusRepository : GenericRepository<Status>, IStatusRepository
{
    private readonly ApplicationDbContext _context;
    public StatusRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
