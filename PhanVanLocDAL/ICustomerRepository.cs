using PhanVanLocModels;

namespace PhanVanLocDAL
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer? GetByEmail(string email);
        IEnumerable<Customer> GetActiveCustomers();
        Task<Customer?> GetByEmailAsync(string email);
    }
}

