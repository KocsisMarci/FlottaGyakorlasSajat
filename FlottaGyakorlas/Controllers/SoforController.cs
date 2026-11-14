using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Argon2Sharp;
using FlottaGyakorlas.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace FlottaGyakorlas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoforController : ControllerBase
    {


        private FlottaContext _context;

        public SoforController(FlottaContext context) {
            _context = context;
        }

        [HttpGet("GetAllSofor")]
            

        
        public IActionResult GetAllSofor()
        {
            try
            {
                var adott = _context.Sofors.ToList();

                return Ok(adott);
            }


            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során {ex.Message}");
            
            }
        }


        [HttpGet("GetAsyncSoforById/{id:int}")]
        public async Task<IActionResult> GetAsyncSoforById([FromRoute] int id)
        {
            var adott = await _context.Sofors
                .FirstOrDefaultAsync(s => s.Id == id);

            if (adott == null)
                return NotFound("Nincs ilyen azonosítójú sofőr");

            return Ok(adott);
        }


        [HttpPost("PostSofor")]

        public async Task<IActionResult> PostSofor(Sofor sofor) {
            try
            {
                _context.ChangeTracker.Clear();

                await _context.Sofors.AddAsync(sofor);

                await _context.SaveChangesAsync();

                return Ok("Sikeres post történt");
            }


            catch(Exception ex){
                return StatusCode(500, $"Na hát akkor hiba történt, {ex.Message}");
            }
        }

        [HttpPut("PutSofor")]

        public IActionResult PutSofor(Sofor kapottSofor)
        {
            try
            {
                var toUpdate = _context.Sofors.FirstOrDefault(sofor => sofor.Id == kapottSofor.Id);
                _context.ChangeTracker.Clear();


                if (toUpdate is null)
                {
                    return NotFound("Nincsen ilyen id-val rendelkező sofőr");
                }

                _context.Update(toUpdate);

                _context.SaveChanges();

                return Ok("Sikeres put");
            }


            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }


        [HttpDelete("DeleteAsyncSofor/{id:int}")]
        public async Task<IActionResult> DeleteAsyncSofor([FromRoute] int id)
        {
            var toDelete = await _context.Sofors
                .FirstOrDefaultAsync(s => s.Id == id);

            if (toDelete == null)
                return NotFound("Nincs ilyen ID-val rendelkező sofőr");

            _context.Sofors.Remove(toDelete);
            await _context.SaveChangesAsync();

            return Ok("Sikeres törlés történt");
        }



        [HttpGet("GetSoforGepjarmu")]

        public IActionResult GetSoforGepJarmu(int id)
        {
            try
            {
                var adottSofor = _context.Sofors.FirstOrDefault(sofor => sofor.Id==id);

                if (adottSofor is null) {
                    return NotFound("Nincsen találat");
                }

                var osszekotve=_context.Kikuldottjarmus.Where(jarmu => jarmu.SoforNavigation.Id==adottSofor.Id).Select(j => j.Gepjarmu);

                return Ok(osszekotve);
            }


            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }
    }
}
