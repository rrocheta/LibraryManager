namespace LibraryManager.API.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual IEnumerable<Book> Books { get; set; }
    }
}
