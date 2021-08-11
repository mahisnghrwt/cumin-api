using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace cumin_api.Services {
    public class DbService<TEntity> where TEntity: class {
        protected CuminApiContext context;
        protected DbSet<TEntity> dbSet;
        public DbService(CuminApiContext context) {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public TEntity FindById(int id) {
            return dbSet.Find(id);
        }
        public IEnumerable<TEntity> GetAll() {
            return dbSet.AsEnumerable();
        }

        public TEntity Find(System.Func<TEntity, bool> filter) {
            return dbSet.FirstOrDefault(filter);
        }

        public IEnumerable<TEntity> GetAllFiltered(List<System.Func<TEntity, bool>> filters) {
            if (filters== null)
                return GetAll();
            System.Func<TEntity, bool> filter = x => {
                var result = true;

                foreach (var filter_ in filters) {
                    if (filter_(x) == false) {
                        result = false;
                        break;
                    }
                };

                return result;
            };

            return dbSet.Where(filter).AsEnumerable();
        }

        public TEntity Add(TEntity entity) {
            var added = dbSet.Add(entity);
            context.SaveChanges();
            return added.Entity;
        }
        public TEntity Update(TEntity entity) {
            dbSet.Attach(entity);
            var trackedEntity = context.Entry(entity);
            trackedEntity.State = EntityState.Modified;
            SaveChanges();

            return trackedEntity.Entity;
        }
        public void DeleteById(int id) {
            TEntity entity = FindById(id);
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void SaveChanges() {
            context.SaveChanges();
        }
    }
}
