using CookingBook.DatabaseModel;
using CookingBook.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class HomePageViewModel : INotifyPropertyChanged
{
    public HomePageViewModel()
    {
        EditCmd = new RelayCommand(x => Edit());
        ShowCmd = new RelayCommand(x => Show());
        DeleteCmd = new RelayCommand(x => Delete());
        SearchCmd = new RelayCommand(x => Search());
        SoupCmd = new RelayCommand(x => ShowSoup());
        DessertCmd = new RelayCommand(x => ShowDessert());
        MainDishCmd = new RelayCommand(x => ShowMainDish());
        BreakfastCmd = new RelayCommand(x => ShowBreakfast());
        OtherCmd = new RelayCommand(x => ShowOther());
        AllRecipesCmd = new RelayCommand(x => ShowAll());
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes);
        }
    }

    private string _Page = null;

    private string searchTxt;

    private ObservableCollection<Recipe> recipeList;

    public event PropertyChangedEventHandler PropertyChanged = null;

    public ICommand AllRecipesCmd { get; set; }

    public ICommand OtherCmd { get; set; }

    public ICommand BreakfastCmd { get; set; }

    public ICommand DeleteCmd { get; set; }

    public ICommand DessertCmd { get; set; }

    public ICommand EditCmd { get; set; }

    public ICommand MainDishCmd { get; set; }

    public ICommand SearchCmd { get; set; }

    public ICommand ShowCmd { get; set; }

    public ICommand SoupCmd { get; set; }

    public string Page
    {
        get
        {
            return _Page;
        }
        set
        {
            _Page = value;
            OnPropertyChanged("Page");
        }
    }

    public string SearchTxt
    {
        get
        {
            return searchTxt;
        }
        set
        {
            searchTxt = value;
            OnPropertyChanged("SearchTxt");
        }
    }

    public ObservableCollection<Recipe> RecipeList
    {
        get
        {
            return recipeList;
        }
        set
        {
            recipeList = value;
            OnPropertyChanged("RecipeList");
        }
    }

    public Recipe SelectedRecipe { get; set; }

    //odswiezenie listy przepisow
    public void Refresh()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes);
        }
    }

    //wyswietlenie sniadan
    private void ShowBreakfast()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.RecipeCategory.Name == "Sniadanie"));
        }
    }

    //usuwanie przepisu z listy
    private void Delete()
    {
        if (SharedVM.HomeVM.SelectedRecipe == null)
        {
            MessageBox.Show("Nie wybrano żadnego przepisu.");
            return;
        }

        MessageBoxResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć przepis?", "Usuwanie przepisu", MessageBoxButton.YesNo);
        if (dialogResult == MessageBoxResult.Yes)
        {
            using (var contex = new CookingBookEntities1())
            {
                contex.Recipes.Remove(contex.Recipes.Where(x => x.ID == SelectedRecipe.ID).First());
                contex.Ingredients.RemoveRange(contex.Ingredients.Where(x => x.RecipeID == SelectedRecipe.ID));
                contex.SaveChanges();

                RecipeList.Remove(SelectedRecipe);
            }
        }
    }

    //przejscie do strony edycji przepisu
    private void Edit()
    {
        if(SelectedRecipe == null)
        {
            MessageBox.Show("Nie wybrano żadnego przepisu.");
            return;
        }
        try
        {
            Page = "AddRecipePage.xaml";
            SharedVM.RecipeVM.Fill(SelectedRecipe.ID);
        }
        catch
        {
            return;
        }

    }

    //filtrowanie listy przepisow wedlug wyszukiwanej nazwy
    private void Search()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.Title.ToUpper().Contains(SearchTxt.ToUpper())));
        }
    }

    //przejscie do strony wyswietlajacej przepis
    private void Show()
    {
        if (SharedVM.HomeVM.SelectedRecipe == null)
        {
            MessageBox.Show("Nie wybrano żadnego przepisu.");
            return;
        }
        Page = "ShowRecipePage.xaml";
    }

    //wyswietlenie wszystkiech pzrepisow
    private void ShowAll()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes);
        }
    }

    //wyswietlenei przepisow z kategorii "Inne"
    private void ShowOther()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.RecipeCategory.Name == "Inne"));
        }
    }

    //Wyswietlenie deserow
    private void ShowDessert()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.RecipeCategory.Name == "Deser"));
        }
    }

    //Wyswietlenie dan glownych
    private void ShowMainDish()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.RecipeCategory.Name == "Danie glowne"));
        }
    }

    //Wyswietlenie zup
    private void ShowSoup()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.RecipeCategory.Name == "Zupa"));
        }
    }

    //odświeżanie widoku
    virtual protected void OnPropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}