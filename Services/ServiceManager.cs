using Microsoft.Extensions.DependencyInjection;
using Services.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IBookService> _bookService;
    
    public ServiceManager(IServiceProvider serviceProvider)
    {
        _bookService = new Lazy<IBookService>(serviceProvider.GetRequiredService<IBookService>());
    }
    
    public IBookService BookService => _bookService.Value;
    
}