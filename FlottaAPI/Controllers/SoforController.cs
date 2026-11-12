using FlottaAPI.DTOs;
using FlottaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoforController : ControllerBase
    {
        private FlottaContext _context;
        public SoforController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _context.Sofors.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error!" + ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = _context.Sofors.FirstOrDefault(s => s.Id == id);
                if (result == null)
                {
                    return NotFound("Missing.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error!" + ex.Message);
            }
        }

        [HttpPost("Ujsofor")]
        public IActionResult Post(Sofor ujsofor)
        {
            try
            {
                if (ujsofor==null)
                    return BadRequest("Empty missig driver.");
                _context.Sofors.Add(ujsofor);
                _context.SaveChanges();
                return Ok("Sofor added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!" + ex.Message);
            }
        }

        [HttpPut("SoforModositás")]
        public IActionResult Put(Sofor modositottsofor)
        {
            try
            {
                var ellenor = _context.Sofors.FirstOrDefault(s => s.Id == modositottsofor.Id);
                if (ellenor == null)
                    return NotFound("Missing driver.");
                _context.ChangeTracker.Clear();
                _context.Sofors.Update(modositottsofor);
                _context.SaveChanges();
                return Ok("Sofor updated sucessfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!" + ex.Message);
            }
        }

        [HttpDelete("Sofortorles")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _context.Sofors.FirstOrDefault(s => s.Id == id);
                if (result == null)
                {
                    return NotFound("Missing settlement.");
                }
                _context.ChangeTracker.Clear();
                _context.Sofors.RemoveRange(result);
                _context.SaveChanges();
                return Ok("A drivers deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!" + ex.Message);
            }
        }

        [HttpGet("GetAllData")]
        public IActionResult GetAllData()
        {
            try
            {
                var result = _context.Kikuldottjarmus
                    .Include(s => s.Kikuldetes)
                    .Include(j => j.Gepjarmu)
                    .Include(s => s.SoforNavigation)
                    .Select(j => new SoforRendszam() { Name = j.SoforNavigation!.Name, Rendszam = j.Gepjarmu!.Rendszam })
                    .GroupBy(s => s.Name)
                    .ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error!" + ex.Message);
            }
        }

        [HttpGet("SoforByIdJarmuvek/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetSoforByIdJarmuvek(int id)
        {
            try
            {
                var soforLetezik = await _context.Sofors.AnyAsync(s => s.Id == id);
                if (!soforLetezik)
                {
                    return NotFound($"A megadott azonosítóval ({id}) nem található sofőr.");
                }

                var rendszamok = await _context.Kikuldottjarmus
                    .Where(kj => kj.Sofor == id)
                    .Select(kj => kj.Gepjarmu!.Rendszam)
                    .Distinct()
                    .ToListAsync();
                return Ok(rendszamok);
            }
            catch(Exception ex)
            {
                return BadRequest("Error!"+ex.Message);
            }
        }
    }
}
