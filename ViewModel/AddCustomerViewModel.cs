using Soul_and_talk.Commands;
using Soul_and_talk.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;



namespace Soul_and_talk.ViewModel
{
    public class AddCustomerViewModel : ViewModelBase
    {
        private MainViewModel _main;
        private Action _close;

        public ObservableCollection<Institution> Institutions { get; set; }

        private string _customerName = "";
        public string CustomerName
        {   
            get { return _customerName; }
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private Institution _selectedInstitution;
        public Institution SelectedInstitution
        {
            get { return _selectedInstitution; }
            set
            {
                _selectedInstitution = value;
                OnPropertyChanged(nameof(SelectedInstitution));
            }
        }

        public ICommand SaveCustomerCommand { get; set; } = new SaveCustomerCommand();
        public ICommand CancelCustomerCommand { get; set; } = new CancelCustomerCommand();


        public AddCustomerViewModel(MainViewModel main, Action close)
        {
            _main = main;
            _close = close;

            Institutions = new ObservableCollection<Institution>(_main.GetInstitutions());


        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                MessageBox.Show("Skriv Kunde Navn.");

                return;
            }

            _main.AddNewCustomer(CustomerName, SelectedInstitution);
            _main.SaveAllToFiles();

            Close();
        }

        public void Close()
        {
            _close();
        }
    }
}

