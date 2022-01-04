using System;
using System.Collections.Generic;

namespace SharedGroceriesWebService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Guid> ShoppingLists { get; set; } = new List<Guid>();

        public UserInfo ToInfo()
        {
            return new UserInfo
            {
                Id = Id,
                Name = Username,
            };
        }
    }
}
