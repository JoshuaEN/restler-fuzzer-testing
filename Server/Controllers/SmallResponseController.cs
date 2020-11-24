using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Models;
using Server.Services;
using Server.Utilities;

namespace Server.Controllers
{
#if ENDPOINT_SMALL_RESPONSES
    [Route("api/v1/SmallResponses")]
    [ApiController]
    public class SmallResponseController : ControllerBase
    {
        [HttpGet]
        public IList<SmallResponse> Index() => Array.Empty<SmallResponse>();

        [HttpPost]
        public ActionResult<SmallResponse> CreateOrUpdate([BindRequired] string smallResponseId, [FromBody, BindRequired] SmallResponse smallResponse) => Ok(new SmallResponse());
    }
#endif
}
