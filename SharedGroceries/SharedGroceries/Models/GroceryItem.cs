using Realms;

namespace SharedGroceries.Models
{
    public class GroceryItem : EmbeddedObject
    {
        public string Name { get; set; }
        public bool Purchased { get; set; }
    }
}
