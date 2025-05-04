using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
{
    private readonly ApplicationDbContext _context;
    public RoomTypeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
