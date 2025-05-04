using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext _context;
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
