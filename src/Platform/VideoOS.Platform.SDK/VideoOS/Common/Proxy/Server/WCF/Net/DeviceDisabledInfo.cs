using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "DeviceDisabledInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class DeviceDisabledInfo : IDeviceDisabledInfo
{
	private Guid DeviceIdField;

	private string DeviceNameField;

	private string DeviceTypeField;

	private Guid HardwareIdField;

	private Guid RecorderIdField;

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
	public string DeviceName
	{
		get
		{
			return DeviceNameField;
		}
		set
		{
			DeviceNameField = value;
		}
	}

	[DataMember]
	public string DeviceType
	{
		get
		{
			return DeviceTypeField;
		}
		set
		{
			DeviceTypeField = value;
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
}
