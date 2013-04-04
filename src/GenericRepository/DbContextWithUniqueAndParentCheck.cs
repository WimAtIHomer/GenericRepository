using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Data.Objects;
using IHomer.GenericRepository.EntityValidators;

namespace IHomer.GenericRepository
{
    public class DbContextWithUniqueAndParentCheck : DbContext
    {
        public DbContextWithUniqueAndParentCheck() : base()
        {
        }

        public DbContextWithUniqueAndParentCheck(DbCompiledModel model) : base(model)
        {
        }

        public DbContextWithUniqueAndParentCheck(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public DbContextWithUniqueAndParentCheck(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public DbContextWithUniqueAndParentCheck(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        public DbContextWithUniqueAndParentCheck(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DbContextWithUniqueAndParentCheck(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var errors = new List<DbValidationError>();
            Type type = ObjectContext.GetObjectType(entityEntry.Entity.GetType());

            ParentValidator.ValidateEntity(this, entityEntry, type);
            errors.AddRange(UniqueValidator.ValidateEntity(this, entityEntry, type));

            var myItems = new Dictionary<object, object> {{"Context", this}};
            errors.AddRange(base.ValidateEntity(entityEntry, myItems).ValidationErrors);

            return new DbEntityValidationResult(entityEntry, errors);
        }

        //public override int SaveChanges()
        //{
        //    var modified = ChangeTracker.Entries().Where(e => e.State == System.Data.EntityState.Modified);
        //    foreach (var entity in modified)
        //    {
        //        ParentValidator.ValidateEntity(this, entity, ObjectContext.GetObjectType(entity.Entity.GetType()));
        //    }
        //    return base.SaveChanges();
        //}
     }
}
