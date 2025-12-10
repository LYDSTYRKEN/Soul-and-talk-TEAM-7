using System.IO;

namespace Soul_and_talk.Model.Repositories
{
    public class IncomeRepository
    {
        private List<Income> _incomes = new List<Income>();

        public List<Income> GetAllIncomes()
        {
            return _incomes;
        }

        public void AddIncome(Income income)
        {
            _incomes.Add(income);
        }

        // customerId;date;hours;isPhysical;kilometers;amount
        public void SaveToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (Income inc in _incomes)
                {
                    int customerId = 0;
                    if (inc.Customer != null)
                    {
                        customerId = inc.Customer.Id;
                    }

                    string line =
                        customerId + ";" +
                        inc.Date.ToString("yyyy-MM-dd") + ";" +
                        inc.Hours + ";" +
                        inc.IsPhysical + ";" +
                        inc.Kilometers + ";" +
                        inc.Amount;

                    writer.WriteLine(line);
                }
            }
        }
        public void LoadFromFile()
                        if (File.Exists("incomes.txt"))
            {
                string[] lines = File.ReadAllLines("incomes.txt");
                List<Customer> customers = _custRepo.GetAllCustomers();

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');   // 0=CustomerId, 1=Date, 2=Hours, 3=IsPhysical, 4=Km, 5=Amount

                    int customerId = int.Parse(parts[0]);

                    Customer foundCustomer = null;
                    foreach (Customer c in customers)
                    {
                        if (c.Id == customerId)
                        {
                            foundCustomer = c;
                            break;
                        }
                    }

                    if (foundCustomer == null)
                        continue;

                    Income inc = new Income();
                    inc.Customer = foundCustomer;
                    inc.Date = DateTime.Parse(parts[1]);
                    inc.Hours = decimal.Parse(parts[2]);
                    inc.IsPhysical = bool.Parse(parts[3]);
                    inc.Kilometers = decimal.Parse(parts[4]);
                    inc.Amount = decimal.Parse(parts[5]);

                    _incomeRepo.AddIncome(inc);
                }
            }
    }
}
