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
#if ENDPOINT_PRECREATED
    [Route("api/v1/PreCreateSubResources")]
    [ApiController]
    public class PreCreateSubResourceController : BaseInMemoryController<PreCreateSubResourceController, string, PreCreateSubResource>
    {
        public PreCreateSubResourceController(InMemoryStorageService<PreCreateSubResourceController, string, PreCreateSubResource> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{webId}")]
        [ResterAnnotation(producerEndpoint: "/api/1/PreCreates/{webId}", producerMethod: "POST", producerResourceName: "id", consumerParam: "webId")]
        public ActionResult<PreCreateSubResource> Get([BindRequired] string webId) => base._Get(webId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="webId" example="default"></param>
        /// <returns></returns>
        [HttpDelete("{webId}")]
        [ResterAnnotation(producerEndpoint: "/api/v1/PreCreates/{webId}", producerMethod: "POST", producerResourceName: "id", consumerParam: "webId")]
        public ActionResult Delete([BindRequired] string webId) => base._Delete(webId);
    }
#endif
}
