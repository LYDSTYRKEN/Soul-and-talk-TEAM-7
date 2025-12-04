using Soul_and_talk.Model.BusinessLogic;
using Soul_and_talk.Model.Institution;
using Soul_and_talk.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Soul_and_talk.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private InstitutionRepository instRepo  = new InstitutionRepository();
        private CustomerRepository custRepo = new CustomerRepository();
        private IncomeRepository incRepo = new IncomeRepository();

        private HourlyRate hourlyRate = new HourlyRate();

        public ObservableCollection<OverviewNode> RootNodes { get; set; }

        public ICommand AddIncomeCommand { get; set; }

        public MainViewModel()
        {
            RootNodes = new ObservableCollection<OverviewNode>();

            AddIncomeCommand = new RelayCommand(AddIncome);

            BuildNoteTree();
        }
        private void ExistingData()
        {
            Institution publicInst1 = new Institution();
            publicInst1.Id = 1;
            publicInst1.Name = "Kommune 1";
            publicInst1.Type = InstitutionType.Public;

            Institution publicInst2 = new Institution();
            publicInst2.Id = 2;
            publicInst2.Name = "Kommune 2";
            publicInst2.Type = InstitutionType.Public;

            Institution privateInst3 = new Institution();
            privateInst3.Id = 3;
            privateInst3.Name = "Institution 3";
            privateInst3.Type = InstitutionType.Private;

            instRepo.AddInstitution(publicInst1);
            instRepo.AddInstitution(publicInst2);
            instRepo.AddInstitution(privateInst3);
        }
        private Income RegisterIncome(Customer Customer, DateTime Date, decimal Hours, bool IsPhysical, decimal Kilometers)
        {

            decimal HourlyRateCalculator = hourlyRate.GetRatePerHour(Customer, IsPhysical);

            Income income = new Income();
            income.Customer = Customer;
            income.Date = Date;
            income.Hours = Hours;
            income.IsPhysical = IsPhysical;
            income.Kilometers = Kilometers;
            income.Amount = HourlyRateCalculator;
            incRepo.AddIncome(income);

            return income;
        }

        public List<Institution> GetInstitutions()
        {
            return instRepo.GetAllInstitutions();
        }

        public List<Customer> GetCustomers()
        {
            return custRepo.GetAllCustomers();
        }

        public Customer AddNewCustomer(string Name, Institution institution)
        {
            Customer Customer = new Customer();
            Customer.Id = custRepo.GetAllCustomers().Count + 1;
            Customer.Name = Name;
            Customer.Institution = institution;

            custRepo.AddCustomer(customer);
            return Customer;
        }

        private void BuildNoteTree()
        {
            RootNodes.Clear();

            OverviewNode publicRoot = new OverviewNode("Public Institutions");
            OverviewNode privateRoot = new OverviewNode("Private Institutions");
            OverviewNode privateCustomer = new OverviewNode("Private Customers");

            RootNodes.Add(publicRoot);
            RootNodes.Add(privateRoot);
            RootNodes.Add(privateCustomer);

            List<Institution> institutions = instRepo.GetAllInstitutions();
            List<Customer> customers = custRepo.GetAllCustomers();

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
                    if (cust.Institution != null && Customer.Institution.id == inst.Id)
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
                    privateCustomer.Children.Add(custNode);

                    AddIncomesToCustomerNode(custNode, cust);
                }
            }
        }
        private void AddIncomesToCustomerNode(OverviewNode custNode, Customer cust)
        {
            List<Income> incomes = incRepo.GetAllIncomes();

            foreach (Income inc in incomes)
            {
                if (inc.customer != null && inc.Customer.Id == Customer.Id) 
                {
                    string text = inc.Date.ToShortDateString() + " - " +
                                  inc.Hours + " hours | " +
                                  inc.Amount + " kr.";
                    if (inc.IsPhysical && inc.kilometers > 0)
                    { 
                        text = text + " | " + inc.Kilometers + " km.";
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

        public void AddIncomeFromDialog(Customer Customer, DateTime Date Decimal Hours, bool IsPhysical, decimal kilometers)
        {
            Income inc = RegisterIncome(Customer, Date, Hours, IsPhysical, kilometers);
            IncomeRepository.AddIncome(inc);

            BuildNoteTree();

            // Gem income til fil eller database her
        }
    }
}
