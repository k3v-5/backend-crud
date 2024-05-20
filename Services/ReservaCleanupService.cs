using backend_crud.Context;

public class ReservaCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ReservaCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var ahora = DateTime.Now;

                var reservasExpiradas = context.Reservas.Where(r => r.HoraFin <= ahora).ToList();

                context.Reservas.RemoveRange(reservasExpiradas);
                await context.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}