namespace mandarinProject1.Services
{
    public class NullMailService : IMailService
    {

        private readonly ILogger<NullMailService> logger;

        public NullMailService(ILogger<NullMailService> _logger)
        {
            logger = _logger;
        }
        public void SendNotificationMessage(string to, int id, int cost)
        {
            logger.LogInformation($"Кому: {to}, Тема: Уведомление о повышении ставки, Письмо: Уведомление о том, что ставка на лот {id} повышена до {cost}.");
        }

        public void SendPurchaseMessage(string to, int id, int cost)
        {
            logger.LogInformation($"Кому: {to}, Тема: Уведомление о покупке, Письмо: Уведомление о том, что лот {id} был успешно вами приобретен. Стоимость покупки составила {cost}.");

        }
    }
}
