using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServerReWear.Models;

public partial class ShiriDBContext : DbContext
{
    public ShiriDBContext()
    {
    }

    public ShiriDBContext(DbContextOptions<ShiriDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<OrdersFrom> OrdersFroms { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=ReWear_DB;User ID=AdminUser;Password=admin123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B7AE3F7A6E");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.Carts).HasConstraintName("FK__Cart__ProductCod__32E0915F");

            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("FK__Cart__UserId__31EC6D26");
        });

        modelBuilder.Entity<OrdersFrom>(entity =>
        {
            entity.HasOne(d => d.ProductCodeNavigation).WithMany().HasConstraintName("FK__OrdersFro__Produ__398D8EEE");

            entity.HasOne(d => d.User).WithMany().HasConstraintName("FK__OrdersFro__UserI__38996AB5");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductCode).HasName("PK__Products__2F4E024E7DACC2A2");

            entity.HasOne(d => d.Status).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Status__2E1BDC42");

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__TypeId__2F10007B");

            entity.HasOne(d => d.User).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__UserId__2D27B809");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusCode).HasName("PK__Status__6A7B44FDFC298240");

            entity.Property(e => e.StatusCode).ValueGeneratedNever();
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.TypeCode).HasName("PK__Types__3E1CDC7D879E9D55");

            entity.Property(e => e.TypeCode).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C4E858C7E");
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("PK__WishList__233189EB4202B38D");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.WishLists).HasConstraintName("FK__WishList__Produc__36B12243");

            entity.HasOne(d => d.User).WithMany(p => p.WishLists).HasConstraintName("FK__WishList__UserId__35BCFE0A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
