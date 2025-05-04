using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class RequestDetailsRepository : GenericRepository<RequestDetails>, IRequestDetailsRepository
{
    private readonly ApplicationDbContext _context;
    public RequestDetailsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
