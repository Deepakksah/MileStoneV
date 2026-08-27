using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace VideoOS.Management.Server;

public class GeminiDispatchMessageInspector : IDispatchMessageInspector
{
	private GeminiRequestContext _requestContext;

	private const string Area = "GeminiDispatchMessageInspector";

	private const string Layer = "WebService";

	public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
	{
		GeminiTrace.Verbose("Entering IDispatchMessageInspector.AfterReceiveRequest");
		_requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateContext(RequestContextType.UserContext, HostRequestType.Wcf, OperationContext.Current);
		_requestContext.RequestTracer.TraceEnter(0, "GeminiDispatchMessageInspector", "WebService", "AfterReceiveRequest");
		HttpContext.Current.Items["WcfRequestContext"] = _requestContext;
		return null;
	}

	public void BeforeSendReply(ref Message reply, object correlationState)
	{
		GeminiTrace.Verbose("Entering IDispatchMessageInspector.BeforeSendReply");
	}
}
