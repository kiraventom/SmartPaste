using System.ComponentModel.DataAnnotations;
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
    public int ExpiresHours { get; set; }
}
