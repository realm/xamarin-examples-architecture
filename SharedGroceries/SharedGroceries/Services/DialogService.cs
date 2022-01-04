using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace SharedGroceries.Services
{
    public static class DialogService
    {
        public static async Task ShowAlert(string title, string message, string okText = "Ok")
        {
            await UserDialogs.Instance.AlertAsync(message, title, okText);
        }

        public static async Task<bool> ShowConfirm(string title, string message,
            string okText = "Ok", string cancelText = "Cancel")
        {
            return await UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText);
        }

        public static async Task<string> ShowActionSheet(string title, string cancel,
            string destruction, string[] buttons)
        {
            return await UserDialogs.Instance.ActionSheetAsync(title, cancel, destruction, null, buttons);
        }

        public static IDisposable ShowLoading()
        {
            return UserDialogs.Instance.Loading();
        }
    }
}
