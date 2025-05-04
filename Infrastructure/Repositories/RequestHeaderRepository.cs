using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class RequestHeaderRepository : GenericRepository<RequestHeader>, IRequestHeaderRepository
{
    private readonly ApplicationDbContext _context;
    public RequestHeaderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
