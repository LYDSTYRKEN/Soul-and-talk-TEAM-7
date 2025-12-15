using System.IO;
using Soul_and_talk.Model;

namespace Soul_and_talk.Model.Repositories
{
    public class CustomerRepository
    {
        private List<Customer> _customers = new List<Customer>();
        private readonly InstitutionRepository _instRepo;

        public CustomerRepository(InstitutionRepository instRepo)
        {
            _instRepo = instRepo;
        }

        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);
        }

        public CustomerRepository()
        {
        }

        //--------------------- Save To File --------------------------

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

        //--------------------- Load From File --------------------------

        public void LoadFromFile(string path)
        {
            if (File.Exists("customers.txt"))
            {
                string[] lines = File.ReadAllLines("customers.txt");
                List<Institution> institutions = _instRepo.GetAllInstitutions();

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');   // 0=Id, 1=Name, 2=InstitutionId

                    Customer cust = new Customer();
                    cust.Id = int.Parse(parts[0]);
                    cust.Name = parts[1];
                    int instId = int.Parse(parts[2]);

                    if (instId != 0)
                    {
                        Institution foundInst = null;
                        foreach (Institution inst in institutions)
                        {
                            if (inst.Id == instId)
                            {
                                foundInst = inst;
                                break;
                            }
                        }

                        cust.Institution = foundInst;
                    }

                    AddCustomer(cust);
                }
            }
        }
    }
}

