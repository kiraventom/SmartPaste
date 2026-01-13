using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SmartPaste.Model;
using Serilog;
using Serilog.Events;
using Serilog.Core;
using SmartPaste;

public partial class Program
{
    public const string PROJECT_NAME = "SmartPaste";

    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var projectDataDir = CreateProjectDataDir();

        var logger = CreateLogger(projectDataDir);

        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<MainContext>(o =>
        {
            var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
            var csb = new SqliteConnectionStringBuilder(connStr);
            var path = csb.DataSource;
            var fullPath = Path.Combine(projectDataDir, path);
            csb.DataSource = fullPath;
            o.UseSqlite(csb.ConnectionString);
        });

        builder.Services.AddHostedService<ExpirationService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            await using var scope = app.Services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<MainContext>();
            await db.Database.MigrateAsync();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseStatusCodePagesWithReExecute("/Errors/{0}");

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }

    private static Logger CreateLogger(string projectDataDir)
    {
        var logsDir = Path.Combine(projectDataDir, "logs");
        Directory.CreateDirectory(logsDir);
        var logFile = Path.Combine(logsDir, $"{PROJECT_NAME}.log");

        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(logFile, rollingInterval: RollingInterval.Day)
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        return logger;
    }

    private static string GetProjectConfigDir()
    {
        if (OperatingSystem.IsWindows())
        {
            var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataDirPath, PROJECT_NAME, "config");
        }

        var homeDirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(homeDirPath, ".config", PROJECT_NAME);
    }

    private static string CreateProjectDataDir()
    {
        string path;
        if (OperatingSystem.IsWindows())
        {
            var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(appDataDirPath, PROJECT_NAME, "data");
        }
        else
        {
            var homeDirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            path = Path.Combine(homeDirPath, ".local", "share", PROJECT_NAME);
        }

        Directory.CreateDirectory(path);
        return path;
    }
}
