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
public class SubscribeConfigurationStatusRequestBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public string token;

	[DataMember(Order = 1)]
	public Guid statusSessionId;

	[DataMember(Order = 2)]
	public bool subscribe;

	public SubscribeConfigurationStatusRequestBody()
	{
	}

	public SubscribeConfigurationStatusRequestBody(string token, Guid statusSessionId, bool subscribe)
	{
		this.token = token;
		this.statusSessionId = statusSessionId;
		this.subscribe = subscribe;
	}
}
