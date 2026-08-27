using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

public sealed class ServerExceptionFilter : ExceptionFilterAttribute
{
	public override void OnException(HttpActionExecutedContext actionExecutedContext)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		if (actionExecutedContext == null)
		{
			throw new ArgumentNullException("actionExecutedContext");
		}
		if (actionExecutedContext.Exception is NotSupportedByLegacyRecorderException)
		{
			actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
		}
	}
}
