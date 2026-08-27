using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class GetMetadataDeviceStatisticsRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string token;

	[DataMember(EmitDefaultValue = false, Order = 1)]
	public ArrayOfGuid deviceIds;

	public GetMetadataDeviceStatisticsRequestBody()
	{
	}

	public GetMetadataDeviceStatisticsRequestBody(string token, ArrayOfGuid deviceIds)
	{
		this.token = token;
		this.deviceIds = deviceIds;
	}
}
