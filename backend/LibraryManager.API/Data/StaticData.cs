using LibraryManager.API.Models;

namespace LibraryManager.API.Data
{
    public static class StaticData
    {
        public static readonly List<Author> Authors = new()
        {
            new Author { Id = 1, Name = "Fernando Pessoa" },
            new Author { Id = 2, Name = "Hernandes Dias lopes" },
            new Author { Id = 3, Name = "Reinhard Bonnke" }
        };

        public static readonly List<Publisher> Publishers = new()
        {
            new Publisher { Id = 1, Name = "Porto Editora" },
            new Publisher { Id = 2, Name = "Hagnos" },
            new Publisher { Id = 3, Name = "CPAD" }
        };
    }
}
