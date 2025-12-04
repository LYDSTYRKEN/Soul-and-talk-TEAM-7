using System;
using System.Collections.Generic;
using System.Text;
using Soul_and_talk.Model;

namespace Soul_and_talk.Model
{
    public class CustomerRepository
    {
        private List<Customer> _customers = new List<Customer>();

        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);
        }
    }
}
