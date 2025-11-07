using Microsoft.EntityFrameworkCore;

namespace GeometryApi.Models;

public class GeometryContext : DbContext
{
    public GeometryContext(DbContextOptions<GeometryContext> options) : base(options) { Database.EnsureCreated(); }
    public DbSet<Figure> Figures { get; set; } = null!;
    public DbSet<Rectangle> Rectangles { get; set; } = null!;
    public DbSet<Circle> Circles { get; set; } = null!;
    public DbSet<Triangle> Triangles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Figure>()
            .HasDiscriminator<string>("Type")
            .HasValue<Rectangle>("Rectangle")
            .HasValue<Circle>("Circle")
            .HasValue<Triangle>("Triangle");

        modelBuilder.Entity<Rectangle>().OwnsOne(r => r.LeftUpper);

        modelBuilder.Entity<Rectangle>().OwnsOne(r => r.RightBottom);

        modelBuilder.Entity<Circle>().OwnsOne(c => c.Center);

        modelBuilder.Entity<Triangle>().OwnsOne(t => t.LeftBottom);

        modelBuilder.Entity<Triangle>().OwnsOne(t => t.RightBottom);

        modelBuilder.Entity<Triangle>().OwnsOne(t => t.Upper);
    }
}
