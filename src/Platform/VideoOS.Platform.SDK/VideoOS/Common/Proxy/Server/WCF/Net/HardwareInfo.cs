using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "HardwareInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class HardwareInfo : IHardwareInfo
{
	private string DescriptionField;

	private Guid[] DeviceIdsField;

	private Guid HardwareIdField;

	private bool InterconnectedField;

	private DateTime LastModifiedField;

	private string NameField;

	private Guid RecorderIdField;

	private string UriField;

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
	public Guid HardwareId
	{
		get
		{
			return HardwareIdField;
		}
		set
		{
			HardwareIdField = value;
		}
	}

	[DataMember]
	public bool Interconnected
	{
		get
		{
			return InterconnectedField;
		}
		set
		{
			InterconnectedField = value;
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
	public Guid RecorderId
	{
		get
		{
			return RecorderIdField;
		}
		set
		{
			RecorderIdField = value;
		}
	}

	[DataMember]
	public string Uri
	{
		get
		{
			return UriField;
		}
		set
		{
			UriField = value;
		}
	}

	public bool Disabled { get; set; }
}
