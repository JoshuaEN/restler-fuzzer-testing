using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Authentication;
using Server.Models;
using Server.Services;
using Server.Utilities;

namespace Server.Controllers
{
    [Route("api/v1/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<string> GetAuthenticationTokenValue()
        {
            var formattedTimestamp = DateTime.UtcNow.AddMinutes(5).ToString(TimestampAuthenticationHandler.TimestampFormat, TimestampAuthenticationHandler.TimestampFormatProvider);
            var base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(formattedTimestamp));
            return Ok(base64Encoded);
        }
    }
}
