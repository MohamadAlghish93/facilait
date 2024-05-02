
using Microsoft.AspNetCore.Mvc;
using FacilaIT.Models;
using FacilaIT.Models.RABC;
using Microsoft.AspNetCore.Authorization;
using FacilaIT.Business;


namespace FacilaIT.Controllers
{
    [Authorize(Roles = UserRoles.ServicePage)]
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly DBBContext _context;
        private readonly ILogger _logger;

        public NotifyController(DBBContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory
                  .CreateLogger<NotifyController>();
        }

        // [HttpPost("Hook")]
        // public async Task<ActionResult<object>> Hook(ServiceResult serviceResult)
        // {
        //     //
        //     string methodName = ControllerContext.ActionDescriptor.ActionName;
        //     _logger.LogInformation($"Start {methodName} ");
        //     RequestResult res = new RequestResult();
        //     try
        //     {
        //         NotifyBusiness notify = new NotifyBusiness();
        //         notify.Process(serviceResult.Message);

        //         res.Response = "Success";
        //     }
        //     catch (System.Exception ex)
        //     {
        //         res.IsSuccess = false;
        //         res.Message = ex.Message;
        //         _logger.LogError(ex.Message);
        //         _logger.LogError(ex.StackTrace);
        //     }
        //     _logger.LogInformation($"End {methodName} {res.IsSuccess}");

        //     return res;
        //     // // //
        // }

    }
}
