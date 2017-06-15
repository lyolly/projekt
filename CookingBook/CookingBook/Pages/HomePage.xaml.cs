using CookingBook.ViewModels;
using System.Windows.Controls;

namespace CookingBook
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = SharedVM.HomeVM;
        }
    }
}