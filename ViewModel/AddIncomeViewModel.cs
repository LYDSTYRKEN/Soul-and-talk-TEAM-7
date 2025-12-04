using Soul_and_talk.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Soul_and_talk.ViewModel
{
    public class AddIncomeViewModel : ViewModelBase
    {
        private MainViewModel _main;
        private Action _close;

        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<Institution> Institutions { get; set; }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        private bool _registerNewCustomer;
        public bool RegisterNewCustomer
        {
            get { return _registerNewCustomer; }
            set
            {
                _registerNewCustomer = value;
                OnPropertyChanged("RegisterNewCustomer");
            }
        }

        private string _newCustomerName = "";
        public string NewCustomerName
        {
            get { return _newCustomerName; }
            set
            {
                _newCustomerName = value;
                OnPropertyChanged("NewCustomerName");
            }
        }

        private Institution _selectedInstitution;
        public Institution SelectedInstitution
        {
            get { return _selectedInstitution; }
            set
            {
                _selectedInstitution = value;
                OnPropertyChanged("SelectedInstitution");
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        private decimal _hours;
        public decimal Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                OnPropertyChanged("Hours");
            }
        }

        private bool _isPhysical;
        public bool IsPhysical
        {
            get { return _isPhysical; }
            set
            {
                _isPhysical = value;
                OnPropertyChanged("IsPhysical");
            }
        }

        private decimal _kilometers;
        public decimal Kilometers
        {
            get { return _kilometers; }
            set
            {
                _kilometers = value;
                OnPropertyChanged("Kilometers");
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddIncomeViewModel(MainViewModel main, Action close)
        {
            _main = main;
            _close = close;

            Customers = new ObservableCollection<Customer>(_main.GetCustomers());
            Institutions = new ObservableCollection<Institution>(_main.GetInstitutions());

            RegisterNewCustomer = false;
            Date = DateTime.Today;
            IsPhysical = true;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Close);
        }

        private void Save()
        {
            Customer customerToUse;

            if (RegisterNewCustomer)
            {
                if (string.IsNullOrWhiteSpace(NewCustomerName))
                {
                    MessageBox.Show("Please enter a name for the new customer.");
                    return;
                }

                customerToUse = _main.AddNewCustomer(NewCustomerName, SelectedInstitution);
            }
            else
            {
                if (SelectedCustomer == null)
                {
                    MessageBox.Show("Please select a customer.");
                    return;
                }

                customerToUse = SelectedCustomer;
            }

            if (Hours <= 0)
            {
                MessageBox.Show("Hours must be greater than 0.");
                return;
            }

            if (!IsPhysical)
            {
                Kilometers = 0;
            }

            _main.AddIncomeFromDialog(customerToUse, Date, Hours, IsPhysical, Kilometers);

            _main.SaveAllToFiles();

            Close();
        }

        private void Close()
        {
            _close();
        }
    }
}
