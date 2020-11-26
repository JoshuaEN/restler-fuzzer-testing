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
#if ENDPOINT_PUT_ONLYS
    [Route("api/v1/PutOnlys")]
    [ApiController]
    public class PutOnlyController : BaseInMemoryController<PutOnlyController, string, PutOnly>
    {
        public PutOnlyController(InMemoryStorageService<PutOnlyController, string, PutOnly> storageService) : base(InMemoryStorageMode.MimicPrivateAPI, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        [HttpGet]
        public IList<PutOnly> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="putOnlyId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{putOnlyId}")]
        public ActionResult<PutOnly> Get([BindRequired] string putOnlyId) => base._Get(putOnlyId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="putOnlyId" example="default"></param>
        /// <param name="putOnly"></param>
        /// <returns></returns>
        [HttpPut("{putOnlyId}")]
        public ActionResult<PutOnly> CreateOrUpdate([BindRequired] string putOnlyId, [FromBody, BindRequired] PutOnly putOnly) => base._CreateOrUpdate(putOnlyId, putOnly);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="putOnlyId" example="default"></param>
        /// <returns></returns>
        [HttpDelete("{putOnlyId}")]
        public ActionResult Delete([BindRequired] string putOnlyId) => base._Delete(putOnlyId);
    }
#endif
}
