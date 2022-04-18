using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Extensions;
using SkolkaPerinka.Server.Data;
using SkolkaPerinka.Shared.Models;

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

        [HttpGet("getalldays/{month}/{year}")]
        public async Task<IActionResult> GetAllDays(int month, int year)
        {
            //var now = DateTime.Now;
            int numberOfAllDays = 42;
            
            
            var fullSelectedMonth = await CreateMonth(month, year, numberOfAllDays);
            

            return Ok(fullSelectedMonth);
        }

        private async Task<List<Day>> CreateMonth(int selectedMonth, int selectedYear, int numberOfAllDays)
        {
            int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
            DateTime SelectedMonth = new DateTime(selectedYear, selectedMonth, 1);
            //DayOfWeek startOfWeek = new DayOfWeek.mo;

            
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
            foreach ( var day in daysFromDatabase )
            {
                var date = days.Where(d => d.Date == day.Date).FirstOrDefault();
                if (date != null)
                {
                    date.NumberOfChild = day.NumberOfChild;
                }
            }

            return days;
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
