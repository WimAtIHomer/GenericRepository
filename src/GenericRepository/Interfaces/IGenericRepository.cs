using System;
using System.Linq;
using System.Linq.Expressions;

namespace IHomer.GenericRepository.Interfaces
{
    public interface IGenericRepository<T, in TId> : ISaveChanges
        where T : class, IEntity<TId> 
        where TId : IComparable<TId>
    {
        IQueryable<T> GetAll();
        IOrderedQueryable<T> GetAll(bool ascending, int skip, int take);
        IOrderedQueryable<T> GetAll<TKey>(Expression<Func<T, TKey>> order, bool ascending, int skip, int take);
        IOrderedQueryable<T> GetAll(string order, bool ascending, int skip, int take);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T GetFirst(Expression<Func<T, bool>> predicate);
        T GetFirst<TKey>(Expression<Func<T, TKey>> order, bool ascending = true);
        T GetFirst<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool ascending = true);
        T Get(TId id);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        IOrderedQueryable<T> FindBy(Expression<Func<T, bool>> predicate, bool ascending, int skip, int take);
        IOrderedQueryable<T> FindBy<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order, bool ascending, int skip, int take);
    }
}