namespace LibraryManager.API.Models
{
    /// <summary>
    /// Represents an author entity in the library system.
    /// </summary>
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual IEnumerable<Book> Books { get; set; }
    }
}
