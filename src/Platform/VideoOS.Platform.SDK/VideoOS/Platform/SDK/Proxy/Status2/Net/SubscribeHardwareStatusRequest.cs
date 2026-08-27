using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class SubscribeHardwareStatusRequest
{
	[MessageBodyMember(Name = "SubscribeHardwareStatus", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Order = 0)]
	public SubscribeHardwareStatusRequestBody Body;

	public SubscribeHardwareStatusRequest()
	{
	}

	public SubscribeHardwareStatusRequest(SubscribeHardwareStatusRequestBody Body)
	{
		this.Body = Body;
	}
}
