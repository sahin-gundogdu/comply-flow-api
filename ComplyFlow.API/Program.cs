using ComplyFlow.API.Data;
using ComplyFlow.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ComplyFlow.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://localhost:5001");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MSSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJsFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Configure Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("GlobalLimit", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "DefaultFallbackSecretKeyIfNotFound123!@#";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// In Development, enable Swagger & Swagger UI.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register Global Exception Handler
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowNextJsFrontend");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure the database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();

    // Manuel Seed (Eğer veritabanı önceden var ama boşsa, EF Core OnModelCreating seed'i çalıştırmaz)
    if (!dbContext.Groups.Any())
    {
        dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Groups ON");
        dbContext.Groups.AddRange(
            new Group { Id = 1, Name = "Uyum Ekibi" },
            new Group { Id = 2, Name = "KVKK Kurulu" },
            new Group { Id = 3, Name = "Hukuk Departmanı" }
        );
        dbContext.SaveChanges();
        dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Groups OFF");
    }

    if (!dbContext.Users.Any())
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
        dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users ON");
        dbContext.Users.AddRange(
            new User { Id = 1, Username = "ahmet", FullName = "Ahmet Yılmaz", Role = "Avukat", PasswordHash = passwordHash },
            new User { Id = 2, Username = "ayse", FullName = "Ayşe Demir", Role = "Uyum Uzmanı", PasswordHash = passwordHash },
            new User { Id = 3, Username = "mehmet", FullName = "Mehmet Kaya", Role = "Stajyer", PasswordHash = passwordHash },
            new User { Id = 4, Username = "zeynep", FullName = "Zeynep Çelik", Role = "Yönetici", PasswordHash = passwordHash }
        );
        dbContext.SaveChanges();
        dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users OFF");
    }

    if (!dbContext.TaskItems.Any())
    {
        dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT TaskItems ON");
        dbContext.TaskItems.AddRange(
            new TaskItem { Id = 1, Title = "Sözleşme İncelemesi", Description = "Yeni tedarikçi sözleşmesinin incelenmesi.", Status = "Beklemede", Priority = "Yüksek", TaskType = "Inceleme", DueDate = DateTime.Now.AddDays(3), AssignedToUserId = 1, AssignedToGroupId = 3 },
            new TaskItem { Id = 2, Title = "KVKK Aydınlatma Metni Güncellemesi", Description = "Web sitesindeki aydınlatma metninin güncellenmesi.", Status = "Devam Ediyor", Priority = "Orta", TaskType = "Guncelleme", DueDate = DateTime.Now.AddDays(7), AssignedToUserId = 2, AssignedToGroupId = 2 },
            new TaskItem { Id = 3, Title = "Uyum Eğitimi Hazırlığı", Description = "Personel için yıllık uyum eğitimi sunumunun hazırlanması.", Status = "Tamamlandı", Priority = "Düşük", TaskType = "Egitim", DueDate = DateTime.Now.AddDays(-2), AssignedToGroupId = 1 }
        );
        dbContext.SaveChanges();
        dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT TaskItems OFF");
    }
}

app.Run();
