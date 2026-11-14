using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Argon2Sharp;
using FlottaGyakorlas.Models;
using FlottaGyakorlas.DTOs;
namespace FlottaGyakorlas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        public FlottaContext _context;

        public LoginController(FlottaContext context) {
            _context = context;
        }

        private string hash;


        [HttpPost("Login")]
        public IActionResult Login(LoginDTO logindto) {
            try
            {
                var vane = _context.Users.FirstOrDefault(user => user.LoginName == logindto.LoginName);
                if (vane is null) {
                    return NotFound("Nincs ilyen felhasználó");
                }

                hash = vane.Hash;


                var (isValid, parameters) = Argon2Sharp.Argon2PhcFormat.VerifyPhcString(logindto.Password, hash);


                if (isValid)
                {
                    return Ok("Token");
                }

                else
                {
                    return NotFound("Helytelen jelszó");
                }
                
            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }
    }
}
