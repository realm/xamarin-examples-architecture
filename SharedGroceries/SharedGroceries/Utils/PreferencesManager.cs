using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SharedGroceries.Models;
using Xamarin.Essentials;

namespace SharedGroceries.Utils
{
    public static class PreferencesManager
    {
        private const string currentUserKey = "currentUserKey";
        private const string editingListKey = "editingListsKey";
        private const string readyForSyncListsKey = "readyForSyncListsKey";

        public static Guid? GetEditingListId()
        {
            var editingListValue = Preferences.Get(editingListKey, null);
            if (string.IsNullOrEmpty(editingListValue))
            {
                return null;
            }

            return Guid.Parse(editingListValue);
        }

        public static void SetEditingListId(Guid listId)
        {
            Preferences.Set(editingListKey, listId.ToString());
        }

        public static void RemoveEditingListId()
        {
            Preferences.Remove(editingListKey);
        }

        public static HashSet<Guid> GetReadyForSyncListsId()
        {
            HashSet<Guid> editedListsIds;
            var editedListsValue = Preferences.Get(readyForSyncListsKey, null);
            if (string.IsNullOrEmpty(editedListsValue))
            {
                editedListsIds = new HashSet<Guid>();
            }
            else
            {
                editedListsIds = JsonConvert.DeserializeObject<HashSet<Guid>>(editedListsValue);
            }

            return editedListsIds;
        }

        public static void AddReadyForSyncListId(Guid listId)
        {
            var readyForSyncListId = GetReadyForSyncListsId();
            readyForSyncListId.Add(listId);
            SetReadyForSyncListsIds(readyForSyncListId);
        }

        public static void RemoveReadyForSyncListId(Guid listId)
        {
            var editedListsIds = GetReadyForSyncListsId();
            editedListsIds.Remove(listId);
            SetReadyForSyncListsIds(editedListsIds);
        }

        private static void SetReadyForSyncListsIds(HashSet<Guid> editedListsIds)
        {
            Preferences.Set(readyForSyncListsKey, JsonConvert.SerializeObject(editedListsIds));
        }

        public static UserInfo GetCurrentUser()
        {
            var currentUserValue = Preferences.Get(currentUserKey, null);

            if (string.IsNullOrEmpty(currentUserValue))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UserInfo>(currentUserValue);
        }

        public static void SetCurrentUser(UserInfo user)
        {
            Preferences.Set(currentUserKey, JsonConvert.SerializeObject(user));
        }
    }
}
