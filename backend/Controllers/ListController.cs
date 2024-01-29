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
    }
}
