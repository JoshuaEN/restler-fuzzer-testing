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
#if ENDPOINT_POST_ON_RESOURCES
    [Route("api/v1/PostOnResources")]
    [ApiController]
    public class PostOnResourcesController : BaseInMemoryController<string, PostOnResource>
    {
        [HttpGet]
        public IList<PostOnResource> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postOnResourceId" example="default"></param>
        /// <param name="postOnResource"></param>
        /// <returns></returns>
        [HttpPost("{postOnResourceId}")]
        public ActionResult<PostOnResource> GetOrCreate([BindRequired] string postOnResourceId, [FromBody, BindRequired] PostOnResource postOnResource) => base._GetOrCreate(postOnResourceId, postOnResource);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postOnResourceId" example="default"></param>
        /// <param name="postOnResource"></param>
        /// <returns></returns>
        [HttpGet("{postOnResourceId}")]
        public ActionResult<PostOnResource> Get([BindRequired] string postOnResourceId) => base._Get(postOnResourceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postOnResourceId" example="default"></param>
        /// <param name="postOnResource"></param>
        /// <returns></returns>
        [HttpPut("{postOnResourceId}")]
        public ActionResult<PostOnResource> CreateOrUpdate([BindRequired] string postOnResourceId, [FromBody, BindRequired] PostOnResource postOnResource) => base._CreateOrUpdate(postOnResourceId, postOnResource);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postOnResourceId" example="default"></param>
        /// <param name="postOnResource"></param>
        /// <returns></returns>
        [HttpDelete("{postOnResourceId}")]
        public ActionResult Delete([BindRequired] string postOnResourceId) => base._Delete(postOnResourceId);
    }
#endif
}
