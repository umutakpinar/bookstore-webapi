using Repositories.EFCore;

namespace Repositories.Contracts;

public interface IRepositoryManager
{ 
    IBookRepository BookRepo { get; }
    void Save();
}