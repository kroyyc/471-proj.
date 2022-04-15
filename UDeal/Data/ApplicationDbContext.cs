﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UDeal.Models;

namespace UDeal.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public DbSet<User> Users { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        //public DbSet<Selling> SellingPosts { get; set; }
        //public DbSet<Looking> LookingPosts { get; set;}
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Favourite> UserFavourites { get; set; } 
        public DbSet<Image> Images { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(b => b.ToTable("Users"));
            modelBuilder.Entity<IdentityRole>(b => b.ToTable("Roles"));
            modelBuilder.Entity<IdentityUserRole<string>>(b => b.ToTable("UserRoles"));

            modelBuilder.Entity<Favourite>().HasKey(o => new { o.UserId, o.PostId });
            modelBuilder.Entity<Post>()
                .Property(p => p.Created)
                .HasDefaultValueSql("datetime('now','localtime')");

            modelBuilder.Entity<Post>()
                .HasOne<Campus>(p => p.Campus)
                .WithMany(c => c.Posts)
                .HasForeignKey(c => c.CampusId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Seed();
        }
    }
}
