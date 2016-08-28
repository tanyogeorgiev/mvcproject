namespace PizzaHub.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PizzaDbContext : DbContext
    {
        public PizzaDbContext()
            : base("name=PizzaDbContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Pizza> Pizzas { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Comments)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.AuthorID);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Pizzas)
                .WithOptional(e => e.AspNetUser)
                .HasForeignKey(e => e.Author);

            modelBuilder.Entity<Pizza>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Pizzas)
                .Map(m => m.ToTable("PizzaProducts").MapLeftKey("PizzaID").MapRightKey("ProductID"));

            modelBuilder.Entity<ProductGroup>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.ProductGroup)
                .HasForeignKey(e => e.GroupID)
                .WillCascadeOnDelete(false);
        }
    }
}
