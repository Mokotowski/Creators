using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Followers>()
                .HasKey(f => new { f.Id_Creator, f.Id_User });

            modelBuilder.Entity<Followers>()
                .HasOne<UserModel>()
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.Id_User);

            modelBuilder.Entity<CreatorPage>()
                .HasOne<UserModel>()
                .WithOne(u => u.CreatorPage)
                .HasForeignKey<CreatorPage>(s => s.Id_Creator);

            modelBuilder.Entity<PageData>()
                .HasOne<CreatorPage>()
                .WithOne(u => u.PageData)
                .HasForeignKey<PageData>(s => s.Id_Creator);

            modelBuilder.Entity<Donates>()
                .HasOne<CreatorPage>()
                .WithMany(u => u.Donates)
                .HasForeignKey(f => f.Id_Donates);

            modelBuilder.Entity<CalendarEvents>()
                .HasOne<CreatorPage>()
                .WithMany(u => u.CalendarEvents)
                .HasForeignKey(f => f.Id_Calendar);


            modelBuilder.Entity<CreatorPhoto>()
                .HasOne<CreatorPage>()
                .WithMany(u => u.CreatorPhotos)
                .HasForeignKey(f => f.Id_Photos);


            modelBuilder.Entity<PhotoHearts>()
                .HasOne<CreatorPhoto>()
                .WithMany(u => u.PhotoHearts)
                .HasForeignKey(f => f.HeartGroup)
                .HasPrincipalKey(u => u.HeartGroup);

            modelBuilder.Entity<PhotoComments>()
                .HasOne<CreatorPhoto>()
                .WithMany(u => u.PhotoComments)
                .HasForeignKey(f => f.CommentsGroup)
                .HasPrincipalKey(u => u.CommentsGroup);



            base.OnModelCreating(modelBuilder);
        }
    }
}
  