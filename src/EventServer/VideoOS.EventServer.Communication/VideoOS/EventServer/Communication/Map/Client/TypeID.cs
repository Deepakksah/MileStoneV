using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TypeID", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class TypeID : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string IDField;

	private Guid ServerIDField;

	private Guid TypeField;

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
	public string ID
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
	public Guid ServerID
	{
		get
		{
			return ServerIDField;
		}
		set
		{
			ServerIDField = value;
		}
	}

	[DataMember]
	public Guid Type
	{
		get
		{
			return TypeField;
		}
		set
		{
			TypeField = value;
		}
	}
}
