using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkolkaPerinka.Client.Components;
using SkolkaPerinka.Server.Data;
using SkolkaPerinka.Shared.Models;
using System.Diagnostics;

namespace SkolkaPerinka.Server.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CalendarController : Controller
    {
        private readonly AppDBContext _appDbContext;

        public CalendarController(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("getalldays/{month}/{year}/{parentEmail}")]
        public async Task<IActionResult> GetAllDays(int month, int year, string parentEmail)
        {
            int numberOfAllDays = 42;
            var fullSelectedMonth = await CreateMonth(month, year, numberOfAllDays, parentEmail);

            return Ok(fullSelectedMonth);
        }

        [HttpPost("addchildrenstoschool")]
        public async Task<IActionResult> AddChildernsToSchool([FromBody] ChildrenToSchool childrenToSchool)
        {
            DateTime currentDay = childrenToSchool.CurrentDay;
            if (childrenToSchool.Variant == "Day")
            {
                var message = await SignInChildernToSchool(currentDay, childrenToSchool.ChildrenOfParent);
            }
            else if (childrenToSchool.Variant == "Week") 
            { 
                DateTime friday = currentDay.StartOfWeek(DayOfWeek.Monday).AddDays(4);
                if(currentDay.DayOfWeek == DayOfWeek.Sunday || currentDay.DayOfWeek == DayOfWeek.Saturday)
                {
                    return BadRequest("SoNeCannotVariantWeek");
                }

                int numberOfDays = (friday - currentDay).Days;
                for (int i = 0; i <= numberOfDays; i++)
                {
                    DateTime dayToSign = currentDay.AddDays(i);
                    var message = await SignInChildernToSchool(dayToSign, childrenToSchool.ChildrenOfParent);
                }
            }
            else if (childrenToSchool.Variant == "Month")
            {
                int daysInMonth = DateTime.DaysInMonth(currentDay.Year, currentDay.Month);
                int numberOfDays = (daysInMonth - currentDay.Day);
                for (int i = 0; i <= numberOfDays; i++)
                {
                    DateTime dayToSign = currentDay.AddDays(i);

                    if (!(dayToSign.DayOfWeek == DayOfWeek.Sunday || dayToSign.DayOfWeek == DayOfWeek.Saturday))
                    {
                        var message = await SignInChildernToSchool(dayToSign, childrenToSchool.ChildrenOfParent);
                    }
                }
            }

            _appDbContext.SaveChanges();
            return Ok();
        }

        private async Task<List<Day>> CreateMonth(int selectedMonth, int selectedYear, int numberOfAllDays, string parentEmail)
        {
            int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
            DateTime SelectedMonth = new DateTime(selectedYear, selectedMonth, 1);
            
            DateTime pondeli = SelectedMonth.StartOfWeek(DayOfWeek.Monday);
            int previewMonthDaysNumber = (int)(SelectedMonth - pondeli).TotalDays;
            previewMonthDaysNumber += 1;
            int nextMonthDaysNumber = numberOfAllDays - previewMonthDaysNumber - daysInMonth;
            int prewMonthStartNumber = SelectedMonth.AddDays(-(previewMonthDaysNumber)).Day;
            var prewMounth = SelectedMonth.AddMonths(-1);
            int prewMonthDays = DateTime.DaysInMonth(prewMounth.Year, prewMounth.Month);
            int nextMonthEndNumber = SelectedMonth.AddDays(daysInMonth + nextMonthDaysNumber).Day;

            DateTime firstDayToSearch = SelectedMonth.AddDays(-(previewMonthDaysNumber));
            DateTime lastDayToSearch = SelectedMonth.AddDays(daysInMonth + nextMonthDaysNumber - 1);
            List<Day> days = new List<Day>(); 

            for (int i = prewMonthStartNumber; i <= prewMonthDays - 1; i++)
            {
                DateTime currentDay = SelectedMonth.AddMonths(-1).AddDays(i);
                Day day = new Day
                {
                    Date = currentDay,
                    CurrentMounth = false,
                    Background = "",
                    NumberOfChild = 0
                };
                if (currentDay == DateTime.Today) day.Background = "today-background";
                days.Add(day);
            }

            for (int i = 0; i < daysInMonth; i++)
            {
                DateTime currentDay = SelectedMonth.AddDays(i);
                Day day = new Day
                {
                    Date = currentDay,
                    CurrentMounth = true,
                    Background = "current-month-background",
                    NumberOfChild = 0
                };
                if (currentDay == DateTime.Today) day.Background = "today-background";
                days.Add(day);
            }

            for (int i = 0; i < nextMonthEndNumber; i++)
            {
                DateTime currentDay = SelectedMonth.AddMonths(1).AddDays(i);
                Day day = new Day
                {
                    Date = currentDay,
                    CurrentMounth = false,
                    Background = "",
                    NumberOfChild = 0
                };
                if (currentDay == DateTime.Today) day.Background = "today-background";
                days.Add(day);
            }


            List<Day> daysFromDatabase = await _appDbContext.Days.Where((day) => day.Date >= firstDayToSearch)
                                                .Where((day) => day.Date <= lastDayToSearch)
                                                .ToListAsync();
            List<Children> childrens = await _appDbContext.Childrens.Where((child) => child.ParentEmail == parentEmail).ToListAsync();
            foreach ( var day in daysFromDatabase )
            {
                var date = days.Where(d => d.Date == day.Date).FirstOrDefault();
                if (date != null)
                {
                    date.NumberOfChild = day.NumberOfChild;
                    date.IdChildrensInSchool = day.IdChildrensInSchool;
                    
                    foreach ( var child in childrens )
                    {
                        var childrenInScoolAtDay = day.IdChildrensInSchool;
                        var f = childrenInScoolAtDay.Split("|");
                        string childId = child.Id.ToString();
                        bool childIdeIsExists = f.Any(x => f.Contains(childId));
                        if (childIdeIsExists)
                        {
                            ChildrenOfParentIcon childIcon = new ChildrenOfParentIcon
                            {
                                Gender = child.Gender,
                                Name = child.FirstName
                            };
                            date.ChildrensOfParentIcon.Add(childIcon);
                        }
                    }
                }
            }

            return days;
        }

        private async Task<IActionResult> SignInChildernToSchool(DateTime currentDay, List<Children> childrenOfParent)
        {
            bool isNew = false;
            Day day = await _appDbContext.Days.FirstOrDefaultAsync((d) => d.Date == currentDay);

            if (day == null)
            {
                day = new();
                day.Date = currentDay;
                isNew = true;
            }

            var childrenInScoolAtDay = day.IdChildrensInSchool;
            var f = childrenInScoolAtDay.Split("|");
            
            foreach (var child in childrenOfParent)
            {
                string childId = child.Id.ToString();
                bool childIdeIsExists = f.Any(x => f.Contains(childId));

                if (child.Checked) // má být ve školce
                {
                    if (!childIdeIsExists)
                    {
                        day.IdChildrensInSchool += childId + "|";
                        day.NumberOfChild += 1;
                    }
                }
                else // nemá být be školce
                {
                    if (childIdeIsExists)
                    {
                        int startId = day.IdChildrensInSchool.IndexOf("|" + childId + "|");
                        day.IdChildrensInSchool = day.IdChildrensInSchool.Remove(startId + 1, childId.Length +1);
                        if (day.NumberOfChild == 0) day.NumberOfChild = 0;
                        else day.NumberOfChild -= 1;
                    }
                }

                if (isNew) _appDbContext.Days.Add(day);
                else _appDbContext.Days.Update(day);
            }
            
            return Ok("hotovo");
        }
    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}