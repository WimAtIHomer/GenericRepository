using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IHomer.GenericRepository
{
    public static class OrderExpression
    {
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByValues) where TEntity : class 
        {
            var orderPair = orderByValues.Trim().Split(',')[0]; 
            var command = orderPair.ToUpper().Contains("DESC") ? "OrderByDescending" : "OrderBy";

            var parameter = Expression.Parameter(typeof(TEntity), "p"); 
 
            var propertyName = (orderPair.Split(' ')[0]).Trim(); 
 
            PropertyInfo property; 
            MemberExpression propertyAccess; 
 
            if (propertyName.Contains('.')) 
            { 
                // support to be sorted on child fields.  
                var childProperties = propertyName.Split('.'); 
                property = typeof(TEntity).GetProperty(childProperties[0]); 
                propertyAccess = Expression.MakeMemberAccess(parameter, property); 
 
                for (var i = 1; i < childProperties.Length; i++) 
                { 
                    var t = property.PropertyType; 
                    property = t.IsGenericType ? 
                                    t.GetGenericArguments().First().GetProperty(childProperties[i]) : 
                                    t.GetProperty(childProperties[i]); 
 
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property); 
                } 
            } 
            else 
            {
                property = typeof(TEntity).GetProperty(propertyName) ??
                           typeof(TEntity).GetProperty("Id");
                propertyAccess = Expression.MakeMemberAccess(parameter, property); 
            } 
 
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { typeof(TEntity), property.PropertyType }, 
                                        source.Expression, Expression.Quote(orderByExpression)); 
 
            IQueryable returnValue = source.Provider.CreateQuery<TEntity>(resultExpression); 
 
            if (orderByValues.Trim().Split(',').Count() > 1) 
            { 
                // remove first item 
                var newSearchForWords = orderByValues.Remove(0, orderByValues.IndexOf(',') + 1); 
                returnValue = source.OrderBy(newSearchForWords); 
            } 
 
            return (IOrderedQueryable<TEntity>)returnValue; 
 
        } 

    }
}
