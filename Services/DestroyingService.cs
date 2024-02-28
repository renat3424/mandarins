using mandarinProject1.Data;
using mandarinProject1.Data.Entities;
using NuGet.Protocol.Core.Types;

namespace mandarinProject1.Services
{
    //уничтожение всех мандаринок через 24 часа
    public class DestroyingService : BackgroundService
    {
        public readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromHours(24));
        private readonly IServiceScopeFactory _scopeFactory;

        public DestroyingService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoAwaitTask();

            while (await _timer.WaitForNextTickAsync() && !stoppingToken.IsCancellationRequested)
            {

                await DoAwaitTask();
            }
        }

        private async Task DoAwaitTask()
        {

            using (var scope = _scopeFactory.CreateScope())
            {

                var _repository = scope.ServiceProvider.GetRequiredService<IMandarinRepository>();
                await _repository.DestroyMandarins();


            }
        }
    }
}