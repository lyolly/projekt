namespace CookingBook.ViewModels
{
    public static class SharedVM
    {
        static SharedVM()
        {
            MainVM = new MainViewModel();
            HomeVM = new HomePageViewModel();
            RecipeVM = new AddRecipePageViewModel();
            SearchVM = new SearchPageViewModel();
        }

        public static HomePageViewModel HomeVM { get; set; }
        public static MainViewModel MainVM { get; set; }
        public static AddRecipePageViewModel RecipeVM { get; set; }
        public static SearchPageViewModel SearchVM { get; set; }
    }
}