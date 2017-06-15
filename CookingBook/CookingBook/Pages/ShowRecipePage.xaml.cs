using CookingBook.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CookingBook.Pages
{
    /// <summary>
    /// Interaction logic for ShowRecipePage.xaml
    /// </summary>
    public partial class ShowRecipePage : Page
    {
        public ShowRecipePage()
        {
            try
            {
                InitializeComponent();
                DataContext = new ShowRecipePageViewModel(SharedVM.HomeVM.SelectedRecipe.ID);
            }
            catch(System.NullReferenceException)
            {
                return;
            }
        }
    }
}