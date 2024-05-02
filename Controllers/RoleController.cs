
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacilaIT.Helper.Shared;
using FacilaIT.Models;
using FacilaIT.Models.RABC;
using Microsoft.Extensions.Caching.Memory;
using FacilaIT.Business;
using Microsoft.AspNetCore.Identity;


namespace FacilaIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly DBBContext _context;
        private readonly ILogger _logger;

        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleController(DBBContext context, ILoggerFactory loggerFactory, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<RoleController>();
            _roleManager = roleManager;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<object>> GetRoles()
        {
            // 
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            RequestResult res = new RequestResult();
            _logger.LogInformation($"Start {methodName}");

            try
            {
                //
                string cacheKeySettings = Constant.Cache_General_AppSettings;
                bool InternalDB = SettingBusiness.SettingInternalDB();
                List<Role>? cachedDataService;
                //

                if (InternalDB)
                {

                    cachedDataService = _context.Roles.ToList();
                }
                else
                {
                    string cacheKeyService = Constant.Cache_Roles_AppSettings;
                    if (!CacheManager.Cache.TryGetValue(cacheKeyService, out cachedDataService))
                    {
                        cachedDataService = new List<Role>();
                    }
                }
                res.Response = cachedDataService;
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

        class RolesName
        {
            public string name { get; set; }

        }

        // GET: api/Role Get roles Name only
        [HttpGet("RolesName")]
        public async Task<ActionResult<object>> GetRolesName()
        {
            // 
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            RequestResult res = new RequestResult();
            _logger.LogInformation($"Start {methodName}");

            try
            {
                //
                string cacheKeySettings = Constant.Cache_General_AppSettings;
                bool InternalDB = SettingBusiness.SettingInternalDB();
                List<Role>? cachedDataService;
                List<RolesName>? resultRolesName = new List<RolesName>();
                //

                if (InternalDB)
                {

                    resultRolesName = _roleManager.Roles.Select(e => new RolesName { name = e.Name }).ToList();
                }
                else
                {
                    string cacheKeyService = Constant.Cache_Roles_AppSettings;
                    if (!CacheManager.Cache.TryGetValue(cacheKeyService, out cachedDataService))
                    {
                        cachedDataService = new List<Role>();
                    }

                    resultRolesName = cachedDataService.Select(e => new RolesName { name = e.Name }).ToList();
                }
                res.Response = resultRolesName;
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

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Role
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'DBBContext.Roles'  is null.");
            }
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return (_context.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
