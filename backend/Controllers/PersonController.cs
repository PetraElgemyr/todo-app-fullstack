using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoApp.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {

        private IConfiguration _configuration;
        private readonly backend.Context.AppContext _context;

        public PersonController(IConfiguration configuration, backend.Context.AppContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        [Route("GetPersons")]
        public async Task<List<Person>> GetPersons()
        {
            return await _context.Persons.ToListAsync();
        }

        [HttpGet]
        [Route("GetPersonById")]
        public async Task<Person?> GetPersonById(long id)
        {
            // IQueryable<Person> query = _context.Persons.Where(e => e.Id == 1);
            // string sql = query.ToQueryString();


            return  await _context.Persons.SingleOrDefaultAsync(p => p.Id == id);
        }

        [HttpPost]
        [Route("AddPerson")]
        public async Task<IActionResult> AddPerson([FromBody] string name,string email, string password, bool isAdmin)
        {
            var personWithExistingEmail = await _context.Persons.SingleOrDefaultAsync(p => p.Email == email);
            if (personWithExistingEmail != null)
            {
                return BadRequest("Email redan tagen");
            }

            Person personToAdd = new Person();
            personToAdd.Name = name;
            personToAdd.Email = email;
            personToAdd.Password = password;
            if (isAdmin)
            {
                personToAdd.Role = "admin";
            }
            _context.Persons.Add(personToAdd);
            await _context.SaveChangesAsync();
            return Ok(personToAdd);
        }

        [HttpDelete]
        [Route("DeletePerson")]
        public async Task<StatusCodeResult> DeletePerson(long id)
        {
            Person personToDelete = await _context.Persons.FindAsync(id);

            if (personToDelete != null)
            {
                var listsToRemove = await _context.Lists.Where(l => l.PersonId == id).ToListAsync();

                if (listsToRemove == null )
                {
                    return Ok();
                }


                    foreach (var l in listsToRemove)
                    {
                        List<Todo> todosToRemove = await _context.Todos.Where(todo => todo.ListId == l.Id).ToListAsync();
                        _context.Todos.RemoveRange(todosToRemove);
                    }
                    _context.Lists.RemoveRange(listsToRemove);
                    _context.Persons.Remove(personToDelete);
                
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> LoginUser(string email, string password)
        {
            if (email == null | password == null)
            {    
                return BadRequest("Både email och lösenord behöver fyllas i");
            }

            var personToLogIn = await _context.Persons.SingleOrDefaultAsync(p => p.Email == email);
            if (personToLogIn.Password == password)
            {
                return Ok(personToLogIn);
            } else
            {
                return BadRequest("Fel inloggningsuppgifter");
            }
        }
    }
}
