using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class GetArchiveStorageStatusResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public StorageStatus[] GetArchiveStorageStatusResult;

	public GetArchiveStorageStatusResponseBody()
	{
	}

	public GetArchiveStorageStatusResponseBody(StorageStatus[] GetArchiveStorageStatusResult)
	{
		this.GetArchiveStorageStatusResult = GetArchiveStorageStatusResult;
	}
}
