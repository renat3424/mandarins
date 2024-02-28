namespace mandarinProject1.Services
{
    public interface IMailService
    {//отправка почты
        void SendNotificationMessage(string to, int id, int cost);
        void SendPurchaseMessage(string to, int id, int cost);

    }
}
