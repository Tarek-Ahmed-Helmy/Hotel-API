using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    private readonly ApplicationDbContext _context;
    public HotelRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
