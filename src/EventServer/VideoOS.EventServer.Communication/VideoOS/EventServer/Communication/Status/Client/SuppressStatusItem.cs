using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SuppressStatusItem", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class SuppressStatusItem : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string CommentField;

	private Guid SourceIdField;

	private string SourceNameField;

	private Guid SuppressStatusItemIdField;

	private DateTime TimeField;

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
	public string Comment
	{
		get
		{
			return CommentField;
		}
		set
		{
			CommentField = value;
		}
	}

	[DataMember]
	public Guid SourceId
	{
		get
		{
			return SourceIdField;
		}
		set
		{
			SourceIdField = value;
		}
	}

	[DataMember]
	public string SourceName
	{
		get
		{
			return SourceNameField;
		}
		set
		{
			SourceNameField = value;
		}
	}

	[DataMember]
	public Guid SuppressStatusItemId
	{
		get
		{
			return SuppressStatusItemIdField;
		}
		set
		{
			SuppressStatusItemIdField = value;
		}
	}

	[DataMember]
	public DateTime Time
	{
		get
		{
			return TimeField;
		}
		set
		{
			TimeField = value;
		}
	}
}
