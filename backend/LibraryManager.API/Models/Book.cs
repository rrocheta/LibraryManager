namespace LibraryManager.API.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public bool IsBorrowed { get; set; }
    }
}
