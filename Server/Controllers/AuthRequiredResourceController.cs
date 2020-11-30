using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Server.Authentication;
using Server.Models;
using Server.Services;
using Server.Utilities;

namespace Server.Controllers
{
#if ENDPOINT_AUTHN
    [Route("api/v1/AuthRequiredResources")]
    [ApiController]
    [Authorize(AuthenticationSchemes = TimestampAuthenticationHandler.AuthenticationScheme, Roles = "Authed" )]
    public class AuthRequiredResourceController : BaseInMemoryController<AuthRequiredResourceController, string, AuthRequiredResource>
    {
        public AuthRequiredResourceController(InMemoryStorageService<AuthRequiredResourceController, string, AuthRequiredResource> storageService) : base(InMemoryStorageMode.MimicExpectedByRESTler, UniqueValueFunctions.GetUniqueString, storageService)
        {
        }

        [HttpGet]
        public IList<AuthRequiredResource> Index() => base._Index();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authRequiredResourceId" example="default"></param>
        /// <returns></returns>
        [HttpGet("{authRequiredResourceId}")]
        public ActionResult<AuthRequiredResource> Get([BindRequired] string authRequiredResourceId) => base._Get(authRequiredResourceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authRequiredResource"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AuthRequiredResource> Create([FromBody, BindRequired] AuthRequiredResource authRequiredResource) => base._Create(authRequiredResource);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authRequiredResourceId" example="default"></param>
        /// <param name="authRequiredResource"></param>
        /// <returns></returns>
        [HttpPut("{authRequiredResourceId}")]
        public ActionResult<AuthRequiredResource> CreateOrUpdate([BindRequired] string authRequiredResourceId, [FromBody, BindRequired] AuthRequiredResource authRequiredResource) => base._CreateOrUpdate(authRequiredResourceId, authRequiredResource);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authRequiredResourceId" example="default"></param>
        /// <returns></returns>
        [HttpDelete("{authRequiredResourceId}")]
        public ActionResult Delete([BindRequired] string authRequiredResourceId) => base._Delete(authRequiredResourceId);
    }
#endif
}
