using System.Net.Http;
using System.Text;
using System.Web.Http.ExceptionHandling;
using VideoOS.Common.Integration.WebApi;

namespace VideoOS.Management.Server.RestApi;

public class RestApiExceptionHandler : ExceptionHandler
{
	public override void Handle(ExceptionHandlerContext context)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if (context != null)
		{
			ErrorModel errorModel = ExceptionHandling.ConvertToErrorModel(context.Exception);
			StringContent content = new StringContent(errorModel.Serialize(), Encoding.UTF8, "application/json");
			context.Result = new UnhandledExceptionResult(new HttpResponseMessage(errorModel.HttpCode)
			{
				Content = (HttpContent)(object)content
			});
		}
	}
}
