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

namespace Server.Controllers
{
#if ENDPOINT_TRADITIONALS
    [Route("api/v1/Traditionals")]
    [ApiController]
    public class TraditionalController : BaseInMemoryController<TraditionalController, string, Traditional>
    {
        public TraditionalController(InMemoryStorageService<TraditionalController, string, Traditional> storageService) : base(storageService)
        {
        }

        [HttpGet]
        public IList<Traditional> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traditional"></param>
        /// <returns></returns>
        [HttpPost]
        [ResterAnnotationAttribute(producerResourceName: "restler_fuzzable_uuid4", consumerParam: "id")]
        public ActionResult<Traditional> Create([FromBody, BindRequired] Traditional traditional) => base._Create(traditional);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traditionalId" example="default"></param>
        /// <param name="traditional"></param>
        /// <returns></returns>
        [HttpGet("{traditionalId}")]
        public ActionResult<Traditional> Get([BindRequired] string traditionalId) => base._Get(traditionalId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traditionalId" example="default"></param>
        /// <param name="traditional"></param>
        /// <returns></returns>
        [HttpPut("{traditionalId}")]
        public ActionResult<Traditional> CreateOrUpdate([BindRequired] string traditionalId, [FromBody, BindRequired] Traditional traditional) => base._CreateOrUpdate(traditionalId, traditional);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traditionalId" example="default"></param>
        /// <param name="traditional"></param>
        /// <returns></returns>
        [HttpDelete("{traditionalId}")]
        public ActionResult Delete([BindRequired] string traditionalId) => base._Delete(traditionalId);
    }
#endif
}
