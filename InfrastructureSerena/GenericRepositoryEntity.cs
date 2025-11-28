using InfrastructureSerena;
using DominioSerena;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace InfrastructureSerena
{
    public class RepositoryEntity<T>: IRepositoryGeneric<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryEntity(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync(int id, string nomeId)
        {
           return await _dbSet.Where(e => EF.Property<int>(e, nomeId) == id).ToListAsync();

        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

    }
}
