using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;

namespace WebAPI.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(
            optionsBuilder => optionsBuilder.UseSqlite(configuration.GetConnectionString("sqlConnection"))
        );
    }
    
    // burada her bir repository'i tek tek ekyecek miyim acaba  respoitorymanager'da lazy loading ile newlemişti ben yine de IoC'ye kaydetmem gerektiğini düşündüm
    // ihtiyac olduğunda GetRequiredService diyerek alabiliyorum örneği RepositoryManager'da var.
    public static void ConfigureIBookRepository(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
    }

    // constructorda bir IRepositoryManager istersem bana RepositoryManager ver (IoC - Dependency Injection)
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureBookService(this IServiceCollection services) => services.AddScoped<IBookService,BookManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

}