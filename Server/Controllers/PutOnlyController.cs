using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Models;

namespace Server.Controllers
{
#if ENDPOINT_PUT_ONLYS
    [Route("api/v1/PutOnlys")]
    [ApiController]
    public class PutOnlysController : BaseInMemoryController<string, PutOnly>
    {
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
        /// <param name="putOnly"></param>
        /// <returns></returns>
        [HttpDelete("{putOnlyId}")]
        public ActionResult Delete([BindRequired] string putOnlyId) => base._Delete(putOnlyId);
    }
#endif
}
