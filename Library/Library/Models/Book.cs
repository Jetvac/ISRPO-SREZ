namespace Library.Models 
{
    public class Book
    {
        public string Title { get; set; } = null;
        public string Author { get; set; }     = null;
        public string Genre { get; set; }      = null;
        public string SubGenre { get; set; }   = null;
        public string Height { get; set; }     = null;
        public string Publisher { get; set; }  = null;

        public string PublisherEx
        {
            get
            {
                return Publisher.Length == 0? "Не указано": Publisher;
            }
        }
        public string AuthorTitle
        {
            get
            {
                return $"{Author} {Title}";
            }
        }
    }
}