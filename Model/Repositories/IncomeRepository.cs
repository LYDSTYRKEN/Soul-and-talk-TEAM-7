using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_and_talk.Model.Repositories
{
    public class IncomeRepository
    {
        public List<Income> Incomes { get; } = new List<Income>();

        public List<Income> GetAllIncomes()
        {
            return Incomes;
        }
        public void AddIncome(Income income)
        {
            Incomes.Add(income);
        }
    }
}
