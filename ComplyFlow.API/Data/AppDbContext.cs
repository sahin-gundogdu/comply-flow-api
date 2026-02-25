using Microsoft.EntityFrameworkCore;
using ComplyFlow.API.Models;
using System;

namespace ComplyFlow.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<SubTask> SubTasks => Set<SubTask>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            // Seed Groups
            modelBuilder.Entity<Group>().HasData(
                new Group { Id = 1, Name = "Uyum Ekibi" },
                new Group { Id = 2, Name = "KVKK Kurulu" },
                new Group { Id = 3, Name = "Hukuk Departmanı" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "ahmet.yilmaz", FullName = "Ahmet Yılmaz", Role = "Avukat", PasswordHash = "" },
                new User { Id = 2, Username = "ayse.demir", FullName = "Ayşe Demir", Role = "Uyum Uzmanı", PasswordHash = "" },
                new User { Id = 3, Username = "mehmet.kaya", FullName = "Mehmet Kaya", Role = "Stajyer", PasswordHash = "" },
                new User { Id = 4, Username = "zeynep.celik", FullName = "Zeynep Çelik", Role = "Yönetici", PasswordHash = "" },
                new User { Id = 99, Username = "administrator", FullName = "Master Admin", Role = "Admin", PasswordHash = "$2a$11$JUUUdxfnetLG.uui3oh6/uSO14gjvzGp2Mv2Y8bLKg6fRD7Ll7UDC" }
            );

            // Seed Tasks
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem 
                { 
                    Id = 1, 
                    Title = "Sözleşme İncelemesi", 
                    Description = "Yeni tedarikçi sözleşmesinin incelenmesi.", 
                    Status = "Beklemede", 
                    Priority = "Yüksek", 
                    DueDate = new DateTime(2024, 1, 4), 
                    AssignedToUserId = 1,
                    AssignedToGroupId = 3
                },
                new TaskItem 
                { 
                    Id = 2, 
                    Title = "KVKK Aydınlatma Metni Güncellemesi", 
                    Description = "Web sitesindeki aydınlatma metninin güncellenmesi.", 
                    Status = "Devam Ediyor", 
                    Priority = "Orta", 
                    DueDate = new DateTime(2024, 1, 8), 
                    AssignedToUserId = 2,
                    AssignedToGroupId = 2
                },
                new TaskItem 
                { 
                    Id = 3, 
                    Title = "Uyum Eğitimi Hazırlığı", 
                    Description = "Personel için yıllık uyum eğitimi sunumunun hazırlanması.", 
                    Status = "Tamamlandı", 
                    Priority = "Düşük", 
                    DueDate = new DateTime(2024, 1, 1).AddDays(-2), 
                    AssignedToGroupId = 1
                }
            );
        }
    }
}
