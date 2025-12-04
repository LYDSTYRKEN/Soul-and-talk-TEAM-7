using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_and_talk.Model.Repositories
{
    public class CustomerRepository
    {
        public List<Customer> Customers { get; } = new List<Customer>();
        public List<Customer> GetAllCustomers()
        {
            return Customers;
        }
        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
    }
}
