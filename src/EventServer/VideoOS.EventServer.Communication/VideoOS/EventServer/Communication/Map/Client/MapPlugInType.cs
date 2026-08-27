using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MapPlugInType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server.WebService.Map")]
public class MapPlugInType : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid DefaultIconField;

	private string DescriptionField;

	private Guid IDField;

	private string NameField;

	private Guid ParentIdField;

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
	public Guid DefaultIcon
	{
		get
		{
			return DefaultIconField;
		}
		set
		{
			DefaultIconField = value;
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
	public Guid ParentId
	{
		get
		{
			return ParentIdField;
		}
		set
		{
			ParentIdField = value;
		}
	}
}
