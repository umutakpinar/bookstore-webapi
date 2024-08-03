using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { 1, 100m, "Devlet" },
                    { 2, 200m, "Sefiller" },
                    { 3, 150m, "Suç ve Ceza" },
                    { 4, 120m, "Beyaz Zambaklar Ülkesinde" },
                    { 5, 180m, "İnce Memed" },
                    { 6, 110m, "İstanbul Hatırası" },
                    { 7, 90m, "Kuyucaklı Yusuf" },
                    { 8, 110m, "Kürk Mantolu Madonna" },
                    { 9, 90m, "Alıştırmalarla SQL" },
                    { 10, 50m, "Prens" },
                    { 11, 33m, "Dönüşüm" },
                    { 12, 21m, "Karamazov Kardeşler" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
