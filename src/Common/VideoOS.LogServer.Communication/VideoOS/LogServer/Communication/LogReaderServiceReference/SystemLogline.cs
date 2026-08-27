using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SystemLogline", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class SystemLogline : LoglineBase
{
	[OptionalField]
	private string EventTypeField;

	[OptionalField]
	private LogLevel LogLevelField;

	[DataMember]
	public string EventType
	{
		get
		{
			return EventTypeField;
		}
		set
		{
			if ((object)EventTypeField != value)
			{
				EventTypeField = value;
				RaisePropertyChanged("EventType");
			}
		}
	}

	[DataMember]
	public LogLevel LogLevel
	{
		get
		{
			return LogLevelField;
		}
		set
		{
			if (!LogLevelField.Equals(value))
			{
				LogLevelField = value;
				RaisePropertyChanged("LogLevel");
			}
		}
	}
}
