using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RestrictedMediaLive", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMediaLive : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid DeviceIdField;

	private DateTime LastModifiedField;

	private DateTime StartTimeField;

	private string UserNameField;

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
	public Guid DeviceId
	{
		get
		{
			return DeviceIdField;
		}
		set
		{
			DeviceIdField = value;
		}
	}

	[DataMember]
	public DateTime LastModified
	{
		get
		{
			return LastModifiedField;
		}
		set
		{
			LastModifiedField = value;
		}
	}

	[DataMember]
	public DateTime StartTime
	{
		get
		{
			return StartTimeField;
		}
		set
		{
			StartTimeField = value;
		}
	}

	[DataMember]
	public string UserName
	{
		get
		{
			return UserNameField;
		}
		set
		{
			UserNameField = value;
		}
	}
}
