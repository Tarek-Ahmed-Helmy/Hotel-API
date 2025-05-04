using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    private readonly ApplicationDbContext _context;
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
