using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace beta.DataAccess.Entities
{
    public partial class NumDBContext : DbContext
    {
        //(DbContextOptions<RestaurantReviewsDbContext> options) : base(options)
        public NumDBContext(DbContextOptions<NumDBContext> options) : base(options)
        { }

        public virtual DbSet<Number> Number { get; set; }

        //builds each table (?)
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        //define each table entries
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity < Number> ()
                  .Property(n => n.Id)
                  .ValueGeneratedOnAdd();//id is generated automatically

            modelBuilder.Entity<Number>()
                   .Property(n => n.IntNum);//add a number to the database
                    

        }


    }
}