using System;
using System.Collections.Generic;
using Realms;

namespace SharedGroceries.Models
{
    public class ShoppingList : RealmObject
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public ISet<UserInfo> Owners { get; }
        public IList<GroceryItem> Items { get; }
    }
}
