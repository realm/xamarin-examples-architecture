using System;
using Realms;

namespace SharedGroceries.Models
{
    public class UserInfo : RealmObject
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
