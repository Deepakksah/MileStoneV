using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LookupItemResp", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class LookupItemResp : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool ItemExistsField;

	private FQID ItemFqidField;

	private string ItemNameField;

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
	public bool ItemExists
	{
		get
		{
			return ItemExistsField;
		}
		set
		{
			ItemExistsField = value;
		}
	}

	[DataMember]
	public FQID ItemFqid
	{
		get
		{
			return ItemFqidField;
		}
		set
		{
			ItemFqidField = value;
		}
	}

	[DataMember]
	public string ItemName
	{
		get
		{
			return ItemNameField;
		}
		set
		{
			ItemNameField = value;
		}
	}
}
