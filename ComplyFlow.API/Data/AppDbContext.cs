using Microsoft.EntityFrameworkCore;
using ComplyFlow.API.Models;
using System;

namespace ComplyFlow.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<SubTask> SubTasks => Set<SubTask>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Groups
            modelBuilder.Entity<Group>().HasData(
                new Group { Id = 1, Name = "Uyum Ekibi" },
                new Group { Id = 2, Name = "KVKK Kurulu" },
                new Group { Id = 3, Name = "Hukuk Departmanı" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "Ahmet Yılmaz", Role = "Avukat" },
                new User { Id = 2, FullName = "Ayşe Demir", Role = "Uyum Uzmanı" },
                new User { Id = 3, FullName = "Mehmet Kaya", Role = "Stajyer" },
                new User { Id = 4, FullName = "Zeynep Çelik", Role = "Yönetici" }
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
                    DueDate = DateTime.Now.AddDays(3), 
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
                    DueDate = DateTime.Now.AddDays(7), 
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
                    DueDate = DateTime.Now.AddDays(-2), 
                    AssignedToGroupId = 1
                }
            );
        }
    }
}
