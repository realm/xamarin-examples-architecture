using SharedGroceries.Models;

namespace SharedGroceries.RestAPI
{
    public class GroceryItemDTO
    {
        public string Name { get; set; }
        public bool Purchased { get; set; }

        public GroceryItem ToModel()
        {
            return new GroceryItem
            {
                Name = Name,
                Purchased = Purchased,
            };
        }

        public static GroceryItemDTO FromModel(GroceryItem item)
        {
            return new GroceryItemDTO
            {
                Name = item.Name,
                Purchased = item.Purchased,
            };
        }
    }
}
