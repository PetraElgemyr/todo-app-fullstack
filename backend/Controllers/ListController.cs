using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : Controller
    {
        private IConfiguration _configuration;
        private readonly backend.Context.AppContext _context;
        public ListController(IConfiguration configuration, backend.Context.AppContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        [Route("GetLists")]

        public async Task<List<List>> GetLists()
        {
            return await _context.Lists.ToListAsync();
        }

        [HttpGet]
        [Route("GetListById")]
        public async Task<List?> GetListById(long id)
        {
            var list = await _context.Lists.SingleOrDefaultAsync(e => e.Id == id);
            return list;

        }

        [HttpPost]
        [Route("AddList")]
        public async Task<IActionResult> AddList(long pId, string listName)
        {
            if (listName == null)
            {
                return BadRequest();
            }

            List newList = new List
            {
                PersonId = pId,
                ListName = listName
            };

            await _context.SaveChangesAsync();

            return Ok(newList);
        }


        [HttpPost]
        [Route("UpdateListName")]
        public async Task<List?> UpdateListName(long id, string listName)
        {
            var listToChange = await _context.Lists.SingleOrDefaultAsync(e => e.Id == id);
            if (listToChange != null)
            {
                 listToChange.ListName = listName;
            await _context.SaveChangesAsync();
            }
            return listToChange;
        }

        [HttpDelete]
        [Route("DeleteList")]
        public async Task<IActionResult> DeleteList(long id)
        {
            var listToRemove =await _context.Lists.FindAsync(id);

            if (listToRemove == null)
            {
                return NotFound();
            }

           _context.Lists.Remove(listToRemove);

            List<Todo> todosToRemove = await _context.Todos.Where(e => e.ListId == id).ToListAsync();
            _context.Todos.RemoveRange(todosToRemove);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
