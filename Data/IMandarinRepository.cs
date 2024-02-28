using mandarinProject1.Data.Entities;

namespace mandarinProject1.Data
{
    public interface IMandarinRepository
    {
        IEnumerable<Mandarin> GetAllMandarins();
        bool SaveAll();

        Mandarin GetmandarinById(int id);
        void AddEntity(object model);
        Task AddMandarin();
        Task DestroyMandarins();
        void Update(Mandarin model);
    }
}
