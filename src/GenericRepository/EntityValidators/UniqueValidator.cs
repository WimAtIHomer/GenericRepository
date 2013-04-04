using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using IHomer.GenericRepository.Attributes;
using System.Data;
using System.Linq.Expressions;

namespace IHomer.GenericRepository.EntityValidators
{
    public class UniqueValidator
    {
        private static readonly Dictionary<Type, string[]> _uniqueAttributes = new Dictionary<Type, string[]>();
        private static readonly Dictionary<Type, Func<DbContext, DbEntityEntry, Type, List<DbValidationError>>> _uniqueMethodDelegates = new Dictionary<Type, Func<DbContext, DbEntityEntry, Type, List<DbValidationError>>>();

        public static List<DbValidationError> ValidateEntity(DbContext context, DbEntityEntry entity, Type type)
        {
            if (!_uniqueMethodDelegates.ContainsKey(type))
            {
                var validateUnique = typeof(UniqueValidator).GetMethod("ValidateUnique", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).MakeGenericMethod(type);
                _uniqueMethodDelegates.Add(type, (Func<DbContext, DbEntityEntry, Type, List<DbValidationError>>)Delegate.CreateDelegate(typeof(Func<DbContext, DbEntityEntry, Type, List<DbValidationError>>), validateUnique));
            }
            return _uniqueMethodDelegates[type](context, entity, type);
        }

        private static List<DbValidationError> ValidateUnique<T>(DbContext context, DbEntityEntry entity, Type type) where T : class
        {
            var errors = new List<DbValidationError>();
            if (!_uniqueAttributes.ContainsKey(type))
            {
                var properties = from attributedProperty in type.GetProperties()
                                 select new
                                 {
                                     attributedProperty,
                                     attributes = attributedProperty.GetCustomAttributes(true)
                                         .Where(attribute => attribute is UniqueAttribute)
                                 };
                properties = properties.Where(p => p.attributes.Any());
                _uniqueAttributes.Add(type,
                                      properties.Select(a => a.attributedProperty.Name).ToArray());
            }

            if (_uniqueAttributes[type].Any())
            {
                var changed = false;
                if (entity.State == EntityState.Added)
                {
                    changed = true;
                }
                else if (entity.State == EntityState.Modified)
                {
                    foreach (var uniqueAttribute in _uniqueAttributes[type])
                    {
                        if (entity.CurrentValues[uniqueAttribute] != entity.OriginalValues[uniqueAttribute])
                        {
                            changed = true;
                        }
                    }
                }

                if (changed)
                {
                    var prm = Expression.Parameter(type, "p");
                    Expression uniqueExp = null;
                    foreach (var uniqueProp in _uniqueAttributes[type])
                    {
                        var prop = type.GetProperty(uniqueProp);
                        var eq = Expression.Equal
                            (
                                Expression.MakeMemberAccess(prm, prop),
                                Expression.Constant(entity.CurrentValues[uniqueProp], prop.PropertyType)
                            );
                        uniqueExp = uniqueExp == null ? eq : Expression.AndAlso(uniqueExp, eq);
                    }

                    var propId = type.GetProperty("Id");
                    Expression<Func<T, bool>> exp =
                        Expression.Lambda<Func<T, bool>>
                            (
                                Expression.AndAlso(
                                    uniqueExp,
                                    Expression.NotEqual
                                        (
                                            Expression.MakeMemberAccess(prm, propId),
                                            Expression.Constant(entity.CurrentValues["Id"], propId.PropertyType)
                                        )
                                    ),
                                prm
                            );

                    if (context.Set<T>().Any(exp))
                    {
                        errors.AddRange(_uniqueAttributes[type].Select(uniqueProp => new DbValidationError(uniqueProp, "NotUnique")));
                    }
                }
            }
            return errors;
        }
    }
}
