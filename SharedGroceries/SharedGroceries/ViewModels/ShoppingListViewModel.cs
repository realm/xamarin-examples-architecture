using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Realms;
using SharedGroceries.Models;
using SharedGroceries.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace SharedGroceries.ViewModels
{
    public class ShoppingListViewModel : BaseViewModel
    {
        private readonly Realm realm;

        public ShoppingList ShoppingList { get; }
        public IEnumerable<GroceryItem> CheckedItems { get; }
        public IEnumerable<GroceryItem> UncheckedItems { get; }

        public ICommand DeleteItemCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand DeleteCommand { get; }

        public ShoppingListViewModel(ShoppingList list)
        {
            realm = RealmService.GetRealm();

            ShoppingList = list;

            CheckedItems = ShoppingList.Items.AsRealmQueryable().Where(i => i.Purchased);
            UncheckedItems = ShoppingList.Items.AsRealmQueryable().Where(i => !i.Purchased);

            DeleteItemCommand = new Command<GroceryItem>(DeleteItem);
            AddItemCommand = new Command(AddItem);
            DeleteCommand = new AsyncCommand(Delete);
        }

        #region Commands

        private void AddItem()
        {
            realm.Write(() =>
            {
                ShoppingList.Items.Add(new GroceryItem());
            });
        }

        private void DeleteItem(GroceryItem item)
        {
            realm.Write(() =>
            {
                ShoppingList.Items.Remove(item);
            });
        }

        private async Task Delete()
        {
            var confirmDelete = await DialogService.ShowConfirm("Deletion",
                "Are you sure you want to delete the shopping list?");

            if (!confirmDelete)
            {
                return;
            }

            realm.Write(() =>
            {
                realm.Remove(ShoppingList);
            });

            await NavigationService.GoBack();
        }

        #endregion
    }
}
