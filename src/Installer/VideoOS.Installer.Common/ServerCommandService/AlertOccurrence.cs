using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlertOccurrence", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class AlertOccurrence : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid AlertIdField;

	private DateTime TimeField;

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
