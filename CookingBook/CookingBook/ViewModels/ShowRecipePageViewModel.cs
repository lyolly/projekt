using CookingBook.DatabaseModel;
using CookingBook.ViewModels;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

public class ShowRecipePageViewModel : INotifyPropertyChanged
{
    public ShowRecipePageViewModel(long recipeID)
    {
        using (var contex = new CookingBookEntities1())
        {
            id = recipeID;
            SelectedRecipe = contex.Recipes.Where(x => x.ID == recipeID).FirstOrDefault();
            RecipeTitle = SelectedRecipe.Title;
            RecipeDescription = SelectedRecipe.Description;
            EditRecipeCmd = new RelayCommand(x => Edit());
            DeleteRecipeCmd = new RelayCommand(x => Delete());
            foreach (var item in SelectedRecipe.Ingredients)
            {
                RecipeIngredients += "•  " + item.ToString() + "\n";
            }
            setQRData();
        }
    }

    private BitmapImage qrCode;

    private string recipeDescription;

    private string recipeIngredients;

    private string recipeTitle;

    private long id { get; set; }

    public event PropertyChangedEventHandler PropertyChanged = null;

    public ICommand DeleteRecipeCmd { get; set; }

    public ICommand EditRecipeCmd { get; set; }

    public BitmapImage QrCode
    {
        get
        {
            return qrCode;
        }
        set
        {
            qrCode = value;
            OnPropertyChanged("QrCode");
        }
    }

    public string RecipeDescription
    {
        get
        {
            return recipeDescription;
        }
        set
        {
            recipeDescription = value;
            OnPropertyChanged("RecipeDescription");
        }
    }

    public string RecipeIngredients
    {
        get
        {
            return recipeIngredients;
        }
        set
        {
            recipeIngredients = value;
            OnPropertyChanged("RecipeIngredients");
        }
    }

    public string RecipeTitle
    {
        get
        {
            return recipeTitle;
        }
        set
        {
            recipeTitle = value;
            OnPropertyChanged("RecipeList");
        }
    }

    public Recipe SelectedRecipe { get; set; }

    //usuwanie wyswietlanego przepisu
    private void Delete()
    {
        MessageBoxResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć przepis?", "Usuwanie przepisu", MessageBoxButton.YesNo);
        if (dialogResult == MessageBoxResult.Yes)
        {
            using (var contex = new CookingBookEntities1())
            {
                contex.Recipes.Remove(contex.Recipes.Where(x => x.ID == id).First());
                contex.Ingredients.RemoveRange(contex.Ingredients.Where(x => x.RecipeID == id));
                contex.SaveChanges();
                SharedVM.MainVM.Home();
                MessageBox.Show("Przepis został usunięty.");
            }
        }
    }

    //przejscie do strony edycji przepisu
    private void Edit()
    {
        SharedVM.MainVM.Add();
        SharedVM.RecipeVM.Fill(id);
    }

    //utworzenie qrcode dla danego przepisu
    private void setQRData()
    {
        var qrcode = new QRCodeWriter();
        var qrValue = RecipeTitle + "\nSkładniki:\n" + RecipeIngredients + "\nSposób przygotowania:\n" + RecipeDescription; //ustawienie wyświetlanej zawartości

        var barcodeWriter = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = 1000,
                Width = 1000
            }
        };

        using (var bitmap = barcodeWriter.Write(qrValue))
        using (var stream = new MemoryStream())
        {
            bitmap.Save(stream, ImageFormat.Png);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            stream.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = stream;
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.EndInit();
            QrCode = bi; //binding
        }
    }

    //odświeżanie widoku
    virtual protected void OnPropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}