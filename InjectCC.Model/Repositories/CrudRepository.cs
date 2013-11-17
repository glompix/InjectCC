using InjectCC.Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InjectCC.Model.Repositories
{
    public abstract class CrudRepository<T> : BaseRepository
        where T : class
    {
        private static IDictionary<Type, PropertyInfo> _collectionProperties;
        private static object _collectionPropertiesLock;
        private static Type _contextType;
        static CrudRepository()
        {
            _collectionProperties = new Dictionary<Type, PropertyInfo>();
            _contextType = typeof(Context);
            _collectionPropertiesLock = new object();
        }

        private DbSet<T> _db;
        protected CrudRepository(UnitOfWork tx)
            : base(tx)
        {
            var type = typeof(T);
            lock (_collectionPropertiesLock)
            {
                if (!_collectionProperties.ContainsKey(type))
                {
                    var propertyName = pluralize(type.Name);
                    var propertyInfo = _contextType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
                    _collectionProperties[type] = propertyInfo;
                }
            }
            _db = (DbSet<T>)_collectionProperties[type].GetValue(tx.Context, null);
        }

        private string pluralize(string p)
        {
            // Naive, but that's okay for now.
            if (p.EndsWith("y"))
                return p.Substring(0, p.Length - 1) + "ies";
            else if (p.EndsWith("s"))
                return p + "es";
            else
                return p + "s";
        }

        public virtual void Add(T entity)
        {
            _db.Add(entity);
        }

        public virtual void Update(T entity)
        {
            // Nothing to do.
        }

        public virtual void Remove(T entity)
        {
            _db.Remove(entity);
        }
    }
}
