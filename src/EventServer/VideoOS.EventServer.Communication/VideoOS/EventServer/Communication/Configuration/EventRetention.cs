using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventRetention", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class EventRetention : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid IdField;

	private bool IsReadOnlyField;

	private int RetentionDaysField;

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
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public bool IsReadOnly
	{
		get
		{
			return IsReadOnlyField;
		}
		set
		{
			IsReadOnlyField = value;
		}
	}

	[DataMember]
	public int RetentionDays
	{
		get
		{
			return RetentionDaysField;
		}
		set
		{
			RetentionDaysField = value;
		}
	}
}
