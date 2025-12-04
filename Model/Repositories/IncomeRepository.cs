using System.Collections.Generic;
using System.IO;
using Soul_and_talk.Model;

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
    }
}
