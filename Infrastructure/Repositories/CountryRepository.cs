using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    internal sealed class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _context;
        public CountryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
