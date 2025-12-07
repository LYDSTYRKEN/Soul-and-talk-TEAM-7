using System.IO;
   // Marius her
namespace Soul_and_talk.Model.Repositories
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
     
        // id;name;institutionId (0 = no institution)
        public void SaveToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (Customer c in _customers)
                {
                    int instId = 0;
                    if (c.Institution != null)
                    {
                        instId = c.Institution.Id;
                    }

                    string line = c.Id + ";" + c.Name + ";" + instId;
                    writer.WriteLine(line);
                }
            }
        }
       
    }
}
 // Marius Her