using Microsoft.AspNetCore.Mvc;



namespace FacilaIT.Controllers
{
    //[Authorize(Roles = UserRoles.User)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProxySController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly FacilaIT.Models.DBBContext _context;

        // private readonly RbacService _rbacService;

        public ProxySController(FacilaIT.Models.DBBContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory
                   .CreateLogger<ProxySController>();
            // _rbacService = rbacService;
        }



        [HttpPost]
        public async Task<ActionResult<object>> Service(FacilaIT.Models.ProxyItem proxyItem)
        {
            //
            string methodName = ControllerContext.ActionDescriptor.ActionName;
            _logger.LogInformation($"Start {methodName} ");
            RequestResult res = new RequestResult();
            string response = null;
            FacilaIT.Business.ProxyBusiness pb = new FacilaIT.Business.ProxyBusiness();
            try
            {
                switch (proxyItem.Method)
                {
                    case "POST":
                        // Call the method to make the POST request with the JWT token
                        response = await pb.PostAsync(proxyItem.URI, proxyItem.PostPayload, proxyItem.Token, proxyItem.Header);

                        break;
                    case "PUT":
                        // Call the method to make the POST request with the JWT token
                        response = await pb.PutAsync(proxyItem.URI, proxyItem.PostPayload, proxyItem.Token, proxyItem.Header);

                        break;
                    case "GET":
                        // Call the method to make the GET request
                        response = await pb.GetAsync(proxyItem.URI, proxyItem.Token);

                        break;
                    case "DEL":
                        // Call the method to make the GET request
                        response = await pb.DeleteAsync(proxyItem.URI, proxyItem.Token);

                        break;
                    default:
                        break;
                }

                if (response != null)
                {
                    // Display the response if successful
                    res.Response = (response);
                }
                else
                {
                    // Handle the error, if any
                    res.Response = ("Failed to retrieve data.");
                }

                // res.Response = // CreatedAtAction("GetServiceItem", new { id = ServiceItem.Id }, ServiceItem);

                // res.Response = logAnalyzerLines;
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
            // // //


        }
    }
}