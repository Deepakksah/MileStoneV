using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class GetRecorderStatusResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public AttachAndConnectionState GetRecorderStatusResult;

	public GetRecorderStatusResponseBody()
	{
	}

	public GetRecorderStatusResponseBody(AttachAndConnectionState GetRecorderStatusResult)
	{
		this.GetRecorderStatusResult = GetRecorderStatusResult;
	}
}
