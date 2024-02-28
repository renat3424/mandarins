using mandarinProject1.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace mandarinProject1.Data
{
    //обращение к базе данных мандаринов
    public class MandarinRepository : IMandarinRepository
    {

        private readonly ApplicationDbContext _ctx;

        public MandarinRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public IEnumerable<Mandarin> GetAllMandarins()
        {

            return _ctx.Mandarins.ToList();
        }

        public Mandarin GetmandarinById(int id)
        {
            return _ctx.Mandarins.Where(p=>p.Id==id).FirstOrDefault();
        }

        public async Task AddMandarin()
        {
            //добавление мандарина с начальной стоимостью 100
            try
            {


                
                Mandarin mandarin = new Mandarin()
                {

                    CurrentPrize = 100,
                    Bought = false

                };


                await _ctx.Mandarins.AddAsync(mandarin);
                _ctx.SaveChanges();

            }  catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        
        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        public async Task DestroyMandarins()
        {
            try
            {


                //удалить все

                await _ctx.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Mandarins]");
                _ctx.SaveChanges();
            }catch (Exception ex) { 
                Console.WriteLine(ex.ToString());   
            }
        }

        public void Update(Mandarin model)
        {
            _ctx.Update(model);

        }
    }
}
