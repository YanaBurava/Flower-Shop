using Flower.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flower.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Table>Table { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
    }
}
