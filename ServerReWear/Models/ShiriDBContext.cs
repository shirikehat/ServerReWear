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

    public virtual DbSet<ProinWish> ProinWishes { get; set; }

    public virtual DbSet<ProintCart> ProintCarts { get; set; }

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
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B7EEFABD4B");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.Carts).HasConstraintName("FK__Cart__ProductCod__30F848ED");

            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("FK__Cart__UserId__300424B4");
        });

        modelBuilder.Entity<OrdersFrom>(entity =>
        {
            entity.HasOne(d => d.ProductCodeNavigation).WithMany().HasConstraintName("FK__OrdersFro__Produ__37A5467C");

            entity.HasOne(d => d.User).WithMany().HasConstraintName("FK__OrdersFro__UserI__36B12243");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductCode).HasName("PK__Products__2F4E024E054216B8");

            entity.HasOne(d => d.Status).WithMany(p => p.Products).HasConstraintName("FK__Products__Status__2C3393D0");

            entity.HasOne(d => d.Type).WithMany(p => p.Products).HasConstraintName("FK__Products__TypeId__2D27B809");

            entity.HasOne(d => d.User).WithMany(p => p.Products).HasConstraintName("FK__Products__UserId__2B3F6F97");
        });

        modelBuilder.Entity<ProinWish>(entity =>
        {
            entity.HasOne(d => d.ProductCodeNavigation).WithMany().HasConstraintName("FK__ProinWish__Produ__3C69FB99");

            entity.HasOne(d => d.Wishlist).WithMany().HasConstraintName("FK__ProinWish__Wishl__3D5E1FD2");
        });

        modelBuilder.Entity<ProintCart>(entity =>
        {
            entity.HasOne(d => d.Cart).WithMany().HasConstraintName("FK__ProintCar__CartI__3A81B327");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany().HasConstraintName("FK__ProintCar__Produ__398D8EEE");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusCode).HasName("PK__Status__6A7B44FD91B2FC89");

            entity.Property(e => e.StatusCode).ValueGeneratedNever();
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.TypeCode).HasName("PK__Types__3E1CDC7D4F4BF7FA");

            entity.Property(e => e.TypeCode).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CCD2994CC");
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("PK__WishList__233189EB85CBBA80");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.WishLists).HasConstraintName("FK__WishList__Produc__34C8D9D1");

            entity.HasOne(d => d.User).WithMany(p => p.WishLists).HasConstraintName("FK__WishList__UserId__33D4B598");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
