using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "SCHmessage", Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
public class SCHmessage : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private DateTime CreatedField;

	private SCHmessageDataItem[] DataItemsField;

	private int MessageIDField;

	private string MsgTypeNameField;

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
	public DateTime Created
	{
		get
		{
			return CreatedField;
		}
		set
		{
			CreatedField = value;
		}
	}

	[DataMember]
	public SCHmessageDataItem[] DataItems
	{
		get
		{
			return DataItemsField;
		}
		set
		{
			DataItemsField = value;
		}
	}

	[DataMember]
	public int MessageID
	{
		get
		{
			return MessageIDField;
		}
		set
		{
			MessageIDField = value;
		}
	}

	[DataMember]
	public string MsgTypeName
	{
		get
		{
			return MsgTypeNameField;
		}
		set
		{
			MsgTypeNameField = value;
		}
	}
}
