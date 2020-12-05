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

    [Route("api/v1/PreCreates")]
    [ApiController]
    public class PreCreateController : BaseInMemoryController<PreCreateSubResourceController, string, PreCreateSubResource>
    {
        private InMemoryStorageService<PreCreateController, string, PreCreate> thisStorageService;
        public PreCreateController(
            InMemoryStorageService<PreCreateController, string, PreCreate> thisStorageService,
            InMemoryStorageService<PreCreateSubResourceController, string, PreCreateSubResource> storageService
            ) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
            this.thisStorageService = thisStorageService;
            var exist1 = new PreCreate() { Id = "exist1", Name = "Pre-existing Item 1" };
            thisStorageService.GetStorage().GetOrAdd(exist1.InternalIdentifier, exist1);
            var exist2 = new PreCreate() { Id = "exist2", Name = "Pre-existing Item 2" };
        }

        [HttpGet]
        public IList<PreCreate> Index() => this.thisStorageService.GetStorage().Values.ToList();

        [HttpGet("{webId}")]
        public ActionResult<PreCreate> Get([BindRequired] string webId)
        {
            if (this.thisStorageService.GetStorage().TryGetValue(webId, out PreCreate value))
            {
                return value;
            }
            return base.NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preCreateSubResource"></param>
        /// <returns></returns>
        [HttpPost("{webId}")]
        public ActionResult<PreCreateSubResource> Create([BindRequired] string webId, [FromBody, BindRequired] PreCreateSubResource preCreateSubResource)
        {
            if (this.thisStorageService.GetStorage().ContainsKey(webId) != true)
            {
                return base.NotFound();
            }
            preCreateSubResource.PreCreateId = webId;

            return base._Create(preCreateSubResource);
        }

    }
#endif
}
