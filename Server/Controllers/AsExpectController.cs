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
#if ENDPOINT_AS_EXPECTED
    [Route("api/v1/AsExpects")]
    [ApiController]
    public class AsExpectController : BaseInMemoryController<AsExpectController, string, AsExpect>
    {
        public AsExpectController(InMemoryStorageService<AsExpectController, string, AsExpect> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        [HttpGet]
        public IList<AsExpect> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asExpectId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{asExpectId}")]
        public ActionResult<AsExpect> Get([BindRequired] string asExpectId) => base._Get(asExpectId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asExpect"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AsExpect> Create([FromBody, BindRequired] AsExpect asExpect) => base._Create(asExpect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asExpectId" example="default"></param>
        /// <param name="asExpect"></param>
        /// <returns></returns>
        [HttpPut("{asExpectId}")]
        public ActionResult<AsExpect> CreateOrUpdate([BindRequired] string asExpectId, [FromBody, BindRequired] AsExpect asExpect) => base._CreateOrUpdate(asExpectId, asExpect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asExpectId" example="default"></param>
        /// <param name="asExpect"></param>
        /// <returns></returns>
        [HttpDelete("{asExpectId}")]
        public ActionResult Delete([BindRequired] string asExpectId) => base._Delete(asExpectId);
    }
#endif
}
