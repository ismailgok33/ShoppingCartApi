using CicekSepetiTask.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Item>()
        //        .HasOne(e => e.Position)
        //        .WithMany()
        //        .HasForeignKey(e => e.PositionId);
        //}
    }

    //public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    //{
    //    public DataContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
    //        optionsBuilder.UseSqlServer("DefaultConntection");

    //        return new DataContext(optionsBuilder.Options);
    //    }
    //}
}
