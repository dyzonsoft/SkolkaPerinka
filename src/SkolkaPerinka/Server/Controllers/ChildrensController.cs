using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkolkaPerinka.Server.Data;
using SkolkaPerinka.Shared.Models;

namespace SkolkaPerinka.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChildrensController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDBContext _context;
        public ChildrensController(UserManager<User> userManager, AppDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("getchildrensforparent/{parentEmail}")]
        public async Task<List<Children>> GetChildrensForParent(string parentEmail)
        {
            List<Children> childrens = await _context.Childrens.Where(ch => ch.ParentEmail == parentEmail).ToListAsync();

            return childrens;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ChildrenToRegister childrenToRegister)
        {
            Children children = new Children
            {
                FirstName = childrenToRegister.FirstName,
                LastName = childrenToRegister.LastName,
                Address = childrenToRegister.Address,
                BirthDate = childrenToRegister.BirthDate.Value,
                Gender = childrenToRegister.Gender,
                ParentEmail = childrenToRegister.ParentEmail
            };

         
                _context.Childrens.Add(children);
                var result = await _context.SaveChangesAsync();

            
                return Ok(children);
        

                //return BadRequest(result);
          
            

        }
    }
}
