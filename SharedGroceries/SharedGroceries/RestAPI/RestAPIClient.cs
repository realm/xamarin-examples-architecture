using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace SharedGroceries.RestAPI
{
    public static class RestAPIClient
    {
        private static readonly string usersUrl;
        private static readonly string shoppingListsUrl;
        private static readonly HttpClient client;

        static RestAPIClient()
        {
            //Due to the way networking works on Android simulator
            //https://developer.android.com/studio/run/emulator-networking.html
            var baseAddress = (Device.RuntimePlatform == Device.Android) ? "http://10.0.2.2:5000" : "http://localhost:5000";
            usersUrl = $"{baseAddress}/users";
            shoppingListsUrl = $"{baseAddress}/shoppingLists";
            client = new HttpClient();
        }

        public static async Task<UserInfoDTO> Authenticate(string username, string password)
        {
            var url = $"{usersUrl}/authenticate?username={username}&password={password}";
            var result = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<UserInfoDTO>(result);
        }

        public static async Task<IEnumerable<UserInfoDTO>> GetAllUsers()
        {
            var result = await client.GetStringAsync(usersUrl);
            return JsonConvert.DeserializeObject<IEnumerable<UserInfoDTO>>(result);
        }

        public static async Task<IEnumerable<ShoppingListDTO>> GetShoppingListsForUser(Guid userId)
        {
            var url = $"{shoppingListsUrl}/{userId}";
            var result = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<IEnumerable<ShoppingListDTO>>(result);
        }

        public static async Task DeleteShoppingList(Guid listId)
        {
            var url = $"{shoppingListsUrl}/{listId}";
            await client.DeleteAsync(url);
        }

        public static async Task AddOrUpdateShoppingList(ShoppingListDTO shoppingList)
        {
            var content = new StringContent(JsonConvert.SerializeObject(shoppingList),
                Encoding.UTF8, "application/json");

            await client.PutAsync(shoppingListsUrl, content);
        }
    }
}

