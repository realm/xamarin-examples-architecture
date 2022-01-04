using System;
using System.Collections.Generic;
using SharedGroceries.Models;

namespace SharedGroceries.RestAPI
{
    public class ShoppingListDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<UserInfoDTO> Owners { get; set; } = new List<UserInfoDTO>();
        public List<GroceryItemDTO> Items { get; set; } = new List<GroceryItemDTO>();

        public ShoppingList ToModel()
        {
            var sl = new ShoppingList
            {
                Id = Id,
                Name = Name,

            };
            Items.ForEach(i => sl.Items.Add(i.ToModel()));
            Owners.ForEach(o => sl.Owners.Add(o.ToModel()));
            return sl;
        }

        public static ShoppingListDTO FromModel(ShoppingList list)
        {
            var dto = new ShoppingListDTO
            {
                Id = list.Id,
                Name = list.Name,
            };

            foreach (var item in list.Items)
            {
                dto.Items.Add(GroceryItemDTO.FromModel(item));
            }

            foreach (var owner in list.Owners)
            {
                dto.Owners.Add(UserInfoDTO.FromModel(owner));
            }

            return dto;
        }
    }

}
