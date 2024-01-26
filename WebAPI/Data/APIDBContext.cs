using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Configurations.Entities;

namespace WebAPI.Data
{
    public class APIDBContext : IdentityDbContext<APIUser>
    {
        public APIDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Booking> Booking { get; set; }
        public DbSet<Driver> Driver { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());

        }


    }
}
