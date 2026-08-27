using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using VideoOS.Management.Common;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Server;

public class ServerConnector : IHttpHandler, IWebServiceManagerExceptionHandler
{
	public bool IsReusable => false;

	public void ProcessRequest(HttpContext context)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		string token = context.Request.Params["token"];
		string userHostAddress = context.Request.UserHostAddress;
		string password = string.Empty;
		string arg;
		using (ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(this, int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"), CultureInfo.InvariantCulture)))
		{
			arg = serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.ValidateToken(out password, token, userHostAddress), null, (Exception ex, Action ac) => false);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		stringBuilder.AppendLine("<methodresponse>");
		stringBuilder.AppendFormat("<user>{0}</user>{1}", arg, Environment.NewLine);
		stringBuilder.AppendFormat("<pass>{0}</pass>{1}", password, Environment.NewLine);
		stringBuilder.AppendLine("</methodresponse>");
		context.Response.ContentType = "text/xml";
		context.Response.ContentEncoding = Encoding.UTF8;
		context.Response.BinaryWrite(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
	}

	public bool HandleException(Exception ex, Action exceptionTraceCall)
	{
		return true;
	}
}
