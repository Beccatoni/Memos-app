
using MemosApplication.Api.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<Memo> Memos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Memo>().HasData(
            new Memo
            {
                Id = 1, Title = "Car Free Day", 
                Content = "Go for a ride on the highway", 
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 6, 1), DateTimeKind.Utc), 
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2025, 6, 1), DateTimeKind.Utc)
            },
            new Memo
            {
                Id = 2, Title = "Shopping",
                Content = "Buy some groceries",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 6, 1), DateTimeKind.Utc), 
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2025, 6, 1), DateTimeKind.Utc)
            }
        );
        modelBuilder.Entity<WeatherForecast>()
            .Property(weatherforecast => weatherforecast.Date)
            .HasColumnType("date");
    }
}