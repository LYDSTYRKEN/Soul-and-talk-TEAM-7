using Soul_and_talk.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Soul_and_talk.Commands
{
    public class CancelIncomeCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public void Execute(object? parameter)
        {
            if (parameter is AddIncomeViewModel addIncomeViewModel)
            {
                addIncomeViewModel.CancelIncome();
            }
        }
    
    }
}
