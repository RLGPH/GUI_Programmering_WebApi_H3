using Microsoft.EntityFrameworkCore;

namespace GUI_Programmering_WebApi.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Image entity
            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.Property(e => e.ImageName)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.ImageUrl)    // <-- new URL property
                      .HasMaxLength(500);
            });

            // Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryName)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.ProductDescription)
                      .HasMaxLength(200);

                entity.Property(e => e.ProductPrice)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.Category)
                      .WithMany(p => p.Products)
                      .HasForeignKey(d => d.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Image)
                      .WithMany(p => p.Products)
                      .HasForeignKey(d => d.ImageId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
