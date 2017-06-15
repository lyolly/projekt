using CookingBook.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

public class MainViewModel : INotifyPropertyChanged
{
    private string _Page = null;

    public MainViewModel()
    {
        SearchCmd = new RelayCommand(x => Search());
        HomeCmd = new RelayCommand(x => Home());
        AddCmd = new RelayCommand(x => Add());
        CloseCmd = new RelayCommand(x => Close());
        Page = "Pages/HomePage.xaml";
    }

    public event PropertyChangedEventHandler PropertyChanged = null;

    public ICommand AddCmd { get; set; }
    public ICommand CloseCmd { get; set; }
    public ICommand HomeCmd { get; set; }
    public ICommand SearchCmd { get; set; }
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

    //przycisk dodawania przepisu
    public void Add()
    {
        Page = "Pages/AddRecipePage.xaml";
        SharedVM.RecipeVM.IsAdd = true;
        SharedVM.RecipeVM.IsEdit = false;
        SharedVM.RecipeVM.ClearPage();
    }

    //przycisk strony glownej
    public void Home()
    {
        Page = "Pages/HomePage.xaml";
        SharedVM.HomeVM.Page = null;
        SharedVM.HomeVM.Refresh();
    }

    //przycisk wyjscia
    private void Close()
    {
        Application.Current.MainWindow.Close();
    }

    //przycisk wyszukiwania przepisow
    private void Search()
    {
        Page = "Pages/SearchPage.xaml";
        SharedVM.SearchVM.Refresh();
    }

    //odświeżanie widoku
    virtual protected void OnPropertyChanged(string propName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}