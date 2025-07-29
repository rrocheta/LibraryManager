namespace LibraryManager.API.Dtos
{
    /// <summary>
    /// Data Transfer Object for Author entity.
    /// </summary>
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
