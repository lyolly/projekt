using CookingBook.DatabaseModel;
using CookingBook.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class SearchPageViewModel : INotifyPropertyChanged
{
    public SearchPageViewModel()
    {
        SearchCmd = new RelayCommand(x => Search());
        DeleteCmd = new RelayCommand(x => Delete());
        EditCmd = new RelayCommand(x => Edit());
        ShowCmd = new RelayCommand(x => Show());
        SelectedTitle = true;
        RecipeList = SharedVM.HomeVM.RecipeList;
    }

    private ObservableCollection<Recipe> recipeList;

    private string searchTxt;

    private bool selectedAuthor;

    private bool selectedIngredient;

    private bool selectedTitle;

    public event PropertyChangedEventHandler PropertyChanged = null;

    public ICommand DeleteCmd { get; set; }

    public ICommand EditCmd { get; set; }

    public ICommand SearchCmd { get; set; }

    public ICommand ShowCmd { get; set; }

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

    public bool SelectedAuthor
    {
        get
        {
            return selectedAuthor;
        }
        set
        {
            selectedAuthor = value;
            OnPropertyChanged("SelectedAuthor");
        }
    }

    public bool SelectedIngredient
    {
        get
        {
            return selectedIngredient;
        }
        set
        {
            selectedIngredient = value;
            OnPropertyChanged("SelectedIngredient");
        }
    }

    public bool SelectedTitle
    {
        get
        {
            return selectedTitle;
        }
        set
        {
            selectedTitle = value;
            OnPropertyChanged("SelectedTitle");
        }
    }

    public Recipe SelectedRecipe { get; set; }

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

    //odswiezenie listy przepisow
    public void Refresh()
    {
        using (var contex = new CookingBookEntities1())
        {
            RecipeList = new ObservableCollection<Recipe>(contex.Recipes);
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

    //przejscie do strony edycji
    private void Edit()
    {
        if (SharedVM.HomeVM.SelectedRecipe == null)
        {
            MessageBox.Show("Nie wybrano żadnego przepisu.");
            return;
        }

        SharedVM.MainVM.Add();
        SharedVM.RecipeVM.Fill(SelectedRecipe.ID);
    }

    //wyswietlanie listy przepisow spelniajacych kryteria wyszukiwania
    private void Search()
    {
        using (var contex = new CookingBookEntities1())
        {
            if (SelectedTitle)
                RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.Title.ToUpper().Contains(SearchTxt.ToUpper())));
            else if (SelectedAuthor)
                RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.Author.Name.ToUpper().Contains(SearchTxt.ToUpper())));
            else if (SelectedIngredient)
                RecipeList = new ObservableCollection<Recipe>(contex.Recipes.Where(x => x.Ingredients.Any(v => v.Name.ToUpper().Contains(SearchTxt.ToUpper()))));
        }
    }

    //przejscie do strony wyswietlajacej przepis
    private void Show()
    {
        if (SelectedRecipe == null)
        {
            MessageBox.Show("Nie wybrano żadnego przepisu.");
            return;
        }

        SharedVM.MainVM.Home();
        SharedVM.HomeVM.SelectedRecipe = this.SelectedRecipe;
        SharedVM.HomeVM.Page = "ShowRecipePage.xaml";
    }

    //odświeżanie widoku
    virtual protected void OnPropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}