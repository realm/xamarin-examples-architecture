using System.Threading.Tasks;
using System.Windows.Input;
using SharedGroceries.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace SharedGroceries.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; }

        public string Username { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            Username = "alice";
            Password = "1234"; //TODO Testing

            LoginCommand = new AsyncCommand(Login);
        }

        private async Task Login()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await DialogService.ShowAlert("Error", "Username or password are empty");
            }

            var loadingIndicator = DialogService.ShowLoading();
            try
            {
                var authenticationSuccess = await DataService.AuthenticateUser(Username, Password);
                loadingIndicator?.Dispose();
                if (authenticationSuccess)
                {
                    await NavigationService.NavigateTo(new ShoppingListsCollectionViewModel());
                }
                else
                {
                    await DialogService.ShowAlert("Error", "Wrong username and/or password");
                }
            }
            catch
            {
                loadingIndicator?.Dispose();
                await DialogService.ShowAlert("Error", "Service is not reachable");
            }
        }
    }
}
