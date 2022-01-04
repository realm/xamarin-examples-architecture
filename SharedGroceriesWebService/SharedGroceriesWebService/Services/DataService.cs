using System;
using System.Collections.Generic;
using System.Linq;
using SharedGroceriesWebService.Models;

namespace SharedGroceriesWebService.Services
{
    public static class DataService
    {
        private static readonly Dictionary<Guid, User> users;
        private static readonly Dictionary<Guid, ShoppingList> shoppingLists;

        static DataService()
        {
            var alice = new User { Username = "alice", Password = "1234", Id = new Guid("9b8469f8-691e-4fd8-8363-718b9cebd784") };
            var bob = new User { Username = "bob", Password = "1234", Id = new Guid("79929376-deda-4378-ac31-4154ee27ba36") };
            var charlie = new User { Username = "charlie", Password = "1234", Id = new Guid("b5e26d07-5696-403e-9901-bae9595299f7") };

            var houseList = new ShoppingList { Name = "House", Id = new Guid("33c43e7e-1f28-4679-8a08-4975d8899cc7") };
            houseList.Owners.Add(alice.ToInfo());
            houseList.Owners.Add(bob.ToInfo());
            var houseItems = new List<GroceryItem>
            {
                GroceryItem.Create("banana", false),
                GroceryItem.Create("milk", true),
                GroceryItem.Create("eggs", false),
            };
            houseList.Items.AddRange(houseItems);

            var officeList = new ShoppingList { Name = "Office", Id = new Guid("0fc4fe3f-710b-4dc6-b109-40d67d8b5723") };
            officeList.Owners.Add(alice.ToInfo());
            var officeItems = new List<GroceryItem>
            {
                GroceryItem.Create("paper", false),
                GroceryItem.Create("coffee", false),
            };
            officeList.Items.AddRange(officeItems);

            alice.ShoppingLists.AddRange(new[] { houseList.Id, officeList.Id });
            bob.ShoppingLists.AddRange(new[] { houseList.Id });

            var defaultUsers = new[] { alice, bob, charlie };
            var defaultShoppingLists = new[] { houseList, officeList };

            users = defaultUsers.ToDictionary(u => u.Id);
            shoppingLists = defaultShoppingLists.ToDictionary(s => s.Id);
        }

        public static UserInfo Authenticate(string name, string password)
        {
            return users.Values.Where(u => u.Username == name && u.Password == password).SingleOrDefault()?.ToInfo();
        }

        public static List<UserInfo> GetAllUsers()
        {
            return users.Values.Select(u => u.ToInfo()).ToList();
        }

        public static List<ShoppingList> GetShoppingListsForUser(Guid? userId)
        {
            if (userId == null)
            {
                return null;
            }

            return users[userId.Value].ShoppingLists.Select(id => shoppingLists[id]).ToList();
        }

        public static void DeleteShoppingList(Guid? listId)
        {
            if (listId == null)
            {
                return;
            }

            if (shoppingLists.TryGetValue(listId.Value, out var list))
            {
                var owners = list.Owners;
                foreach (var owner in owners)
                {
                    users[owner.Id].ShoppingLists.Remove(list.Id);
                }
            }

            shoppingLists.Remove(listId.Value);
        }

        public static void AddOrUpdateShoppingList(ShoppingList list)
        {
            if (list == null)
            {
                return;
            }

            if (shoppingLists.TryGetValue(list.Id, out var oldShoppingList)) //Update
            {
                var oldOwners = oldShoppingList.Owners;
                foreach (var oldOwner in oldOwners)
                {
                    users[oldOwner.Id].ShoppingLists.Remove(list.Id);
                }
            }

            shoppingLists[list.Id] = list;
            foreach (var newOwner in list.Owners)
            {
                users[newOwner.Id].ShoppingLists.Add(list.Id);
            }
        }
    }
}
