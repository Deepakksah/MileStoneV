using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DeviceMappingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class DeviceMappingInfo : IExtensibleDataObject, IDeviceMappingInfo
{
	private ExtensionDataObject extensionDataField;

	private Guid DeviceAField;

	private Guid DeviceBField;

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
	public Guid DeviceA
	{
		get
		{
			return DeviceAField;
		}
		set
		{
			DeviceAField = value;
		}
	}

	[DataMember]
	public Guid DeviceB
	{
		get
		{
			return DeviceBField;
		}
		set
		{
			DeviceBField = value;
		}
	}
}
