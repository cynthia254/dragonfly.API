using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayhouseDragonFly.CORE.Models.TicketRegistration;
using PayhouseDragonFly.CORE.Models.UserRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.INFRASTRUCTURE.DataContext
{
    public  class DragonFlyContext: IdentityDbContext<PayhouseDragonFlyUsers>
    {
        public DragonFlyContext(DbContextOptions<DragonFlyContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PayhouseDragonFlyUsers>(entity =>
            {
                entity.ToTable(name: "PayhouseDragonFlyUsers");
            });
        }
        public DbSet<PayhouseDragonFlyUsers> PayhouseDragonFlyUsers { get; set; }

        public DbSet<Tickets>  Tickets { get; set; }    



    }
}
