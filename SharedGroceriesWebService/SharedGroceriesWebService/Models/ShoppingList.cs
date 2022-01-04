using System;
using System.Collections.Generic;

namespace SharedGroceriesWebService.Models
{
    public class ShoppingList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserInfo> Owners { get; set; } = new List<UserInfo>();
        public List<GroceryItem> Items { get; set; } = new List<GroceryItem>();
    }
}
