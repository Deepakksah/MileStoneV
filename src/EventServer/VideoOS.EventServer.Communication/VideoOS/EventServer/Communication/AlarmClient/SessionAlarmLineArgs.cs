using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AlarmClient;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SessionAlarmLineArgs", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class SessionAlarmLineArgs : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid SessionIdField;

	private int MaxCountField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public Guid SessionId
	{
		get
		{
			return SessionIdField;
		}
		set
		{
			SessionIdField = value;
		}
	}

	[DataMember]
	public int MaxCount
	{
		get
		{
			return MaxCountField;
		}
		set
		{
			MaxCountField = value;
		}
	}
}
