using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TimeProfileDefinition", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class TimeProfileDefinition : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DefinitionField;

	private string DescriptionField;

	private Guid IdField;

	private string NameField;

	private string PathField;

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
	public string Definition
	{
		get
		{
			return DefinitionField;
		}
		set
		{
			DefinitionField = value;
		}
	}

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
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
	public string Path
	{
		get
		{
			return PathField;
		}
		set
		{
			PathField = value;
		}
	}
}
