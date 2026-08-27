using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "StreamInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class StreamInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool DefaultField;

	private string NameField;

	private Guid StreamIdField;

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
	public bool Default
	{
		get
		{
			return DefaultField;
		}
		set
		{
			DefaultField = value;
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
	public Guid StreamId
	{
		get
		{
			return StreamIdField;
		}
		set
		{
			StreamIdField = value;
		}
	}
}
