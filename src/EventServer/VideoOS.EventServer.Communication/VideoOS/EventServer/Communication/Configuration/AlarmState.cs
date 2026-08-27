using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmState", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class AlarmState : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid IdField;

	private string NameField;

	private int NumberField;

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
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public int Number
	{
		get
		{
			return NumberField;
		}
		set
		{
			NumberField = value;
		}
	}
}
