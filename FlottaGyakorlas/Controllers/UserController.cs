using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlottaGyakorlas.Models;

namespace FlottaGyakorlas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private FlottaContext _context;

        public UserController(FlottaContext context) {
            _context = context;
        }



        [HttpGet("GetAllAsyncUser")]

        public async Task<IActionResult> GetAllAsnycUser()
        {
            try
            {
                var adottak = _context.Users.ToList();
                return Ok(adottak);
            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }


        [HttpGet("GetUserById/{id:int}")]

        public IActionResult GetUserById([FromRoute] int id)
        {
            try
            {
                var adottFelhasznalo = _context.Users.FirstOrDefault(user => user.Id == id);

                if (adottFelhasznalo is null) {
                    return NotFound("Nincsen találat");
                }

                else
                {
                    return Ok(adottFelhasznalo);
                }

            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }

        [HttpPost("PosAsynctUser")]


        public async Task<IActionResult> PostUser(User user) {
            try
            {
                _context.ChangeTracker.Clear();
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok("Sikeres post történt");
            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }


        [HttpPut("PutUser")]

        public IActionResult PutUser(User givenUser)
        {
            try
            {
                var toUpdate = _context.Users.FirstOrDefault(user => user.Id == givenUser.Id);
                _context.ChangeTracker.Clear();

                if (toUpdate  is null)
                {
                    return NotFound("Nincsen találat");

                }


                _context.Users.Update(givenUser);

                _context.SaveChanges();

                return Ok("Sikeres put");
            }


            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }

        [HttpDelete("DeleteAsyncUser/{id:int}")]

        public async Task<IActionResult> DeleteAsyncUser([FromRoute] int id)
        {
            try
            {
                var toDelete =  _context.Users.FirstOrDefault(user => user.Id == id);

                _context.ChangeTracker.Clear();

                if (toDelete is null) {
                    return NotFound("Nincs ilyen id-jú user");
                }

                 _context.Remove(toDelete);

                await _context.SaveChangesAsync();

                return Ok("Sikeres törlés");

            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        
        }
    }

}
