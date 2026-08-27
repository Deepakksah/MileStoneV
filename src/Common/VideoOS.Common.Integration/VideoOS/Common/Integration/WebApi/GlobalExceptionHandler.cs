using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace VideoOS.Common.Integration.WebApi;

public class GlobalExceptionHandler : ExceptionHandler
{
	public override void Handle(ExceptionHandlerContext context)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (context != null)
		{
			context.Result = (IHttpActionResult)(object)new UnhandledExceptionResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
		}
	}
}
