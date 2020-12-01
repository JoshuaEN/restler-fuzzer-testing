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
    public class PayloadController : BaseInMemoryController<PayloadController, string, Payload>
    {
        public PayloadController(InMemoryStorageService<PayloadController, string, Payload> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        [HttpGet]
        [RestlerExample(title: "Test", nameof(IndexExample))]
        public IList<Payload> Index([FromBody] Payload payload) => base._Index();

        private static object IndexExample()
        {
            return new
            {
                body = new
                {
                    TestString = "Hello",
                    TestType = PayloadEnum.Orange
                }
            };
        }
    }
#endif
}
