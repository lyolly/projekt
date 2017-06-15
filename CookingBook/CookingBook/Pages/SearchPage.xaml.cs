using CookingBook.ViewModels;
using System.Windows.Controls;

namespace CookingBook
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
            DataContext = SharedVM.SearchVM;
        }
    }
}