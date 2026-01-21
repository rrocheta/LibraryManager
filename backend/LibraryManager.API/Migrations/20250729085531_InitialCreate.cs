using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManager.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    PublisherId = table.Column<int>(type: "integer", nullable: false),
                    IsBorrowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Authors (real, recognizable)
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fernando Pessoa" },
                    { 2, "Jane Austen" },
                    { 3, "George Orwell" },
                    { 4, "Fyodor Dostoevsky" },
                    { 5, "Mary Shelley" }
                });

            // Publishers (generic to avoid edition-specific mismatches)
            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Classic Literature Press" },
                    { 2, "Public Domain Books" },
                    { 3, "Demo Publisher" }
                });

            // Books (15 items to demonstrate pagination: 10 + 5)
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "IsBorrowed", "PublisherId", "Title" },
                values: new object[,]
                {
                    // Fernando Pessoa (3)
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c001"), 1, false, 1, "The Book of Disquiet" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c002"), 1, false, 1, "Mensagem" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c003"), 1, false, 3, "Selected Poems" },

                    // Jane Austen (4)
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c004"), 2, false, 2, "Pride and Prejudice" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c005"), 2, false, 2, "Sense and Sensibility" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c006"), 2, false, 2, "Emma" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c007"), 2, false, 2, "Persuasion" },

                    // George Orwell (3) - mark one as borrowed for UX demo
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c008"), 3, true,  1, "1984" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c009"), 3, false, 1, "Animal Farm" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00a"), 3, false, 1, "Homage to Catalonia" },

                    // Fyodor Dostoevsky (3) - mark one as borrowed for UX demo
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00b"), 4, true,  3, "Crime and Punishment" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00c"), 4, false, 3, "The Brothers Karamazov" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00d"), 4, false, 3, "Notes from Underground" },

                    // Mary Shelley (2)
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00e"), 5, false, 2, "Frankenstein" },
                    { new Guid("b1d8b2dd-2d77-4b42-9dc6-7f4c1ec4c00f"), 5, false, 2, "The Last Man" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Books");
            migrationBuilder.DropTable(name: "Publishers");
            migrationBuilder.DropTable(name: "Authors");
        }
    }
}
