
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacilaIT.Models;
using FacilaIT.Models.RABC;
using FacilaIT.Helper.Shared;

//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FacilaIT.Business;

namespace FacilaIT.Controllers
{
    [Authorize(Roles = UserRoles.UserPage)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DBBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        public UserController(
            UserManager<IdentityUser> userManager, DBBContext context, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<UserController>();
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<object>> GetUsers()
        {
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            RequestResult res = new RequestResult();
            _logger.LogInformation($"Start {methodName}");

            try
            {
                //
                List<User> users = _userManager.Users.Select(e => new User
                {
                    Email = e.Email,
                    EmailConfirmed = e.EmailConfirmed,
                    Username = e.UserName,
                    Id = e.Id
                }).ToList();

                res.Response = users;
            }
            catch (System.Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            _logger.LogInformation($"End {methodName} {res.IsSuccess}");
            return res;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUser(string id)
        {

            //
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            RequestResult res = new RequestResult();
            _logger.LogInformation($"Start {methodName}");

            try
            {
                //
                var user = await _userManager.FindByIdAsync(id);

                res.Response = new
                {
                    Email = user.Email,
                    Username = user.UserName,
                    Id = user.Id,
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }
            catch (System.Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            _logger.LogInformation($"End {methodName} {res.IsSuccess}");
            return res;
            //
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DBBContext.Users'  is null.");
            }

            if (user != null && user.Password != null)
            {
                user.Password = PasswordHasher.HashPassword(user.Password);
                _context.Users.Add(user);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }


        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
