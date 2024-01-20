namespace githubSearch.Models
{
    public class User
    {
        public static readonly string SECRET = "AABBCC==";

        public string Token { get; set; }

        private List<BookMark> _bookMarks;
        public List<BookMark> Bookmarks
        {
            get
            {
                if (_bookMarks == null)
                    _bookMarks = new List<BookMark>();
                return _bookMarks;
            }
        }
    }
}
