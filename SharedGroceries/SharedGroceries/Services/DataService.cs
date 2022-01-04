using System;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using SharedGroceries.Models;
using SharedGroceries.RestAPI;
using SharedGroceries.Utils;

namespace SharedGroceries.Services
{
    public static class DataService
    {
        public static UserInfo CurrentUser => PreferencesManager.GetCurrentUser();

        public static async Task<bool> AuthenticateUser(string username, string password)
        {
            var userInfo = await RestAPIClient.Authenticate(username, password);
            if (userInfo == null)
            {
                return false;
            }

            PreferencesManager.SetCurrentUser(userInfo.ToModel());
            return true;
        }

        public static async Task RetrieveUsers()
        {
            try
            {
                var users = await RestAPIClient.GetAllUsers();

                using var realm = RealmService.GetRealm();
                realm.Write(() =>
                {
                    realm.Add(users.Select(u => u.ToModel()), update: true);
                });
            }
            catch (HttpRequestException) //Offline/Service is not reachable
            {
            }
        }

        public static async Task RetrieveShoppingLists()
        {
            try
            {
                var shopListsDto = await RestAPIClient.GetShoppingListsForUser(CurrentUser.Id);

                using var realm = RealmService.GetRealm();
                realm.Write(() =>
                {
                    realm.Add(shopListsDto.Select(s => s.ToModel()), update: true);
                });
            }
            catch (HttpRequestException) //Offline/Service is not reachable
            {
            }
        }

        private static async Task<bool> UpdateShoppingList(Guid listId)
        {
            try
            {
                using var realm = RealmService.GetRealm();
                var shoppingList = realm.Find<ShoppingList>(listId);
                if (shoppingList == null) //List was deleted
                {
                    await RestAPIClient.DeleteShoppingList(listId);
                }
                else  //List was created or modified
                {
                    await RestAPIClient.AddOrUpdateShoppingList(ShoppingListDTO.FromModel(shoppingList));
                }

                return true;
            }
            catch (HttpRequestException) //Offline/Service is not reachable
            {
                return false;
            }
        }

        public static void StartEditing(Guid listId)
        {
            PreferencesManager.SetEditingListId(listId);
        }

        public static void FinishEditing()
        {
            var editingListId = PreferencesManager.GetEditingListId();

            if (editingListId == null)
            {
                return;
            }

            PreferencesManager.AddReadyForSyncListId(editingListId.Value);
            PreferencesManager.RemoveEditingListId();

            Task.Run(TrySync);
        }

        public static async Task TrySync()
        {
            var readyForSyncListsId = PreferencesManager.GetReadyForSyncListsId();
            var editingListId = PreferencesManager.GetEditingListId();

            foreach (var readyForSyncListId in readyForSyncListsId)
            {
                if (readyForSyncListId == editingListId)  //The list is still being edited
                {
                    continue;
                }

                var updateSuccessful = await UpdateShoppingList(readyForSyncListId);
                if (updateSuccessful)
                {
                    PreferencesManager.RemoveReadyForSyncListId(readyForSyncListId);
                }
            }
        }
    }
}
