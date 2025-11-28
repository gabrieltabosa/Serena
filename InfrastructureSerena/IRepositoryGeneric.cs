using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureSerena
{
    public interface IRepositoryGeneric<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(int id, string propriedade);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

    }
}
