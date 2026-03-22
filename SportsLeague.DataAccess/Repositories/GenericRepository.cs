using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : AuditBase
    {
        protected readonly LeagueDbContext _context;
        protected readonly DbSet<T> _dbset;

        public GenericRepository(LeagueDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();

        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            entity.CreateAt = DateTime.UtcNow;
            entity.UpdateAt = null;
            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(T entity)
        {
            entity.UpdateAt = DateTime.UtcNow;
            _dbset.Update(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbset.Remove(entity);
                await _context.SaveChangesAsync();
            }

        } 
        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbset.AnyAsync(e => e.Id == id);
        }
    }

}
