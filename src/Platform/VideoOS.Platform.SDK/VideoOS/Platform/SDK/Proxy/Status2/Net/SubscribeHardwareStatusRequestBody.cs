using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class SubscribeHardwareStatusRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string token;

	[DataMember(Order = 1)]
	public Guid statusSessionId;

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public ArrayOfGuid hardwareIds;

	public SubscribeHardwareStatusRequestBody()
	{
	}

	public SubscribeHardwareStatusRequestBody(string token, Guid statusSessionId, ArrayOfGuid hardwareIds)
	{
		this.token = token;
		this.statusSessionId = statusSessionId;
		this.hardwareIds = hardwareIds;
	}
}
