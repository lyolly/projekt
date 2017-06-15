using CookingBook.ViewModels;
using System.Windows.Controls;

namespace CookingBook
{
    /// <summary>
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page
    {
        public AddRecipePage()
        {
            InitializeComponent();
            DataContext = SharedVM.RecipeVM;
        }
    }
}