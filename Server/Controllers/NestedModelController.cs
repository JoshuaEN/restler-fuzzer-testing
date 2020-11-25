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
#if ENDPOINT_NESTED_MODELS
    [Route("api/v1/NestedModels")]
    [ApiController]
    public class NestedModelController : BaseInMemoryController<NestedModelController, string, NestedModel>
    {
        public NestedModelController(InMemoryStorageService<NestedModelController, string, NestedModel> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        [HttpGet]
        public IList<NestedModel> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nestedModelId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{nestedModelId}")]
        public ActionResult<NestedModel> Get([BindRequired] string nestedModelId) => base._Get(nestedModelId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nestedModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<NestedModel> Create([FromBody, BindRequired] NestedModel nestedModel) => base._Create(nestedModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nestedModelId" example="default"></param>
        /// <param name="nestedModel"></param>
        /// <returns></returns>
        [HttpPut("{nestedModelId}")]
        public ActionResult<NestedModel> CreateOrUpdate([BindRequired] string nestedModelId, [FromBody, BindRequired] NestedModel nestedModel) => base._CreateOrUpdate(nestedModelId, nestedModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nestedModelId" example="default"></param>
        /// <param name="nestedModel"></param>
        /// <returns></returns>
        [HttpDelete("{nestedModelId}")]
        public ActionResult Delete([BindRequired] string nestedModelId) => base._Delete(nestedModelId);
    }
#endif
}
