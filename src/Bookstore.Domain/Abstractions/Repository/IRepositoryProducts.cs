using Bookstore.Domain.Abstractions.DomainInterfaces;
using Bookstore.Domain.Abstractions.Repository.Base;
using Bookstore.Domain.Entities;
namespace Bookstore.Domain.Abstractions.Repository
{
    public interface IRepositoryProducts : IUnitOfWork, IGenericRepository<Product, int>
    {
    }
}
