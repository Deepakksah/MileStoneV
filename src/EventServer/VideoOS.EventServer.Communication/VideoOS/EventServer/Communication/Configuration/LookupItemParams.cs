using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LookupItemParams", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class LookupItemParams : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid ItemIdField;

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
	public Guid ItemId
	{
		get
		{
			return ItemIdField;
		}
		set
		{
			ItemIdField = value;
		}
	}
}
