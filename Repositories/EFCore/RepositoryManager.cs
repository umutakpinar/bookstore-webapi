using Microsoft.Extensions.DependencyInjection;
using Repositories.Contracts;

namespace Repositories.EFCore;

public class RepositoryManager : IRepositoryManager
{
    private RepositoryContext _context;
    private readonly Lazy<IBookRepository> _bookRepository;
    public RepositoryManager(RepositoryContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _bookRepository = new Lazy<IBookRepository>(serviceProvider.GetRequiredService<IBookRepository>);
    }
    
    // Dependency Injection modülleriyle uğraşmamak için şimdilik newleme yaptık normalde böylebir uygulama doğru değil
    // class içinde başka bir class newlenirse sıkı bağlı bir uygulama yapmış oluruz aslında aşağıdaki yaklaşım yanlış yani
    // burada birden fazla repository olabilir sadece birinde değişiklik yaptıysak diğerlerinden instance  oluşturmaya gerek yok o nedenle yukarıda Lazy loading yyaısı kurduk
    public IBookRepository BookRepo => _bookRepository.Value;
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}