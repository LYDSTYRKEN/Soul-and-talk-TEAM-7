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
            privateInst.Name = "Fulgereden";
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

            OverviewNode publicRoot = new OverviewNode("Offentlige Instituationer");
            OverviewNode privateRoot = new OverviewNode("Private Instituationer");
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




        /// <summary>
        /// Oh no
        /// </summary>
        public void SaveAllToFiles()
        {
            _instRepo.SaveToFile("institutions.txt");
            _custRepo.SaveToFile("customers.txt");
            _incomeRepo.SaveToFile("incomes.txt");
        }

        private void LoadAllFromFiles()
        {
            // 1) Institutions
            if (File.Exists("institutions.txt"))
            {
                string[] lines = File.ReadAllLines("institutions.txt");

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');   // 0=Id, 1=Name, 2=Type

                    Institution inst = new Institution();
                    inst.Id = int.Parse(parts[0]);
                    inst.Name = parts[1];
                    inst.Type = (InstitutionType)Enum.Parse(typeof(InstitutionType), parts[2]);

                    _instRepo.AddInstitution(inst);
                }
            }

            // 2) Customers
            if (File.Exists("customers.txt"))
            {
                string[] lines = File.ReadAllLines("customers.txt");
                List<Institution> institutions = _instRepo.GetAllInstitutions();

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');   // 0=Id, 1=Name, 2=InstitutionId

                    Customer cust = new Customer();
                    cust.Id = int.Parse(parts[0]);
                    cust.Name = parts[1];
                    int instId = int.Parse(parts[2]);

                    if (instId != 0)
                    {
                        Institution foundInst = null;
                        foreach (Institution inst in institutions)
                        {
                            if (inst.Id == instId)
                            {
                                foundInst = inst;
                                break;
                            }
                        }

                        cust.Institution = foundInst;
                    }

                    _custRepo.AddCustomer(cust);
                }
            }

            // 3) Incomes
            if (File.Exists("incomes.txt"))
            {
                string[] lines = File.ReadAllLines("incomes.txt");
                List<Customer> customers = _custRepo.GetAllCustomers();

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');   // 0=CustomerId, 1=Date, 2=Hours, 3=IsPhysical, 4=Km, 5=Amount

                    int customerId = int.Parse(parts[0]);

                    Customer foundCustomer = null;
                    foreach (Customer c in customers)
                    {
                        if (c.Id == customerId)
                        {
                            foundCustomer = c;
                            break;
                        }
                    }

                    if (foundCustomer == null)
                        continue;

                    Income inc = new Income();
                    inc.Customer = foundCustomer;
                    inc.Date = DateTime.Parse(parts[1]);
                    inc.Hours = decimal.Parse(parts[2]);
                    inc.IsPhysical = bool.Parse(parts[3]);
                    inc.Kilometers = decimal.Parse(parts[4]);
                    inc.Amount = decimal.Parse(parts[5]);

                    _incomeRepo.AddIncome(inc);
                }
            }
        }


    }
}
