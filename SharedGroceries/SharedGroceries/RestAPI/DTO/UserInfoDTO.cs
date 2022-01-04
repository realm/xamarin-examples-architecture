using System;
using SharedGroceries.Models;

namespace SharedGroceries.RestAPI
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public UserInfo ToModel()
        {
            return new UserInfo
            {
                Id = Id,
                Name = Name,
            };
        }

        public static UserInfoDTO FromModel(UserInfo user)
        {
            return new UserInfoDTO
            {
                Id = user.Id,
                Name = user.Name,
            };
        }
    }

}
