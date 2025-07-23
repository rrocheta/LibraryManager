namespace LibraryManager.API.Dtos
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public bool IsBorrowed { get; set; }
    }
}
