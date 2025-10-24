using System.Collections.Generic;
using System.Linq;
using PhanVanLocDAL;
using PhanVanLocModels;
using System.Threading.Tasks;

namespace PhanVanLocBLL
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomService()
        {
            _roomRepository = DatabaseService.Instance.UnitOfWork.Rooms;
            _roomTypeRepository = DatabaseService.Instance.UnitOfWork.RoomTypes;
        }

        public IEnumerable<RoomInformation> GetAll() => _roomRepository.GetActiveRooms();

        public RoomInformation? GetById(int id) => _roomRepository.GetById(id);

        public IEnumerable<RoomType> GetAllRoomTypes() => _roomTypeRepository.GetAll();

        public async Task<bool> AddAsync(RoomInformation room)
        {
            try
            {
                room.RoomStatus = 1;
                _roomRepository.Add(room);
                await _roomRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(RoomInformation room)
        {
            try
            {
                var existingRoom = _roomRepository.GetById(room.RoomID);
                if (existingRoom == null) return false;
                
                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.RoomDetailDescription = room.RoomDetailDescription;
                existingRoom.RoomMaxCapacity = room.RoomMaxCapacity;
                existingRoom.RoomPricePerDay = room.RoomPricePerDay;
                existingRoom.RoomTypeID = room.RoomTypeID;
                
                _roomRepository.Update(existingRoom);
                await _roomRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existingRoom = _roomRepository.GetById(id);
                if (existingRoom == null) return false;
                
                existingRoom.RoomStatus = 2;
                _roomRepository.Update(existingRoom);
                await _roomRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<RoomInformation> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAll();

            return _roomRepository.Find(r => 
                (r.RoomNumber != null && r.RoomNumber.Contains(searchTerm)) ||
                (r.RoomDetailDescription != null && r.RoomDetailDescription.Contains(searchTerm))
            );
        }

        public IEnumerable<RoomInformation> GetRoomsByType(int roomTypeId)
        {
            return _roomRepository.GetRoomsByType(roomTypeId);
        }
    }
}

