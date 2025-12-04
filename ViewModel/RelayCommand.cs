using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_and_talk.ViewModel
{
    public class RelayCommand : ICommand
    {
        private Action execute;

        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (execute != null)
            {
                execute();
            }
        }
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}
