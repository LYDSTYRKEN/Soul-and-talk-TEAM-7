using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Soul_and_talk.ViewModel
{
    public class AddIncomeViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel;
        private Action close;
        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<Institution> Institutions { get; set; }

        private Customer selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }
        private bool registerNewCustomer;
        public bool RegisterNewCustomer
        {
            get { return RegisterNewCustomer; }
            set
            {
                RegisterNewCustomer = value;
                OnPropertyChanged("OpretNewCustomer");
            }
        }
        private string newCustomerName = "";
        public string NewCustomerName
        {
            get { return newCustomerName; }
            set
            {
                newCustomerName = value;
                OnPropertyChanged("NewCustomerName");
            }
        }
        private Institution selectedInstitution;
        public Institution SelectedInstitution
        {
            get { return selectedInstitution; }
            set
            {
                selectedInstitution = value;
                OnPropertyChanged("SelectedInstitution");
            }
        }
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        private decimal hours;
        public decimal Hours
        {
            get { return hours; }
            set
            {
                hours = value;
                OnPropertyChanged("Hours");
            }
        }
        private bool isPhysical;
        public bool IsPhysical
        {
            get { return isPhysical; }
            set
            {
                isPhysical = value;
                OnPropertyChanged("IsPhysical");
            }
        }
        private decimal kilometers;
        public decimal Kilometers
        {
            get { return kilometers; }
            set
            {
                kilometers = value;
                OnPropertyChanged("Kilometers");
            }
        }

        public ICommand saveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddIncomeViewModel(MainViewModel mainViewModel, Action close)
        {
            this.mainViewModel = mainViewModel;
            this.close = close;

            Customers = new ObservableCollection<Customer>(mainViewModel.GetCustomers());
            Institutions = new ObservableCollection<Institution>(mainViewModel.GetInstitutions);

            RegisterNewCustomer = false;
            Date = DateTime.Today;
            IsPhysical = true;

            saveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Close);
        }

        private void Save()
        {
            Customer customerToUse;
            if (RegisterNewCustomer)
            {
                if (string.IsNullOrWhiteSpace(NewCustomerName))
                {
                    MessageBox.Show("Skriv et Name til den nye Customer.");
                    return;
                }
                CustomerToUse = mainViewModel.AddNewCustomer(NewCustomerName, SelectedInstitution);
            }
            else
            {
                if (SelectedCustomer == null)
                {
                    MessageBox.Show("Vælg en Customer.");
                    return;
                }
                CustomerToUse = SelectedCustomer;
            }
            if (Hours <= 0)
            {
                MessageBox.Show("Angiv et gyldigt antal timer.");
                return;
            }
            if (!IsPhysical)
            {
                kilometers = 0;
            }
            mainViewModel.AddIncomeFromDialog(CustomerToUse, Date, Hours, IsPhysical, Kilometers);

            Close();
        }
    }
}
