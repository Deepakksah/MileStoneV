using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.1")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class DeviceDisabledInfo : IDeviceDisabledInfo, IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid DeviceIdField;

	private string DeviceNameField;

	private string DeviceTypeField;

	private Guid HardwareIdField;

	private Guid RecorderIdField;

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
