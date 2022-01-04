using Realms;

namespace SharedGroceries.Services
{
    public static class RealmService
    {
        public static Realm GetRealm() => Realm.GetInstance();
    }
}
