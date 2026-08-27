using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MapPlugIn", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server.WebService.Map")]
public class MapPlugIn : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid IDField;

	private byte[] IconField;

	private Guid KindField;

	private MapPlugInType[] MapPlugInTypesField;

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
	public Guid ID
	{
		get
		{
			return IDField;
		}
		set
		{
			IDField = value;
		}
	}

	[DataMember]
	public byte[] Icon
	{
		get
		{
			return IconField;
		}
		set
		{
			IconField = value;
		}
	}

	[DataMember]
	public Guid Kind
	{
		get
		{
			return KindField;
		}
		set
		{
			KindField = value;
		}
	}

	[DataMember]
	public MapPlugInType[] MapPlugInTypes
	{
		get
		{
			return MapPlugInTypesField;
		}
		set
		{
			MapPlugInTypesField = value;
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
