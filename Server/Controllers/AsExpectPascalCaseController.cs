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
#if ENDPOINT_AS_EXPECT_PASCAL_CASES
    [Route("Api/V1/AsExpectPascalCases")]
    [ApiController]
    public class AsExpectPascalCaseController : BaseInMemoryController<AsExpectPascalCaseController, string, AsExpectPascalCase>
    {
        public AsExpectPascalCaseController(InMemoryStorageService<AsExpectPascalCaseController, string, AsExpectPascalCase> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        [HttpGet]
        public IList<AsExpectPascalCase> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AsExpectPascalCaseId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{AsExpectPascalCaseId}")]
        // [ResterAnnotation(producerEndpoint: "/Api/V1/AsExpectPascalCases", producerMethod: "POST", producerResourceName: "Id", consumerParam: "AsExpectPascalCaseId")]
        public ActionResult<AsExpectPascalCase> Get([BindRequired] string AsExpectPascalCaseId) => base._Get(AsExpectPascalCaseId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AsExpectPascalCase"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AsExpectPascalCase> Create([FromBody, BindRequired] AsExpectPascalCase AsExpectPascalCase) => base._Create(AsExpectPascalCase);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AsExpectPascalCaseId" example="default"></param>
        /// <param name="AsExpectPascalCase"></param>
        /// <returns></returns>
        [HttpPut("{AsExpectPascalCaseId}")]
        public ActionResult<AsExpectPascalCase> CreateOrUpdate([BindRequired] string AsExpectPascalCaseId, [FromBody, BindRequired] AsExpectPascalCase AsExpectPascalCase) => base._CreateOrUpdate(AsExpectPascalCaseId, AsExpectPascalCase);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AsExpectPascalCaseId" example="default"></param>
        /// <returns></returns>
        [HttpDelete("{AsExpectPascalCaseId}")]
        // [ResterAnnotation(producerEndpoint: "/Api/V1/AsExpectPascalCases", producerMethod: "POST", producerResourceName: "Id", consumerParam: "AsExpectPascalCaseId")]
        public ActionResult Delete([BindRequired] string AsExpectPascalCaseId) => base._Delete(AsExpectPascalCaseId);
    }
#endif
}
