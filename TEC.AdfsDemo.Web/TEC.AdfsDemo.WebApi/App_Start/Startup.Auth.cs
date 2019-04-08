using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using TEC.AdfsDemo.WebApi.Enums;
using TEC.AdfsDemo.WebApi.Exceptions;
using TEC.AdfsDemo.WebApi.Models;
using TEC.Core.Web.WebApi.Common.Response;
using TEC.Internal.Utils.Core.Security;

namespace TEC.AdfsDemo.WebApi
{
    public partial class Startup
    {
        /// <summary>
        /// 設定 OAuth
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureOAuth(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(OAuthConfigUtil.OAuthBearerAuthenticationOptions);
            Internal.Utils.WebApi.Attributes.OAuthAuthorizeAttribute.ValidationFailedAction = (exception, actionContext) =>
            {
                OperationFailedException operationFailedException = new OperationFailedException(ResultCodeSettingEnum.AuthorizationFailed);
                ExceptionResponse<ResultCodeSettingEnum> exceptionResponse = operationFailedException.getResponse();
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, exceptionResponse);
            };
        }
    }
}