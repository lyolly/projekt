using CookingBook.DatabaseModel;
using CookingBook.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class AddRecipePageViewModel : INotifyPropertyChanged
{
    public AddRecipePageViewModel()
    {
        IngredientList = new ObservableCollection<Ingredient>();
        AddIngredientCmd = new RelayCommand(x => AddIngredient());
        DeleteIngredientCmd = new RelayCommand(DeleteIngredient);
        AddRecipeCmd = new RelayCommand(x => AddRecipe());
        EditRecipeCmd = new RelayCommand(x => EditRecipe());
        using (var contex = new CookingBookEntities1())
        {
            CategoriesCB = new ObservableCollection<RecipeCategory>(contex.RecipeCategories);
            SelectedCategoryCB = contex.RecipeCategories.First();
            SelectedCategoryIndex = 0;
            QuantityCB = new ObservableCollection<string>(contex.QuantityTypes.Select(x => x.Name));
            SelectedQuantityCB = contex.QuantityTypes.Select(x => x.Name).First();
        }
        OnPropertyChanged("IngredientNameTxt");
    }

    private string _AuthorTxt;

    private string _Description;

    private string _IngredientName;

    private string _IngredientQuantity;

    private string _Title;

    private bool isAdd;

    private bool isEdit;

    private RecipeCategory selectedCategoryCB;

    private string selectedQuantityCB;

    private int selectedCategoryIndex;

    public event PropertyChangedEventHandler PropertyChanged = null;

    public ICommand AddIngredientCmd { get; set; }

    public ICommand AddRecipeCmd { get; set; }

    public ICommand DeleteIngredientCmd { get; set; }

    public ICommand EditRecipeCmd { get; set; }

    public ObservableCollection<RecipeCategory> CategoriesCB { get; set; }

    public ObservableCollection<Ingredient> IngredientList { get; set; }

    public ObservableCollection<string> QuantityCB { get; set; }

    public string AuthorTxt
    {
        get
        {
            return _AuthorTxt;
        }
        set
        {
            _AuthorTxt = value;
            OnPropertyChanged("AuthorTxt");
        }
    }

    public string DescriptionTxt
    {
        get
        {
            return _Description;
        }
        set
        {
            _Description = value;
            OnPropertyChanged("DescriptionTxt");
        }
    }

    public string IngredientNameTxt
    {
        get
        {
            return _IngredientName;
        }
        set
        {
            _IngredientName = value;
            OnPropertyChanged("IngredientNameTxt");
        }
    }

    public string IngredientQuantityTxt
    {
        get
        {
            return _IngredientQuantity;
        }
        set
        {
            _IngredientQuantity = value;
            OnPropertyChanged("IngredientQuantityTxt");
        }
    }

    public string TitleTxt
    {
        get
        {
            return _Title;
        }
        set
        {
            _Title = value;
            OnPropertyChanged("TitleTxt");
        }
    }

    public string SelectedQuantityCB
    {
        get
        {
            return selectedQuantityCB;
        }
        set
        {
            selectedQuantityCB = value;
            OnPropertyChanged("SelectedQuantityCB");
        }
    }

    public bool IsAdd
    {
        get
        {
            return isAdd;
        }
        set
        {
            isAdd = value;
            OnPropertyChanged("IsAdd");
        }
    }

    public bool IsEdit
    {
        get
        {
            return isEdit;
        }
        set
        {
            isEdit = value;
            OnPropertyChanged("IsEdit");
        }
    }

    public RecipeCategory SelectedCategoryCB
    {
        get
        {
            return selectedCategoryCB;
        }
        set
        {
            selectedCategoryCB = value;
            OnPropertyChanged("SelectedCategoryCB");
        }
    }

    public int SelectedCategoryIndex
    {
        get
        {
            return selectedCategoryIndex;
        }
        set
        {
            selectedCategoryIndex = value;
            OnPropertyChanged("SelectedCategoryIndex");
        }
    }

    private long RecipeID { get; set; }

    //Wypelnianie pol strony danymi do edycji
    public void Fill(long id)
    {
        using (var contex = new CookingBookEntities1())
        {
            var recipe = contex.Recipes.Where(x => x.ID == id).FirstOrDefault();
            TitleTxt = recipe.Title;
            SelectedCategoryIndex = contex.RecipeCategories.ToList().IndexOf(recipe.RecipeCategory);
            SelectedCategoryCB = recipe.RecipeCategory;
            IngredientList = new ObservableCollection<Ingredient>(recipe.Ingredients);
            DescriptionTxt = recipe.Description;
            AuthorTxt = recipe.Author.Name;
            IsEdit = true;
            IsAdd = false;
            RecipeID = recipe.ID;
        }
    }

    //Dodawanie nowego skladnika do listy
    private void AddIngredient()
    {
        var ingredient = new Ingredient();
        ingredient.Name = IngredientNameTxt;
        if (IngredientNameTxt == null || IngredientNameTxt == "")
        {
            MessageBox.Show("Uzupełnij nazwę składnika");
            return;
        }
        try
        {
            ingredient.Quantity = double.Parse(IngredientQuantityTxt);
        }
        catch (Exception)
        {
            return;
        }

        using (var contex = new CookingBookEntities1())
        {
            ingredient.TypeID = contex.QuantityTypes.Where(x => x.Name == SelectedQuantityCB).First().ID;
        }

        IngredientList.Add(ingredient);
        IngredientNameTxt = null;
        IngredientQuantityTxt = null;
    }

    //Dodawanie nowego przepisu do bazy
    private void AddRecipe()
    {
        if (DescriptionTxt == null || TitleTxt == null || DescriptionTxt == "" || TitleTxt == "") //jeżeli pola opis lub nazwa przepisu są puste
        {
            MessageBox.Show("Uzupełnij puste pola.");
            return;
        }

        if (AuthorTxt == null || AuthorTxt == "") //jeżeli pole autor jest puste
        {
            MessageBoxResult dialogResult = MessageBox.Show("Czy chcesz dodać przepis bez autora?", "Dodawanie przepisu", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
                AuthorTxt = "Anonimowy";
            else
                return;
        }

        if (IngredientList.Count == 0) //jeżeli lista składników = 0
        {
            MessageBoxResult dialogResult = MessageBox.Show("Czy na pewno chcesz dodać przepis nie zawierający składników?", "Dodawanie przepisu", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.No)
                return;
        }

        using (var contex = new CookingBookEntities1()) //dodawanie przepisu
        {
            Recipe newRecipe = new Recipe();
            newRecipe.Description = DescriptionTxt;
            newRecipe.Title = TitleTxt;
            newRecipe.Ingredients = IngredientList;
            newRecipe.RecipeCategoryID = contex.RecipeCategories.Where(n => n.Name == selectedCategoryCB.Name).First().ID;
            long AuthorID = GetAuthorID();
            if (AuthorID == -1) //jeżeli autor nie istnieje
            {
                Author author = new Author();
                author.Name = AuthorTxt;
                contex.Authors.Add(author);
                newRecipe.Author = author;
            }
            else
                newRecipe.AuthorID = AuthorID;
            contex.Recipes.Add(newRecipe);
            contex.SaveChanges();
            MessageBox.Show("Dodano nowy przepis.");
            ClearPage();
        }
    }

    //Czyszczenie pól
    public void ClearPage()
    {
        DescriptionTxt = "";
        TitleTxt = "";
        AuthorTxt = "";
        IngredientList.Clear();
        SelectedCategoryCB.ID = 0;
        SelectedQuantityCB = "Szklanka";
    }

    //Usuwanie składnika z listy
    private void DeleteIngredient(object ingredient)
    {
        IngredientList.Remove(ingredient as Ingredient);
    }

    //Dodanie do bazy zedytowanego przepisu
    private void EditRecipe()
    {
        if (DescriptionTxt == "" || TitleTxt == "") //jeżeli pola opis lub nazwa są puste
        {
            MessageBox.Show("Uzupełnij puste pola.");
            return;
        }

        if (AuthorTxt == "") //jeżeli pole autor jest puste
        {
            MessageBoxResult dialogResult = MessageBox.Show("Czy chcesz dodać przepis bez autora?", "Dodawanie przepisu", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
                AuthorTxt = "Anonimowy";
            else
                return;
        }

        if (IngredientList.Count == 0) //jeżeli lista składników jest pusta
        {
            MessageBoxResult dialogResult = MessageBox.Show("Czy na pewno chcesz dodać przepis nie zawierający składników?", "Dodawanie przepisu", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.No)
                return;
        }

        using (var contex = new CookingBookEntities1()) //edycja przepisu
        {
            Recipe original = contex.Recipes.Find(RecipeID);

            if (original != null)
            {
                original.Description = DescriptionTxt;
                original.Title = TitleTxt;
                contex.Ingredients.RemoveRange(original.Ingredients); //usunięcie wszystkich składników
                foreach (var item in IngredientList) //zapisanie nowej listy składników
                {
                    item.Recipe = original;
                    contex.Ingredients.Add(item);
                }
                original.RecipeCategoryID = SelectedCategoryCB.ID;
                long AuthorID = GetAuthorID();
                if (AuthorID == -1) //jeżeli autor nie istnieje
                {
                    Author author = new Author();
                    author.Name = AuthorTxt;
                    contex.Authors.Add(author);
                    original.Author = author;
                }
                else
                    original.AuthorID = AuthorID;
                contex.SaveChanges();
                MessageBox.Show("Przepis został zedytowany");
                ClearPage();
                SharedVM.MainVM.Home();
            }
        }
    }

    //Pobieranie ID autora według nazwy, jeśli nie istnieje zwraca -1
    private long GetAuthorID()
    {
        using (var contex = new CookingBookEntities1())
        {
            try
            {
                return contex.Authors.Where(x => x.Name == this.AuthorTxt).First().ID;
            }
            catch
            {
                return -1;
            }
        }
    }

    //odświeżenie widoku
    virtual protected void OnPropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}
