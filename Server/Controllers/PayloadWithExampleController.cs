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

    [Route("api/v1/PayloadWithExamples")]
    [ApiController]
    public class PayloadWithExampleController : ControllerBase
    {
        [HttpPost]
        [RestlerExample(title: "Test", nameof(IndexExample))]
        [RestlerExample(title: "Test 2", nameof(IndexExample2))]
        public ActionResult Index([FromBody] Payload payload) => NoContent();

        private static object IndexExample()
        {
            return new
            {
                body = new
                {
                    testType = PayloadEnum.Orange
                }
            };
        }
        private static object IndexExample2()
        {
            return new
            {
                body = new
                {
                    testType = PayloadEnum.Lemon
                }
            };
        }
    }
#endif
}
