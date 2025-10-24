using Microsoft.EntityFrameworkCore;
using PhanVanLocModels;

namespace PhanVanLocDAL
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(HotelDbContext context) : base(context)
        {
        }

        public Customer? GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(c => c.EmailAddress == email && c.CustomerStatus == 1);
        }

        public IEnumerable<Customer> GetActiveCustomers()
        {
            return _dbSet.Where(c => c.CustomerStatus == 1).ToList();
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.EmailAddress == email && c.CustomerStatus == 1);
        }
    }
}

