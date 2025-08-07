using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Text;

namespace HealthCare.Cloud.AuthService.Controllers
{
    [Tags("Public Auth APIs")]
    [ApiController]
    [Route("auth/public")]
    public class PublicAuthController : ControllerBase
    {
        [HttpGet]
        [Route("getstatus")]
        public string GetAuthAPIStatus() => "Running"; 
    }
}
