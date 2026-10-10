using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //localhost:5000/api/members
    public class MembersController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembersList()
        {
           // return context.Users.ToList();
           var members = await context.Users.ToListAsync();
           return members;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(int id)
        {
            var member = await context.Users.FindAsync(id);
            if (member == null) return NotFound();
            return member;
        }
    }       
}