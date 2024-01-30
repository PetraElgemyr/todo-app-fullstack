
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace backend.Context
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        {
        }
        public DbSet<TodoApp.Models.Todo> Todos { get; set; } = default!;
        public DbSet<TodoApp.Models.Person> Persons { get; set; } = default!;
        public DbSet<TodoApp.Models.List> Lists { get; set; } = default!;
    }
}