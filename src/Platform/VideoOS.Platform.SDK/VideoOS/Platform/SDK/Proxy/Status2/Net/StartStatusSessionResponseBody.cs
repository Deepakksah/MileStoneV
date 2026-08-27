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
public class StartStatusSessionResponseBody
{
	[DataMember(Order = 0)]
	public Guid StartStatusSessionResult;

	public StartStatusSessionResponseBody()
	{
	}

	public StartStatusSessionResponseBody(Guid StartStatusSessionResult)
	{
		this.StartStatusSessionResult = StartStatusSessionResult;
	}
}
