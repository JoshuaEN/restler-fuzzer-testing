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
#if ENDPOINT_RECURSIVE_MODELS
    [Route("api/v1/RecursiveModels")]
    [ApiController]
    public class RecursiveModelController : BaseInMemoryController<RecursiveModelController, string, RecursiveModel>
    {
        public RecursiveModelController(InMemoryStorageService<RecursiveModelController, string, RecursiveModel> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        [HttpGet]
        public IList<RecursiveModel> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recursiveModelId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{recursiveModelId}")]
        public ActionResult<RecursiveModel> Get([BindRequired] string recursiveModelId) => base._Get(recursiveModelId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recursiveModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<RecursiveModel> Create([FromBody, BindRequired] RecursiveModel recursiveModel) => base._Create(recursiveModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recursiveModelId" example="default"></param>
        /// <param name="recursiveModel"></param>
        /// <returns></returns>
        [HttpPut("{recursiveModelId}")]
        public ActionResult<RecursiveModel> CreateOrUpdate([BindRequired] string recursiveModelId, [FromBody, BindRequired] RecursiveModel recursiveModel) => base._CreateOrUpdate(recursiveModelId, recursiveModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recursiveModelId" example="default"></param>
        /// <param name="recursiveModel"></param>
        /// <returns></returns>
        [HttpDelete("{recursiveModelId}")]
        public ActionResult Delete([BindRequired] string recursiveModelId) => base._Delete(recursiveModelId);
    }
#endif
}
