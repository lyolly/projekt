using CookingBook.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace CookingBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = SharedVM.MainVM;
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Nie udało się połączyć z bazą.");
                Environment.Exit(0);
            }
        }
    }
}