using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SubscribeConfigurationStatusRequest
{
	[MessageBodyMember(Name = "SubscribeConfigurationStatus", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Order = 0)]
	public SubscribeConfigurationStatusRequestBody Body;

	public SubscribeConfigurationStatusRequest()
	{
	}

	public SubscribeConfigurationStatusRequest(SubscribeConfigurationStatusRequestBody Body)
	{
		this.Body = Body;
	}
}
