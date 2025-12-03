using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureSerena
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        // NOVO MÉTODO: Busca por Foreign Key
        Task<IEnumerable<T>> FindAllByForeignKeyAsync(
        string foreignKeyName,
        object foreignKeyValue);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);

    }
}
