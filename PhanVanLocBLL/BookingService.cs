using PhanVanLocDAL;
using PhanVanLocModels;

namespace PhanVanLocBLL
{
    public class BookingService
    {
        private readonly DatabaseService db = DatabaseService.Instance;

        public IEnumerable<BookingReservation> GetAll() => db.BookingReservations;

        public BookingReservation? GetById(int id) => db.BookingReservations.FirstOrDefault(b => b.BookingReservationID == id);

        public async Task<bool> AddAsync(BookingReservation booking)
        {
            try
            {
                // Generate new BookingReservationID
                var maxId = db.BookingReservations.Any() ? db.BookingReservations.Max(x => x.BookingReservationID) : 0;
                booking.BookingReservationID = maxId + 1;
                
                db.Context.BookingReservations.Add(booking);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(BookingReservation booking)
        {
            try
            {
                var existing = await db.Context.BookingReservations.FindAsync(booking.BookingReservationID);
                if (existing == null) return false;
                
                existing.BookingDate = booking.BookingDate;
                existing.TotalPrice = booking.TotalPrice;
                existing.CustomerID = booking.CustomerID;
                existing.BookingStatus = booking.BookingStatus;
                
                await db.SaveChangesAsync();
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
                var booking = await db.Context.BookingReservations.FindAsync(id);
                if (booking == null) return false;
                
                db.Context.BookingReservations.Remove(booking);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddBookingDetailAsync(BookingDetail detail)
        {
            try
            {
                db.Context.BookingDetails.Add(detail);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBookingDetailAsync(int bookingReservationId, int roomId)
        {
            try
            {
                var detail = await db.Context.BookingDetails.FindAsync(bookingReservationId, roomId);
                if (detail == null) return false;
                
                db.Context.BookingDetails.Remove(detail);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

