using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DeviceMappingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class DeviceMappingInfo : IExtensibleDataObject
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
