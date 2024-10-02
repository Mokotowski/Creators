using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace Creators.Creators.Database
{
    public class DatabaseContext : IdentityDbContext<UserModel>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CreatorPage> CreatorPage { get; set; }
        public DbSet<PageData> PageData { get; set; }
        public DbSet<CalendarEvents> CalendarEvents { get; set; }
        public DbSet<Donates> Donates { get; set; }
        public DbSet<CreatorPhoto> CreatorPhoto { get; set; }
        public DbSet<PhotoComments> PhotoComments { get; set; }
        public DbSet<PhotoHearts> PhotoHearts { get; set; }
        public DbSet<Followers> Followers { get; set; }
        public DbSet<CreatorBalance> CreatorBalance { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Followers>()
                .HasOne<UserModel>()
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.Id_User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorPage>()
                .HasOne(d => d.User)
                .WithMany(c => c.Creators)
                .HasForeignKey(d => d.Id_Creator);

            modelBuilder.Entity<CreatorPage>()
                .HasOne(s => s.PageData)
                .WithOne(c => c.CreatorPage)
                .HasForeignKey<CreatorPage>(s => s.Id_Creator);

            modelBuilder.Entity<Donates>()
                .HasOne(d => d.CreatorPage)
                .WithMany(c => c.Donates)
                .HasForeignKey(d => d.Id_Donates);

            modelBuilder.Entity<CalendarEvents>()
                .HasOne(d => d.CreatorPage)
                .WithMany(c => c.CalendarEvents)
                .HasForeignKey(d => d.Id_Calendar);

            modelBuilder.Entity<CreatorPhoto>()
                .HasOne(d => d.CreatorPage)
                .WithMany(c => c.CreatorPhotos)
                .HasForeignKey(d => d.Id_Photos);


            modelBuilder.Entity<CreatorPhoto>()
                .HasOne(d => d.CreatorPage)
                .WithMany(c => c.CreatorPhotos)
                .HasForeignKey(d => d.Id_Photos);

            modelBuilder.Entity<PhotoHearts>()
                .HasOne(d => d.CreatorPhoto)
                .WithMany(c => c.Hearts)
                .HasForeignKey(d => d.HeartGroup)
                .HasPrincipalKey(c => c.HeartGroup);


            modelBuilder.Entity<PhotoComments>()
                .HasOne(d => d.CreatorPhoto)
                .WithMany(c => c.Comments)
                .HasForeignKey(d => d.CommentsGroup)
                .HasPrincipalKey(c => c.CommentsGroup);

            modelBuilder.Entity<CreatorPage>()
               .HasOne(c => c.CreatorBalance)
               .WithOne(d => d.CreatorPage)
               .HasForeignKey<CreatorBalance>(d => d.Id_Donates);



            base.OnModelCreating(modelBuilder);
        }
    }
}
  