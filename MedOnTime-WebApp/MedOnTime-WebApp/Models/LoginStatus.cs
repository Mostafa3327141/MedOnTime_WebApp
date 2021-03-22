using MongoDB.Bson;

namespace MedOnTime_WebApp.Models
{
    public static class LoginStatus
    {
        public static bool IsLoggedIn { get; set; } = false;
        public static Caretaker LogginedUser { get; set; }
    }
}
