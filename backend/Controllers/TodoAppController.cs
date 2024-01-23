﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TodoApp.Models;
using System.Security.AccessControl;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using backend.Context;


namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAppController : ControllerBase
    {

        private IConfiguration _configuration;
        private readonly backend.Context.AppContext _context;

        public TodoAppController(IConfiguration configuration, backend.Context.AppContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpGet]
        [Route("GetTodos")]
        public async Task<List<Todo>> GetTodos()
        {
            return await _context.Todo.ToListAsync();
        }

        [HttpPost]
        [Route("AddTodo")]
        public async Task<Todo> AddTodo([FromForm] string description)
        {
            var todo = new Todo { Description = description, IsChecked = false };
            _context.Todo.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        [HttpDelete]        
        [Route("DeleteTodo")]

        public async Task<IActionResult> DeleteTodo(long id)
        {
            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
