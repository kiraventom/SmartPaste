using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartPaste.Model;

public class MainContext : DbContext
{
    public DbSet<Paste> Pastes { get; set; }

    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies(false);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Paste>()
            .HasIndex(p => p.Link)
            .IsUnique();
    }
}

public class Paste
{
    [Key]
    public int PasteId { get; set; }

    public string Link { get; set; }

    public string Text { get; set; }

    public DateTime Created { get; set; }
    public int ExpiresMin { get; set; }

    public string PasswordHash { get; set; }

    [NotMapped]
    public bool Protected => PasswordHash is not null;

    [NotMapped]
    public bool OneShot => ExpiresMin == 0;

    [NotMapped]
    public bool NeverDelete => ExpiresMin < 0;
}

public enum Expiration
{
    OnceRead = 0,
    TenMinutes = 10,
    Hour = 60,
    SixHours = Hour * 6,
    Day = Hour * 24,
    Week = Day * 7,
    Never = -1
}
