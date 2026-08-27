using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MapPlugInItems", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server.WebService.Map")]
public class MapPlugInItems : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private MapPlugInItem[] ItemsField;

	private DateTime TimestampField;

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
	public MapPlugInItem[] Items
	{
		get
		{
			return ItemsField;
		}
		set
		{
			ItemsField = value;
		}
	}

	[DataMember]
	public DateTime Timestamp
	{
		get
		{
			return TimestampField;
		}
		set
		{
			TimestampField = value;
		}
	}
}
