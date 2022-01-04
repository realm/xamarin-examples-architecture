namespace SharedGroceriesWebService.Models
{
    public class GroceryItem
    {
        public string Name { get; set; }
        public bool Purchased { get; set; }

        public static GroceryItem Create(string name, bool purchased)
        {
            return new GroceryItem { Name = name, Purchased = purchased };
        }
    }
}
