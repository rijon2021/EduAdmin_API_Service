using DotNet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Services.Repositories.Infrastructure
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DotNetContext _dotnetContext;

        protected GenericRepository(DotNetContext dotnetContext)
        {
            _dotnetContext = dotnetContext;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dotnetContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByID(int id)
        {
            return await _dotnetContext.Set<T>().FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _dotnetContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dotnetContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dotnetContext.Set<T>().Update(entity);
        }
    }
}
