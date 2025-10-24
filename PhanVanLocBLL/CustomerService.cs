using System.Collections.Generic;
using System.Linq;
using PhanVanLocDAL;
using PhanVanLocModels;
using System.Threading.Tasks;

namespace PhanVanLocBLL
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = DatabaseService.Instance.UnitOfWork.Customers;
        }

        public IEnumerable<Customer> GetAll() => _customerRepository.GetActiveCustomers();

        public Customer? GetById(int id) => _customerRepository.GetById(id);

        public Customer? GetByEmail(string email) => _customerRepository.GetByEmail(email);

        public async Task<bool> AddAsync(Customer customer)
        {
            try
            {
                customer.CustomerStatus = 1;
                _customerRepository.Add(customer);
                await _customerRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            try
            {
                var existingCustomer = _customerRepository.GetById(customer.CustomerID);
                if (existingCustomer == null) return false;

                existingCustomer.CustomerFullName = customer.CustomerFullName;
                existingCustomer.EmailAddress = customer.EmailAddress;
                existingCustomer.Telephone = customer.Telephone;
                existingCustomer.CustomerBirthday = customer.CustomerBirthday;
                existingCustomer.Password = customer.Password;

                _customerRepository.Update(existingCustomer);
                await _customerRepository.SaveChangesAsync();
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
                var existingCustomer = _customerRepository.GetById(id);
                if (existingCustomer == null) return false;

                existingCustomer.CustomerStatus = 2;
                _customerRepository.Update(existingCustomer);
                await _customerRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Customer> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAll();

            return _customerRepository.Find(c => 
                (c.CustomerFullName != null && c.CustomerFullName.Contains(searchTerm)) ||
                (c.EmailAddress != null && c.EmailAddress.Contains(searchTerm)) ||
                (c.Telephone != null && c.Telephone.Contains(searchTerm))
            );
        }
    }
}

