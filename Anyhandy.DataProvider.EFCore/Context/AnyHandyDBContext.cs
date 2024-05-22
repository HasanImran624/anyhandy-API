using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Anyhandy.DataProvider.EFCore.Context
{
    public class AnyHandyDBContext<T>: AnyhandyBaseDBContext where T : class
    {
        public int Id { get; set; }

        public DbSet<T> entities { get; set; }

        public int Save(T obj)
        {
            entities.Add(obj);
            return SaveChanges();
        }
        public async Task<int> SaveAsync(T obj)
        {
            entities.Add(obj);
            return await SaveChangesAsync();
        }
        public int Save(List<T> objList)
        {
            entities.AddRange(objList);
            return SaveChanges();
        }
    
        public virtual void Update(T currentobj, T modifiedobj)
        {
            Entry(currentobj).CurrentValues.SetValues(modifiedobj);
            SaveChanges();
        }

        public virtual void Update()
        {
            SaveChanges();
        }
        public int BulkSave(List<T> obj)
        {
            entities.AddRange(obj);
            return SaveChanges();
        }

        public int BulkUpdate()
        {
            return SaveChanges();
        }
        public int BulkRemove(List<T> obj)
        {
            entities.RemoveRange(obj);
            return SaveChanges();
        }
        public int Remove(T obj)
        {
            entities.Remove(obj);
            return SaveChanges();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition)
        {
            return this.entities.Where(condition);
        }

        public T GetObjetByCondition(Expression<Func<T, bool>> condition)
        {
            return this.entities.FirstOrDefault(condition);
        }

        public IEnumerable<T> GetObjectListByCondition(Expression<Func<T, bool>> condition)
        {
            return this.entities.Where(condition);
        }
    }
}
