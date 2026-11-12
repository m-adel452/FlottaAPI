using Argon2Sharp;
using FlottaAPI.DTOs;
using FlottaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private FlottaContext _context;
        
        public LoginController(FlottaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                if (loginDTO == null)
                {
                    return BadRequest("Login name and password reyuired!");
                }
                User? loginUser = _context.Users.FirstOrDefault(u => u.LoginName == loginDTO.LoginName);
                if(loginUser==null)
                {
                    return BadRequest("Bad user!");
                }
                var (isOk, Parameters) = Argon2PhcFormat.VerifyPhcString(loginDTO.Password, loginUser.Hash);
                if(isOk)
                {
                    //helyes password
                    return Ok("token");
                }
                else
                {
                    return BadRequest("Bad password!");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Something went wrong!" + ex.Message);
            }
        }
    }
}
