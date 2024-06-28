using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    //[Route("api/[controller]")]  // Asigna automáticamente el nombre del controlador
    [Route("api/cities")]  // Asigna manual el nombre del controlador
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public CitiesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var response = await _context.Cities.ToListAsync();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(City city)
        {
            try
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(City city)
        {
            try
            {
                _context.Update(city);
                await _context.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
