using System;
using System.Linq;
using System.Linq.Expressions;
using IHomer.GenericRepository.Interfaces;
using System.Data.Entity;

namespace IHomer.GenericRepository
{
    public abstract class GenericRepository<T, TC, TId> : IGenericRepository<T, TId>
        where T : class, IEntity<TId>
        where TC : DbContext
        where TId : IComparable<TId> 
    {
        protected IUnitOfWork<TC> Context;

        protected GenericRepository(IUnitOfWork<TC> context)
        {
            Context = context;
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = Context.Entities.Set<T>();
            return query;
        }

        public virtual IOrderedQueryable<T> GetAll(bool ascending, int skip, int take)
        {
            return @ascending
                       ? GetAll()
                            .OrderBy(t => t.Id)
                            .Skip(skip)
                            .Take(take) as IOrderedQueryable<T>
                       : GetAll()
                            .OrderByDescending(t => t.Id)
                            .Skip(skip)
                            .Take(take) as IOrderedQueryable<T>;
        }

        public virtual IOrderedQueryable<T> GetAll<TKey>(Expression<Func<T, TKey>> order, bool ascending, int skip, int take)
        {
            return @ascending
                       ? GetAll()
                            .OrderBy(order)
                            .ThenBy(t => t.Id)
                            .Skip(skip)
                            .Take(take) as IOrderedQueryable<T>
                       : GetAll()
                            .OrderByDescending(order)
                            .ThenByDescending(t => t.Id)
                            .Skip(skip)
                            .Take(take) as IOrderedQueryable<T>;
        }

        public virtual IOrderedQueryable<T> GetAll(string order, bool ascending, int skip, int take)
        {
            return @ascending
                       ? GetAll()
                            .OrderBy(order)
                            .ThenBy(t => t.Id)
                            .Skip(skip)
                            .Take(take) as IOrderedQueryable<T>
                       : GetAll()
                            .OrderBy(order + " desc")
                            .ThenByDescending(t => t.Id)
                            .Skip(skip)
                            .Take(take) as IOrderedQueryable<T>;
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public virtual T GetFirst(Expression<Func<T, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public virtual T Get(TId id) 
        {     
            var param = Expression.Parameter(typeof(T));
            var lambda = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    Expression.Property(param, "Id"), 
                    Expression.Constant(id)), 
                param);     
            return GetAll().Single(lambda); 
        }

        public virtual T GetFirst<TKey>(Expression<Func<T, TKey>> order, bool ascending = true)
        {
            return @ascending 
                ? GetAll()
                    .OrderBy(order)
                    .FirstOrDefault() 
                : GetAll()
                    .OrderByDescending(order)
                    .FirstOrDefault();
        }

        public virtual T GetFirst<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool ascending = true)
        {
            return @ascending 
                ? GetAll()
                    .OrderBy(order)
                    .FirstOrDefault(predicate) 
                : GetAll()
                    .OrderByDescending(order)
                    .FirstOrDefault(predicate);
        }

        public virtual void Add(T entity)
        {
            Context.Entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Entities.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            Context.Entities.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public virtual int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public virtual IOrderedQueryable<T> FindBy(Expression<Func<T, bool>> predicate, bool ascending, int skip, int take)
        {
            return @ascending
                       ? GetAll()
                            .Where(predicate)
                            .OrderBy(t => t.Id)
                            .Skip(skip)
                            .Take(take) as IOrderedQueryable<T>
                       : GetAll()
                            .Where(predicate)
                            .OrderByDescending(t => t.Id)
                            .Skip(skip)
                            .Take(take) as IOrderedQueryable<T>;
        }

        public virtual IOrderedQueryable<T> FindBy<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool ascending, int skip, int take)
        {
            return @ascending
                       ? GetAll()
                             .Where(predicate)
                             .OrderBy(order)
                             .ThenBy(t => t.Id)
                             .Skip(skip)
                             .Take(take) as IOrderedQueryable<T>
                       : GetAll()
                             .Where(predicate)
                             .OrderByDescending(order)
                             .ThenByDescending(t => t.Id)
                             .Skip(skip)
                             .Take(take) as IOrderedQueryable<T>;
        }
    }
}
