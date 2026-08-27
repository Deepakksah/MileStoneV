using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class GetRecorderStatusRequest
{
	[MessageBodyMember(Name = "GetRecorderStatus", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Order = 0)]
	public GetRecorderStatusRequestBody Body;

	public GetRecorderStatusRequest()
	{
	}

	public GetRecorderStatusRequest(GetRecorderStatusRequestBody Body)
	{
		this.Body = Body;
	}
}
