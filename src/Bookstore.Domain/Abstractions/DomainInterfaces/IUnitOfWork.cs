using System.Threading.Tasks;
namespace Bookstore.Domain.Abstractions.DomainInterfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
        Task Rollback();
    }
}
