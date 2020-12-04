using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Attributes;
using Server.Models;
using Server.Services;
using Server.Utilities;

namespace Server.Controllers
{
#if ENDPOINT_PAYLOADS

    [Route("api/v1/Payloads")]
    [ApiController]
    public class PayloadController : ControllerBase
    {
        [HttpPost]
        public AlphabetBody Index([FromBody] AlphabetBody alphabet) => alphabet;
    }
#endif
}
