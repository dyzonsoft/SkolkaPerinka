using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkolkaPerinka.Server.Data;
using SkolkaPerinka.Shared.Models;
using System.Globalization;

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

        [HttpGet("getchildrensforparent/{parentEmail}/{selectedDate}")]
        public async Task<List<Children>> GetChildrensForParent(string parentEmail, string selectedDate)
        {
            List<Children> childrens = await _context.Childrens.Where(ch => ch.ParentEmail == parentEmail).ToListAsync();
            var cultureInfo = new CultureInfo("cs-CZ");
            var currDay = DateTime.Parse(selectedDate, cultureInfo);
            Day day = _context.Days.FirstOrDefault((d) => d.Date == currDay);
            if (day != null)
            {
                foreach (var child in childrens)
                {
                    var childrenInScoolAtDay = day.IdChildrensInSchool;
                    var f = childrenInScoolAtDay.Split("|");
                    string childId = child.Id.ToString();
                    bool childIdeIsExists = f.Any(x => f.Contains(childId));
                    if (childIdeIsExists)
                    {
                        
                        child.Checked = true;
                    }
                }
            }
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
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(Children children)
        {
            var test = await _context.Childrens.Where(f => f.Id == children.Id).FirstOrDefaultAsync();
            if (test != null)
            {
                _context.Childrens.Remove(test);
                await _context.SaveChangesAsync();
                return Ok(children);
            }
            else
            {
                return BadRequest(children);
            }
        }
    }
}