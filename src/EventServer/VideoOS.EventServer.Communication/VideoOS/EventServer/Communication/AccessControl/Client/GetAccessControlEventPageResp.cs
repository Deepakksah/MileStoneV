using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetAccessControlEventPageResp", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
public class GetAccessControlEventPageResp : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private AccessControlEventLine[] ResultField;

	private Guid SearchIdField;

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
	public AccessControlEventLine[] Result
	{
		get
		{
			return ResultField;
		}
		set
		{
			ResultField = value;
		}
	}

	[DataMember]
	public Guid SearchId
	{
		get
		{
			return SearchIdField;
		}
		set
		{
			SearchIdField = value;
		}
	}
}
