using githubSearch.Models;

namespace githubSearch.Sessions
{
    public static class Sessions
    {
        private static List<User> _list;
         static List<User> UserSessions
        {
            get
            {
                if(_list == null) 
                    _list = new List<User> ();
                return _list;   
            }
        }

        internal static User GetString(string? token)
        {
            var user = UserSessions.FirstOrDefault(s => s.Token == token);

            return user;

        }

        internal static void SetString(string token)
        {
            if(GetString(token) == null)
                UserSessions.Add(new User { Token = token });
        }
    }
}
