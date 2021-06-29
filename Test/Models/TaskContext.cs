using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class TaskContext : DbContext
    {

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
            // создаем базу данных при первом обращении
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
            //optionsBuilder.LogTo(System.Console.WriteLine);// пока лог в консоль не нужен
            
        }

    }
}

