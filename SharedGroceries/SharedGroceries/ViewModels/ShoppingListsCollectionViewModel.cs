using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Realms;
using SharedGroceries.Models;
using SharedGroceries.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace SharedGroceries.ViewModels
{
    public class ShoppingListsCollectionViewModel : BaseViewModel
    {
        private readonly Realm realm;
        private bool loaded;

        public ICommand AddListCommand { get; }
        public ICommand OpenListCommand { get; }

        public IEnumerable<ShoppingList> Lists { get; }

        public ShoppingList SelectedList
        {
            get => null;
            set
            {
                OpenListCommand.Execute(value);
                OnPropertyChanged();
            }
        }

        public ShoppingListsCollectionViewModel()
        {
            realm = RealmService.GetRealm();
            Lists = realm.All<ShoppingList>();

            AddListCommand = new AsyncCommand(AddList);
            OpenListCommand = new AsyncCommand<ShoppingList>(OpenList);
        }

        internal override async void OnAppearing()
        {
            base.OnAppearing();

            IDisposable loadingIndicator = null;

            try
            {

                if (!loaded)
                {
                    //Page is appearing for the first time, sync with service
                    //and retrieve users and shopping lists
                    loaded = true;
                    loadingIndicator = DialogService.ShowLoading();
                    await DataService.TrySync();
                    await DataService.RetrieveUsers();
                    await DataService.RetrieveShoppingLists();
                }
                else
                {
                    DataService.FinishEditing();
                }
            }
            catch
            {
                await DialogService.ShowAlert("Error", "Error while loading the page");
            }
            finally
            {
                loadingIndicator?.Dispose();
            }
        }

        #region Commands

        private async Task AddList()
        {
            var newList = new ShoppingList();
            newList.Owners.Add(DataService.CurrentUser);
            realm.Write(() =>
            {
                return realm.Add(newList, true);
            });

            await OpenList(newList);
        }

        private async Task OpenList(ShoppingList list)
        {
            DataService.StartEditing(list.Id);
            await NavigationService.NavigateTo(new ShoppingListViewModel(list));
        }

        #endregion
    }
}
