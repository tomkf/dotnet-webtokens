using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todoWebApi.Data {
    public class ToDo {
        [Key] // Enables auto-increment.
        public int    Id          { get; set; }
        public string Description { get; set; }
        public bool   IsComplete  { get; set; }
    }

    public class TodoContext : DbContext {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Seed data. Auto-increment PK’s must still be specified in seed data.
            modelBuilder.Entity<ToDo>().HasData(
                new { Id = 1, Description = "Clean house", IsComplete = false },
                new { Id = 2, Description = "Bake cake",   IsComplete = false }
            );
        }
    }

}
