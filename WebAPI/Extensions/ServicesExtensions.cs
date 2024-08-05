using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
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

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerService, LoggerManager>();
    
    public static void ConfigureActionFilters(this IServiceCollection services)
    {
        services.AddScoped<IsModelStateNotValid>();
        services.AddSingleton<LoggerAction>();
        services.AddScoped<ValidateMediaType>();
    }
    
    // CORS policy ayarlamak icin
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")
            )
        );
    }

    public static void ConfigureDataShaper(this IServiceCollection services)
    {
        services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
    }
    
    // for hateoas
    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter = config
                .OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();

            if (systemTextJsonOutputFormatter is not null)
                systemTextJsonOutputFormatter
                    .SupportedMediaTypes
                    .Add("application/vnd.umutakpinar.hateoas+json"); // vnd = vendor

            var xmlOutputFormatter = config
                .OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()
                .FirstOrDefault();
            
            if(xmlOutputFormatter is not null)
                xmlOutputFormatter
                    .SupportedMediaTypes
                    .Add("application/vnd.umutakpinar.hateoas+xml");
        });
    }
    
    
}