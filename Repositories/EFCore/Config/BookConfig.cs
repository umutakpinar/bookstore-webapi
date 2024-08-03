using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config;


//for creating duummy data if nothing inside Books table
public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(
            new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title = "Devlet",
                    Price = 100
                },
                new Book()
                {
                    Id = 2,
                    Title = "Sefiller",
                    Price = 200
                },
                new Book()
                {
                    Id = 3,
                    Title = "Suç ve Ceza",
                    Price = 150
                },
                new Book()
                {
                    Id = 4,
                    Title = "Beyaz Zambaklar Ülkesinde",
                    Price = 120
                },
                new Book()
                {
                    Id = 5,
                    Title = "İnce Memed",
                    Price = 180
                },
                new Book()
                {
                    Id = 6,
                    Title = "İstanbul Hatırası",
                    Price = 110
                },
                new Book()
                {
                    Id = 7,
                    Title = "Kuyucaklı Yusuf",
                    Price = 90
                },
                new Book()
                {
                    Id = 8,
                    Title = "Kürk Mantolu Madonna",
                    Price = 110
                },
                new Book()
                {
                    Id = 9,
                    Title = "Alıştırmalarla SQL",
                    Price = 90
                },
                new Book()
                {
                    Id = 10,
                    Title = "Prens",
                    Price = 50
                },
                new Book()
                {
                    Id = 11,
                    Title = "Dönüşüm",
                    Price = 33
                },
                new Book()
                {
                    Id = 12,
                    Title = "Karamazov Kardeşler",
                    Price = 21
                },
            }
            );
    }
}