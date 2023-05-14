using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDo;

namespace ToDo.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext (DbContextOptions<ToDoContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<ToDo.Todo> Todo { get; set; } = default!;
    }
}
