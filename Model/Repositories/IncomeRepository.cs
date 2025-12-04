using System.Collections.Generic;
using Soul_and_talk.Model;


namespace Soul_and_talk.Model
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
    }
}
