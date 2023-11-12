using GadgetIPhoneStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GadgetIPhoneStore.Context
{
    public class DbContextClass : IdentityDbContext<IdentityUser>
    {
        protected readonly IConfiguration Configuration;
        public DbContextClass(DbContextOptions config) : base(config) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
