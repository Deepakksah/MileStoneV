using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AudioMessageInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class AudioMessageInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DescriptionField;

	private Guid IdField;

	private string NameField;

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
}
