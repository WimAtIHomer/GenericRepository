using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace IHomer.GenericRepository.Tests.Models.Mapping
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Body)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Posts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Created).HasColumnName("Created");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Body).HasColumnName("Body");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Posts)
                .HasForeignKey(d => d.UserId).WillCascadeOnDelete(true);

        }
    }
}
