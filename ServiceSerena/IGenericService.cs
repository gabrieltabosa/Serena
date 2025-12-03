using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSerena
{
    public interface IGenericService<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        // Operação Genérica de Busca por Chave Estrangeira
        // Usada para qualquer entidade T que precise filtrar por uma FK
        Task<IEnumerable<T>> FindAllByForeignKeyAsync(string foreignKeyName, object foreignKeyValue);


    }
    
       
    
}
