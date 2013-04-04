using System.Data.Entity;
using IHomer.GenericRepository.Tests.Models.Mapping;

namespace IHomer.GenericRepository.Tests.Models
{
    public class GenericRepositoryTestContext : DbContextWithUniqueAndParentCheck
    {
        static GenericRepositoryTestContext()
        {
            Database.SetInitializer(new GenericRepositoryTestContextInitializer());
        }

        public GenericRepositoryTestContext()
            : base("Name=GenericRepositoryTestContext")
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
