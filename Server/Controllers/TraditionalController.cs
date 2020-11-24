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
#if ENDPOINT_TRADITIONALS
    [Route("api/v1/Traditionals")]
    [ApiController]
    public class TraditionalsController : BaseInMemoryController<string, Traditional>
    {
        [HttpGet]
        public IList<Traditional> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traditionalId" example="default"></param>
        /// <param name="traditional"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Traditional> Create([BindRequired] string traditionalId, [FromBody, BindRequired] Traditional traditional) => base._Create(traditionalId, traditional);

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
