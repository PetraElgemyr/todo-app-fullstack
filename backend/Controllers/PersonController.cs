using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
          //  return await _context.Todo.ToListAsync();
        }

    }
}
