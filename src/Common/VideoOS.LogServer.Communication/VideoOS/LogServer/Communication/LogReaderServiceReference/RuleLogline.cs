using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RuleLogline", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class RuleLogline : LoglineBase
{
	[OptionalField]
	private string EventTypeField;

	[OptionalField]
	private string RuleNameField;

	[OptionalField]
	private string ServiceNameField;

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
	public string RuleName
	{
		get
		{
			return RuleNameField;
		}
		set
		{
			if ((object)RuleNameField != value)
			{
				RuleNameField = value;
				RaisePropertyChanged("RuleName");
			}
		}
	}

	[DataMember]
	public string ServiceName
	{
		get
		{
			return ServiceNameField;
		}
		set
		{
			if ((object)ServiceNameField != value)
			{
				ServiceNameField = value;
				RaisePropertyChanged("ServiceName");
			}
		}
	}
}
