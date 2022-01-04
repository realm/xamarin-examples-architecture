using SharedGroceries.Services;
using SharedGroceries.ViewModels;
using Xamarin.Forms;

namespace SharedGroceries
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (DataService.CurrentUser == null)
            {
                NavigationService.SetMainPage(new LoginViewModel());
            }
            else
            {
                NavigationService.SetMainPage(new ShoppingListsCollectionViewModel());
            }
        }
    }
}
