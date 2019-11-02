using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TenantsApp.Repository
{
    public class Repository<T> : IDisposable where T : class, new()
    {


        private DBContext context;
        private SQLiteConnection _db;
        protected static object collisionLock = new object();


        public Repository(DBContext _context)
        {
            context = _context;
            _db = context.GetConnection();

        }

        public virtual List<T> GetAll()
        {           
            return _db.Table<T>().ToList();
        }


        public virtual List<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {           
            var query = _db.Table<T>();
            if (predicate != null)
                query = query.Where(predicate);

            return query.ToList();

        }

        public virtual T Get(Guid id)
        {
            
            return _db.Find<T>(id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
         
            return _db.Find<T>(predicate);
        }

        public virtual bool Insert(T entity)
        {
            if (_db.Insert(entity)>0)
            {
                return true;
            }
            return false;
        }

        public virtual bool Insert(IEnumerable<T> entities)
        {
            if (_db.InsertAll(entities) > 0)
            {
                return true;
            }
            return false;            
        }

        public virtual bool Update(T entity)
        {
            if (_db.Update(entity, entity.GetType()) > 0)
            {
                return true;
            }
            return false;            
        }

        public virtual bool Update(IEnumerable<T> entities)
        {
            if (_db.UpdateAll(entities) > 0)
            {
                return true;
            }
            return false;
        }

        public virtual int Delete(T entity)
        {          
            return _db.Delete(entity);
        }
        public virtual int Delete(IEnumerable<T> entities)
        {
            int result = 0;
            foreach (var item in entities)
            {
                result = Delete(item);
            }
            return result;
        }

        public int DeleteAll<T>()
        {            
            return _db.DeleteAll<T>();
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
