using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "AlertOccurrence", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class AlertOccurrence : IAlertOccurrence
{
	private Guid AlertIdField;

	private DateTime TimeField;

	[DataMember]
	public Guid AlertId
	{
		get
		{
			return AlertIdField;
		}
		set
		{
			AlertIdField = value;
		}
	}

	[DataMember]
	public DateTime Time
	{
		get
		{
			return TimeField;
		}
		set
		{
			TimeField = value;
		}
	}
}
