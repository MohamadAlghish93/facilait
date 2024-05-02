using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using FacilaIT.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FacilaIT.Models.RABC;
using Microsoft.AspNetCore.Authorization;
using FacilaIT.Helper.Shared;
using FacilaIT.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FacilaIT.Controllers
{
    [Route("api")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;


        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<AuthenticateController>(); ;
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login([FromBody] LoginModel model)
        {
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            _logger.LogInformation($"Start {methodName}, {model.Username}");
            RequestResult res = new RequestResult();
            IdentityUser user = null;
            try
            {
                if (model.ExternalSP)
                {
                    user = await _userManager.FindByEmailAsync(model.ExternalFilter);
                }
                else
                {
                    user = await _userManager.FindByNameAsync(model.Username);
                }


                if (user != null && (await _userManager.CheckPasswordAsync(user, model.Password) || model.ExternalSP))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    // Roles
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);

                    // General settings
                    string cacheKeySettings = Constant.Cache_General_AppSettings;
                    bool InternalDB = false;
                    SettingItem cachedDataSetting;
                    if (!CacheManager.Cache.TryGetValue(cacheKeySettings, out cachedDataSetting))
                    {
                        InternalDB = false;
                    }
                    else
                    {
                        InternalDB = cachedDataSetting.InternalDB;
                    }

                    res.Response = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        roles = userRoles,
                        settings = new { InternalDB = InternalDB }
                    };
                }
                else
                {
                    return Unauthorized();
                }
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

        [HttpPost]
        [Route("register")]
        [Authorize(Roles = UserRoles.UserPage)]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            System.Console.WriteLine(model.Username);
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new RequestResult { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            foreach (var item in result.Errors)
            {
                System.Console.WriteLine(item.Description);
            }
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new RequestResult { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new RequestResult { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        [Authorize(Roles = UserRoles.UserPage)]
        public async Task<IActionResult> RegisterAdmin([FromBody] User model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new RequestResult { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new RequestResult { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new RequestResult { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route("AssignRolesUser")]
        [Authorize(Roles = UserRoles.UserPage)]
        public async Task<ActionResult<object>> AssignRolesUser([FromBody] RoleUser model)
        {
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            _logger.LogInformation($"Start {methodName}, {model.Name}");
            RequestResult res = new RequestResult();
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Name);
                if (userExists == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new RequestResult { Status = "Error", Message = "User dosn't exists! Please check user details and try again." });


                foreach (var item in UserRoles.GetListRoles())
                {
                    if (!await _roleManager.RoleExistsAsync(item))
                        await _roleManager.CreateAsync(new IdentityRole(item));
                }

                foreach (var item in model.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(item))
                    {
                        await _userManager.AddToRoleAsync(userExists, item);
                    }
                }

                // Revoke user roles doesn't exist
                var userRoles = await _userManager.GetRolesAsync(userExists);
                List<string> removedRoles = new List<string>();
                foreach (var role in userRoles)
                {
                    if (!model.Roles.Contains(role))
                    {
                        removedRoles.Add(role);
                    }
                }
                if (removedRoles.Count > 0)
                {
                    await _userManager.RemoveFromRolesAsync(userExists, removedRoles);
                }
                //

                res.Response = Ok(new RequestResult { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

            }
            _logger.LogInformation($"End {methodName} {res.IsSuccess}");

            return res;
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }


    }
}