using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using IHomer.GenericRepository.Attributes;
using System.Data.Entity;

namespace IHomer.GenericRepository.EntityValidators
{
    public class ParentValidator
    {
        private static readonly Dictionary<Type, string> _parentAttributes = new Dictionary<Type, string>();

        public static void ValidateEntity(DbContext context, DbEntityEntry entity, Type type)
        {
            if (entity.State == System.Data.EntityState.Modified)
            {
                if (!_parentAttributes.ContainsKey(type))
                {
                    var properties = from attributedProperty in type.GetProperties()
                                     select new
                                     {
                                         attributedProperty,
                                         attributes = attributedProperty.GetCustomAttributes(true)
                                             .Where(attribute => attribute is ParentAttribute)
                                     };
                    properties = properties.Where(p => p.attributes.Any());
                    _parentAttributes.Add(type,
                                          properties.Any()
                                              ? properties.First().attributedProperty.Name
                                              : string.Empty);
                }

                if (!string.IsNullOrEmpty(_parentAttributes[type]))
                {
                    if (entity.Reference(_parentAttributes[type]).CurrentValue == null)
                    {
                        context.Set(type).Remove(entity.Entity);
                    }
                }
            }
        }
    }
}
