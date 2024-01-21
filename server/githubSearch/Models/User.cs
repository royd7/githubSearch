namespace githubSearch.Models
{
    public class User
    {
        public string Token { get; set; }

        private List<Repo> _bookMarks;
        public List<Repo> Bookmarks
        {
            get
            {
                if (_bookMarks == null)
                    _bookMarks = new List<Repo>();
                return _bookMarks;
            }
        }
    }
}
