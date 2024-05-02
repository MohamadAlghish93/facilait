using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacilaIT.Models;
using FacilaIT.Models.RABC;
using Microsoft.AspNetCore.Authorization;
using FacilaIT.Helper.Shared;
using Microsoft.Extensions.Caching.Memory;
using FacilaIT.Helper.Shared;

namespace FacilaIT.Controllers
{
    [Authorize(Roles = UserRoles.SettingsPage)]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly DBBContext _context;
        private readonly ILogger _logger;


        public SettingController(DBBContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory
                  .CreateLogger<SettingController>();
        }

        // GET: api/Setting
        [HttpGet("GetSetting")]
        public async Task<ActionResult<object>> GetSetting()
        {
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            _logger.LogInformation($"Start {methodName} ");
            RequestResult res = new RequestResult();
            try
            {

                string cacheKey = Constant.Cache_General_AppSettings;
                SettingItem cachedData;
                if (!CacheManager.Cache.TryGetValue(cacheKey, out cachedData))
                {
                    return NotFound();
                }
                else
                {
                    res.Response = cachedData;
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


        [HttpPost("saveSetting")]
        public async Task<ActionResult<object>> saveSetting(SettingItem setting)
        {
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            _logger.LogInformation($"Start {methodName} ");
            RequestResult res = new RequestResult();
            try
            {
                string fileName = "GeneralSettings.json";
                Startup startup = new Startup(CacheManager.Cache);
                startup.SaveSetting(fileName, setting);

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

        [HttpPost("encryptPlaintText")]
        public async Task<ActionResult<object>> encryptPlaintText(string text)
        {
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            _logger.LogInformation($"Start {methodName} ");
            RequestResult res = new RequestResult();
            try
            {
                res.Response = PasswordEncryptor.EncryptString(text);

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


    }
}


// 