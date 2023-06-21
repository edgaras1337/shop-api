using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace api.Repo
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int first = 0, int offset = 0,
            params Expression<Func<TEntity, object>>[] relations)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (offset > 0)
            {
                query = query.Skip(offset);
            }
            if (first > 0)
            {
                query = query.Take(first);
            }

            query = relations.Aggregate(query, (current, relation) => current.Include(relation));

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var res = await query.ToListAsync();
            return res;
        }


        public IQueryable<TEntity> GetAllQuery()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() 
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
