using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkolkaPerinka.Server.Data;
using SkolkaPerinka.Shared.Models;

namespace SkolkaPerinka.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Director")]
    public class WeatherForecastController : ControllerBase
    {
        //private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDBContext _context;

        public WeatherForecastController(/*ILogger<WeatherForecastController> logger, */AppDBContext context)
        {
            //_logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var forecast = await _context.Forecasts.ToListAsync();
            return Ok(forecast);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var forecast = await _context.Forecasts.Where(f => f.Id == id).FirstOrDefaultAsync();
            return Ok(forecast);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeatherForecast forecast)
        {
            _context.Forecasts.Add(forecast);
            await _context.SaveChangesAsync();
            return Ok(forecast);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, WeatherForecast updatedForecast)
        {
            _context.Forecasts.Update(updatedForecast);
            await _context.SaveChangesAsync();
            return Ok(updatedForecast);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var forecast = await _context.Forecasts.Where(f => f.Id == id).FirstOrDefaultAsync();
            _context.Forecasts.Remove(forecast);
            await _context.SaveChangesAsync();
            return Ok(forecast);
        }
    }
}