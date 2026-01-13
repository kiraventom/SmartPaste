using SmartPaste.Model;

namespace SmartPaste;

public class ExpirationService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private const int Minutes = 1;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(Minutes));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<MainContext>();

            var now = DateTime.UtcNow;
            var pastes = db.Pastes.AsEnumerable().Where(p => !p.NeverDelete && !p.OneShot).ToList();
            foreach (var paste in pastes)
            {
                var dt = DateTime.SpecifyKind(paste.Created, DateTimeKind.Utc);
                var expirationDT = dt.AddMinutes(paste.ExpiresMin);
                if (expirationDT < now)
                {
                    db.Remove(paste);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
