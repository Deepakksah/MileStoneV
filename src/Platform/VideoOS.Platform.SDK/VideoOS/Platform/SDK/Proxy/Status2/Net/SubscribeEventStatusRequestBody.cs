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
public class SubscribeEventStatusRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string token;

	[DataMember(Order = 1)]
	public Guid statusSessionId;

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public ArrayOfGuid eventIds;

	public SubscribeEventStatusRequestBody()
	{
	}

	public SubscribeEventStatusRequestBody(string token, Guid statusSessionId, ArrayOfGuid eventIds)
	{
		this.token = token;
		this.statusSessionId = statusSessionId;
		this.eventIds = eventIds;
	}
}
