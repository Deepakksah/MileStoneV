using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RestrictedMedia", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMedia : IExtensibleDataObject, IRestrictedMedia
{
	private ExtensionDataObject extensionDataField;

	private string DescriptionField;

	private Guid[] DeviceIdsField;

	private DateTime EndTimeField;

	private string HeaderField;

	private Guid IdField;

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
	public Guid[] DeviceIds
	{
		get
		{
			return DeviceIdsField;
		}
		set
		{
			DeviceIdsField = value;
		}
	}

	[DataMember]
	public DateTime EndTime
	{
		get
		{
			return EndTimeField;
		}
		set
		{
			EndTimeField = value;
		}
	}

	[DataMember]
	public string Header
	{
		get
		{
			return HeaderField;
		}
		set
		{
			HeaderField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
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
