using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    //[Route("api/[controller]")]  // Asigna automáticamente el nombre del controlador
    [Route("api/states")]  // Asigna manual el nombre del controlador
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly DataContext _context;

        public StatesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var response = await _context.States
                .Include(x => x.Cities)     // Equivalente a un Inner Join con la tabla Cities
                .ToListAsync();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var state = await _context.States
                .Include(x => x.Cities)     // Equivalente a un Inner Join con la tabla Cities
                .FirstOrDefaultAsync(x => x.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            return Ok(state);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(State state)
        {
            try
            {
                _context.Add(state);
                await _context.SaveChangesAsync();
                return Ok(state);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un estado con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(State state)
        {
            try
            {
                _context.Update(state);
                await _context.SaveChangesAsync();
                return Ok(state);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un estado con el mismo nombre.");
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
            var state = await _context.States.FirstOrDefaultAsync(x => x.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            _context.Remove(state);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
