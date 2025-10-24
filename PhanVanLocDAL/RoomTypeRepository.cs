using PhanVanLocModels;

namespace PhanVanLocDAL
{
    public class RoomTypeRepository : Repository<RoomType>, IRoomTypeRepository
    {
        public RoomTypeRepository(HotelDbContext context) : base(context)
        {
        }
    }
}

