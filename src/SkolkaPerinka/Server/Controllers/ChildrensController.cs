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

        [HttpGet("getallchildrens")]
        public async Task<List<Children>> GetAllChildrens()
        {
            List<Children> childrens = await _context.Childrens.ToListAsync();
            return childrens;
        }

        [HttpGet("getchildrenbyid/{id}")]
        public async Task<Children> GetChildrenById(int id)
        {
            Children children = new(); 
            children = await _context.Childrens.SingleOrDefaultAsync(ch => ch.Id == id);
            return children;
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
            await _context.SaveChangesAsync();

            return Ok(children);
        }

        [HttpPost]
        [Route("deletechild")]
        public async Task<IActionResult> DeleteChild(Children children)
        {
            var test = await _context.Childrens.Where(f => f.Id == children.Id).FirstOrDefaultAsync();
            if (test != null)
            {
                List<Children> childrens = new();
                childrens.Add(test);
                await DeleteChildernsFromSchoolToFuture(DateTime.Today, childrens);

                _context.Childrens.Remove(test);
                await _context.SaveChangesAsync();
                return Ok(children);
            }
            else
            {
                return BadRequest(children);
            }
        }

        [HttpPost]
        [Route("deleteallchildofparent")]
        public async Task<IActionResult> DeleteAllChildOfParent(User user)
        {
            List<Children> childrens = new();
            childrens = await _context.Childrens.Where(f => f.ParentEmail == user.Email).ToListAsync();
            if (childrens != null)
            {
                foreach(var child in childrens)
                {
                    await DeleteChildernsFromSchoolToFuture(DateTime.Today, childrens);
                    _context.Childrens.Remove(child);
                }

                await _context.SaveChangesAsync();
                return Ok(user.Email);
            }
            else
            {
                return BadRequest(user.Email);
            }
        }

        private async Task<IActionResult> DeleteChildernsFromSchoolToFuture(DateTime currentDay, List<Children> childrenOfParent)
        {
            List<Day> days = await _context.Days.Where((d) => d.Date >= currentDay).ToListAsync();

            if (days != null)
            { 
                foreach (var day in days)
                {
                    var childrenInScoolAtDay = day.IdChildrensInSchool;
                    var f = childrenInScoolAtDay.Split("|");

                    foreach (var child in childrenOfParent)
                    {
                        string childId = child.Id.ToString();
                        bool childIdeIsExists = f.Any(x => f.Contains(childId));

                        if (childIdeIsExists)
                        {
                            int startId = day.IdChildrensInSchool.IndexOf("|" + childId + "|");
                            day.IdChildrensInSchool = day.IdChildrensInSchool.Remove(startId + 1, childId.Length + 1);
                            if (day.NumberOfChild == 0) day.NumberOfChild = 0;
                            else day.NumberOfChild -= 1;
                            
                            _context.Days.Update(day);
                        }

                    }
                }
                await _context.SaveChangesAsync();
                return Ok("hotovo");
            }
            else
            {
                return Ok("zadnej den");
            }
        }
    }
}