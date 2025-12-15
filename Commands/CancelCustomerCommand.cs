using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Soul_and_talk.Commands
{
    public class CancelCustomerCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public void Execute(object? parameter)
        {
            if (parameter is ViewModel.AddCustomerViewModel addCustomerViewModel)
            {
                addCustomerViewModel.Close();
            }
        }
    }
}
