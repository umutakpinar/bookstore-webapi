using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI.Repositories;

public class RepositoryContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    
    public RepositoryContext(DbContextOptions options) : base(options)
    {
        
    }
}