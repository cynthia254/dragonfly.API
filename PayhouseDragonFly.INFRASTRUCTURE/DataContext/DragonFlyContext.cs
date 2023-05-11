using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayhouseDragonFly.CORE.DTOs.RegisterVms;
using PayhouseDragonFly.CORE.Models.departments;
using PayhouseDragonFly.CORE.Models.Designation;
using PayhouseDragonFly.CORE.Models.Roles;
using PayhouseDragonFly.CORE.Models.statusTable;
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
        public DbSet<Departments> Departments { get; set; }
        public DbSet<RolesTable>  RolesTable { get; set; }
        public DbSet<UserStatusTable> UserStatusTable { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<RoleClaimsTable> RoleClaimsTable { get; set; }

        public  DbSet<Claim_Role_Map> Claim_Role_Map { get; set; }


    }
}
