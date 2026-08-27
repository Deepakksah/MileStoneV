using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class GetRecordingStorageStatusRequest
{
	[MessageBodyMember(Name = "GetRecordingStorageStatus", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2", Order = 0)]
	public GetRecordingStorageStatusRequestBody Body;

	public GetRecordingStorageStatusRequest()
	{
	}

	public GetRecordingStorageStatusRequest(GetRecordingStorageStatusRequestBody Body)
	{
		this.Body = Body;
	}
}
