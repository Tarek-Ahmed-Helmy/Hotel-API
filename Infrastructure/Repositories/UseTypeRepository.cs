using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class UseTypeRepository : GenericRepository<UseType>, IUseTypeRepository
{
    private readonly ApplicationDbContext _context;
    public UseTypeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
