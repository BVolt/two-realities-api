using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Claims;
using two_realities.Models;
using two_realities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


namespace two_realities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static User user = new User();
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;


        public AuthController(DataContext dataContext, IConfiguration configuration) {
        
            _context = dataContext;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<ActionResult<string>> register(UserDto request)
        {
            try
            {
                string id = request.UserId;
                string trimmed = String.Concat(id.Where(character => !Char.IsWhiteSpace(character)));
                if (trimmed == "")
                {
                    return BadRequest("Not a valid user id");
                }
                var userCheck = await _context.Users.FindAsync(request.UserId);
                if (userCheck != null)
                {
                    return BadRequest("User Name is taken.");
                }
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.UserId = request.UserId;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> login(UserDto request)
        {
                user.UserId = request.UserId;
                var validUsers = await _context.Users.ToListAsync();
                bool isValid = false;
                User currUser = new User();
                for (int i = 0; i < validUsers.Count; i++)
                {
                    if (validUsers[i].UserId == request.UserId)
                    {
                        currUser = validUsers[i];
                        isValid = true;
                    }
                }
                if (!isValid)
                {
                    return NotFound("User does not exist");
                }
                if (!VerifyPasswordHash(request.Password, currUser.PasswordHash, currUser.PasswordSalt))
                {
                    return BadRequest("Password is Incorrect");
                }
                string newtoken = CreateToken(currUser);
                

                return Ok(new JsonResult(newtoken));
        }

        [Authorize]
        [HttpGet]
        public ActionResult getId() {
            var authHeader = Request.Headers.Authorization.First().Split(" ");
            var token = authHeader[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var userId = new JsonResult(jwtSecurityToken.Claims.First().Value);

            return Ok(userId);
        }

        //[Authorize]
        //[HttpDelete("{UserId}")]
        //public async Task<ActionResult> deleteAcc(string UserId)
        //{
        //    try
        //    {
        //        _context.Users.Remove(await _context.Users.FindAsync(UserId));
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        return BadRequest();
        //    }
        //}

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserId)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}