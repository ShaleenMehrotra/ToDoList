using Microsoft.EntityFrameworkCore;
using Models;
using System;

namespace DataProvider
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options): base(options)
        {

        }

        public DbSet<Task> Tasks { get; set; } 
    }

}
