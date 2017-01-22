using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AspNetTomasosPizzeria1_0.Models
{
    public class AppUser: IdentityUser
    {
        public virtual Kund Kund { get; set; }
        public int KundId { get; set; }
    }
}