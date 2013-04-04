using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace IHomer.GenericRepository.Tests.Models.Mapping
{
    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Body)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Comments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Created).HasColumnName("Created");
            this.Property(t => t.Body).HasColumnName("Body");

            // Relationships
            this.HasRequired(t => t.Post)
                .WithMany(t => t.Comments)
                .HasForeignKey(d => d.PostId).WillCascadeOnDelete();
            this.HasRequired(t => t.User)
                .WithMany(t => t.Comments)
                .HasForeignKey(d => d.UserId).WillCascadeOnDelete(false);

        }
    }
}
