using Microsoft.AspNetCore.Identity;

namespace mandarinProject1.Data.Entities
{
    public class Mandarin
    {
        public int Id { get; set; }
        public bool Bought { get; set; }

        public int CurrentPrize { get; set; }
        public IdentityUser? User { get; set; }
    }
}
