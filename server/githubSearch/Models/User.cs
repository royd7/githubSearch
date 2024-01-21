namespace githubSearch.Models
{
    public class User
    {
        public string Token { get; set; }

        private List<int> _bookMarks;
        public List<int> Bookmarks
        {
            get
            {
                if (_bookMarks == null)
                    _bookMarks = new List<int>();
                return _bookMarks;
            }
        }
    }
}
