using System;
using System.Linq;

using TestStack.Seleno.Samples.Movies.Core.Domain;

namespace TestStack.Seleno.Samples.Movies.Core.Services
{
    public interface IRepository<T> where T : Entity
    {
        T FindById(Guid id);
        IQueryable<T> GetAll();
        T SaveOrUpdate(T entity);
    }
}