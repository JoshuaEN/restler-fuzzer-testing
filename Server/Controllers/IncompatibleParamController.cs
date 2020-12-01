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
#if ENDPOINT_INCOMPAT_PARAMS
    [Route("api/v1/IncompatibleParams")]
    [ApiController]
    public class IncompatibleParamController : BaseInMemoryController<IncompatibleParamController, string, IncompatibleParam>
    {
        public IncompatibleParamController(InMemoryStorageService<IncompatibleParamController, string, IncompatibleParam> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        [HttpGet]
        public IList<IncompatibleParam> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incompatibleParamId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{incompatibleParamId}")]
        public ActionResult<IncompatibleParam> Get([BindRequired] string incompatibleParamId) => base._Get(incompatibleParamId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incompatibleParam"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<IncompatibleParam> Create([FromBody, BindRequired] IncompatibleParam incompatibleParam)
        {
            if (incompatibleParam.CanFly)
            {
                if (incompatibleParam.MaxHeight.HasValue != true || incompatibleParam.MaxHeight < 1)
                {
                    return BadRequest("If CanFly is true, MaxHeight must be a finite number > 0");
                }
            }
            else if (incompatibleParam.MaxHeight != null)
            {
                return BadRequest("If CanFly is false, MaxHeight must be null or excluded");
            }

            if (incompatibleParam.CanSwim)
            {
                if (incompatibleParam.MaxDepth.HasValue != true || incompatibleParam.MaxDepth < 1)
                {
                    return BadRequest("If CanSwim is true, MaxDepth must be a finite number > 0");
                }
            }
            else if (incompatibleParam.MaxDepth != null)
            {
                return BadRequest("If CanSwim is false, MaxDepth must be null or excluded");
            }

            if (incompatibleParam.CanFly == incompatibleParam.CanSwim)
            {
                return BadRequest("Both CanFly and CanSwim cannot be set to the same value");
            }

            return base._Create(incompatibleParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incompatibleParamId" example="default"></param>
        /// <returns></returns>
        [HttpDelete("{incompatibleParamId}")]
        public ActionResult Delete([BindRequired] string incompatibleParamId) => base._Delete(incompatibleParamId);
    }
#endif
}
