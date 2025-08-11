using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Web_Book_BE.Models;

public partial class BookStoreDbContext : DbContext
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Authors> Authors { get; set; }

    public virtual DbSet<CartItems> CartItems { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Customers> Customers { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<OrderDetails> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetails> ProductDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Authors>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__55B9F6DF09EA41E5");

            entity.Property(e => e.AuthorId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Author_ID");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<CartItems>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__7B65152127340B2A");

            entity.Property(e => e.CartItemId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CartItem_ID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Product_ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("User_ID");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_CartItems_Products");

            entity.HasOne(d => d.User).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_CartItems_User");
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.CategoriesId).HasName("PK__Categori__EE560CFD38E95A3B");

            entity.Property(e => e.CategoriesId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Categories_ID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.CategoriesName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customers>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__8CB286B9C84E6BDE");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Customer_ID");
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.OrdersId).HasName("PK__Orders__B2D3006C29DBE337");

            entity.Property(e => e.OrdersId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Orders_ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ShippingAddress).HasColumnType("text");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Orders_User");
        });

        modelBuilder.Entity<OrderDetails>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__53D880E0AA1BBADA");

            entity.Property(e => e.OrderDetailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OrderDetail_ID");
            entity.Property(e => e.OrdersId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Orders_ID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Product_ID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Orders).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrdersId)
                .HasConstraintName("FK_OrderDetails_Orders");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__DA6C7FE13D0BF816");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Payment_ID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OrdersId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Orders_ID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Orders).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrdersId)
                .HasConstraintName("FK_Payment_Orders");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.HasKey(e => e.PaymentHistoryId).HasName("PK__Payment___121FFA6C64FEDE42");

            entity.ToTable("Payment_History");

            entity.Property(e => e.PaymentHistoryId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Payment_History_ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Payment_ID");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_Payment_History_Payment");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__9834FB9AD6753872");

            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Product_ID");
            entity.Property(e => e.AuthorId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Author_ID");
            entity.Property(e => e.CategoriesId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Categories_ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Discount)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Author).WithMany(p => p.Products)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Products_Authors");

            entity.HasOne(d => d.Categories).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoriesId)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<ProductDetails>(entity =>
        {
            entity.HasKey(e => e.ProductDetailId).HasName("PK__ProductD__3D49BA566A2E1429");

            entity.Property(e => e.ProductDetailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProductDetail_ID");
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Product_ID");
            entity.Property(e => e.Publisher)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductDetails_Products");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__206D9190D9B8F22C");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E41854DD00").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("User_ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Customer_ID");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("customers");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Users)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_User_Customers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
