﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.Domain.Abstractions.Repository.Base
{
    public interface IGenericRepository<T,Key> where T : class, IDisposable, new()
    {
        Task<T> GetById(Key id);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> quando = null);
        Task Add(T obj);
        Task Update(T obj);
        Task Delete(T obj);
    }
}
