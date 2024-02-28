using mandarinProject1.Data;
using mandarinProject1.Data.Entities;

namespace mandarinProject1.Services
{
    //создание мандаринок раз в 5 минут
    public class CreatingService : BackgroundService
    {
        public readonly PeriodicTimer _timer=new PeriodicTimer(TimeSpan.FromMinutes(1));
        private readonly IServiceScopeFactory _scopeFactory;

        public CreatingService(IServiceScopeFactory scopeFactory)
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
                await _repository.AddMandarin();
                

            }       
        }
    }
}
