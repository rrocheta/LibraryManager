namespace LibraryManager.API.Models
{
    /// <summary>
    /// Represents an author entity in the library system.
    /// </summary>
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual IEnumerable<Book> Books { get; set; }
    }
}
