using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TrackInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class TrackInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool EdgeField;

	private Guid TrackIdField;

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
	public bool Edge
	{
		get
		{
			return EdgeField;
		}
		set
		{
			EdgeField = value;
		}
	}

	[DataMember]
	public Guid TrackId
	{
		get
		{
			return TrackIdField;
		}
		set
		{
			TrackIdField = value;
		}
	}
}
