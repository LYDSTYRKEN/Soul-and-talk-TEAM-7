using Soul_and_talk.Model;
using Soul_and_talk.Model.BusinessLogic;
using Soul_and_talk.Model.Repositories;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Soul_and_talk;

namespace Soul_and_talk.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private InstitutionRepository _instRepo = new InstitutionRepository();
        private CustomerRepository _custRepo = new CustomerRepository();
        private IncomeRepository _incomeRepo = new IncomeRepository();

        private HourlyRate _hourlyRate = new HourlyRate();

        public ObservableCollection<OverviewNode> RootNodes { get; set; }

        public ICommand AddIncomeCommand { get; set; }
        public ICommand AddCustomerCommand { get; set; }

        public MainViewModel()
        {
            RootNodes = new ObservableCollection<OverviewNode>();

            AddIncomeCommand = new RelayCommand(AddIncome);
            AddCustomerCommand = new RelayCommand(AddCustomer);

            LoadAllFromFiles();

            if (_instRepo.GetAllInstitutions().Count == 0)
            {
                FillInitialData();
            }

            BuildNodeTree();
        }

        private void FillInitialData()
        {
            Institution publicInst1 = new Institution();
            publicInst1.Id = 1;
            publicInst1.Name = "Kolding Kommune";
            publicInst1.Type = InstitutionType.Public;

            Institution publicInst2 = new Institution();
            publicInst2.Id = 2;
            publicInst2.Name = "Haderslev Kommune";
            publicInst2.Type = InstitutionType.Public;

            Institution publicInst3 = new Institution();
            publicInst3.Id = 3;
            publicInst3.Name = "Horsens Kommune";
            publicInst3.Type = InstitutionType.Public;

            Institution privateInst = new Institution();
            privateInst.Id = 4;
            privateInst.Name = "Fuglereden";
            privateInst.Type = InstitutionType.Private;

            _instRepo.AddInstitution(publicInst1);
            _instRepo.AddInstitution(publicInst2);
            _instRepo.AddInstitution(publicInst3);
            _instRepo.AddInstitution(privateInst);
        }

        private Income RegisterIncome(Customer customer, DateTime date, decimal hours, bool isPhysical, decimal kilometers)
        {
            decimal rate = _hourlyRate.GetRatePerHour(customer, isPhysical);

            Income income = new Income();
            income.Customer = customer;
            income.Date = date;
            income.Hours = hours;
            income.IsPhysical = isPhysical;
            income.Kilometers = kilometers;
            income.Amount = rate * hours;

            _incomeRepo.AddIncome(income);

            return income;
        }

        public List<Institution> GetInstitutions()
        {
            return _instRepo.GetAllInstitutions();
        }

        public List<Customer> GetCustomers()
        {
            return _custRepo.GetAllCustomers();
        }

        public Customer AddNewCustomer(string name, Institution institution)
        {
            Customer customer = new Customer();
            customer.Id = _custRepo.GetAllCustomers().Count + 1;
            customer.Name = name;
            customer.Institution = institution;

            _custRepo.AddCustomer(customer);
            return customer;
        }

        private void BuildNodeTree()
        {
            RootNodes.Clear();

            OverviewNode publicRoot = new OverviewNode("Offentlige Institutioner");
            OverviewNode privateRoot = new OverviewNode("Private Institutioner");
            OverviewNode privateCustomersRoot = new OverviewNode("Private Kunder");

            RootNodes.Add(publicRoot);
            RootNodes.Add(privateRoot);
            RootNodes.Add(privateCustomersRoot);

            List<Institution> institutions = _instRepo.GetAllInstitutions();
            List<Customer> customers = _custRepo.GetAllCustomers();

            foreach (Institution inst in institutions)
            {
                OverviewNode instNode = new OverviewNode(inst.Name, inst);

                if (inst.Type == InstitutionType.Public)
                {
                    publicRoot.Children.Add(instNode);
                }
                else
                {
                    privateRoot.Children.Add(instNode);
                }

                foreach (Customer cust in customers)
                {
                    if (cust.Institution != null && cust.Institution.Id == inst.Id)
                    {
                        OverviewNode custNode = new OverviewNode(cust.Name, cust);
                        instNode.Children.Add(custNode);

                        AddIncomesToCustomerNode(custNode, cust);
                    }
                }
            }

            foreach (Customer cust in customers)
            {
                if (cust.Institution == null)
                {
                    OverviewNode custNode = new OverviewNode(cust.Name, cust);
                    privateCustomersRoot.Children.Add(custNode);

                    AddIncomesToCustomerNode(custNode, cust);
                }
            }
        }

        private void AddIncomesToCustomerNode(OverviewNode custNode, Customer cust)
        {
            List<Income> incomes = _incomeRepo.GetAllIncomes();

            foreach (Income inc in incomes)
            {
                if (inc.Customer != null && inc.Customer.Id == cust.Id)
                {
                    string text = inc.Date.ToShortDateString() + " | " +
                                  inc.Hours + " Timer | " +
                                  inc.Amount + " kr.";

                    if (inc.IsPhysical && inc.Kilometers > 0)
                    {
                        text = text + " | " + inc.Kilometers + " km";
                    }

                    OverviewNode incNode = new OverviewNode(text, inc);
                    custNode.Children.Add(incNode);
                }
            }
        }

        private void AddIncome()
        {
            AddIncomeWindow window = new AddIncomeWindow();

            AddIncomeViewModel vm = new AddIncomeViewModel(this, () => window.Close());
            window.DataContext = vm;

            window.ShowDialog();
        }

        public void AddIncomeFromDialog(Customer customer, DateTime date, decimal hours, bool isPhysical, decimal kilometers)
        {
            RegisterIncome(customer, date, hours, isPhysical, kilometers);
            BuildNodeTree();
        }

        // Called when "Add Customer" button is clicked

        private void AddCustomer()
        {
            AddCustomerWindow window = new AddCustomerWindow();

            AddCustomerViewModel vm = new AddCustomerViewModel(this, () => window.Close());
            window.DataContext = vm;

            window.ShowDialog();

            // Byg træet igen, så den nye kunde vises
            BuildNodeTree();
        }
       
        public void SaveAllToFiles()
        {
            _instRepo.SaveToFile("institutions.txt");
            _custRepo.SaveToFile("customers.txt");
            _incomeRepo.SaveToFile("incomes.txt");
        }

        private void LoadAllFromFiles()
        {
            _instRepo.LoadFromFile("institutions.txt");
            _custRepo.LoadFromFile("customers.txt");
            _incomeRepo.LoadFromFile("incomes.txt");
        }
    }
}
