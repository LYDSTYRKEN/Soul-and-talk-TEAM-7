using Soul_and_talk.ViewModel;
using System.Windows;

namespace Soul_and_talk
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
