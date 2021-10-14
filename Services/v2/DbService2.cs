using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class DbService2<T> where T : class {
        protected CuminApiContext context;
        protected DbSet<T> dbSet;

        public DbService2 (CuminApiContext context) {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity) {
            var ent = context.Add(entity);
            await context.SaveChangesAsync();
            return ent.Entity;
        }

        public T FindById(int id) {
            return dbSet.Find(id);
        }

        public T Find(Expression<Func<T, bool>> expression) {
            return dbSet.FirstOrDefault(expression);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression) {
            return await dbSet.FirstOrDefaultAsync(expression);
        }

        public IEnumerable<T> GetAll() {
            return dbSet.AsNoTracking().ToList();
        }

        public async void DeleteByIdAsync(int id) {
            var ent = FindById(id);
            if (ent != null) {
                dbSet.Remove(ent);
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(T entity) {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T updatedEntity) {
            context.Update(updatedEntity);
            await context.SaveChangesAsync();
        }
    }
}
