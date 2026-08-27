using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class DeviceTypeInformation
{
	public static readonly IReadOnlyDictionary<DeviceType, DeviceTypeInformation> DeviceTypeDictionary = new Dictionary<DeviceType, DeviceTypeInformation>
	{
		{
			DeviceType.Camera,
			new DeviceTypeInformation(DeviceType.Camera, SecurityNamespaceIdentifier.Camera, "Camera")
		},
		{
			DeviceType.Microphone,
			new DeviceTypeInformation(DeviceType.Microphone, SecurityNamespaceIdentifier.Microphone, "AudioInput")
		},
		{
			DeviceType.Speaker,
			new DeviceTypeInformation(DeviceType.Speaker, SecurityNamespaceIdentifier.Speaker, "AudioOutput")
		},
		{
			DeviceType.Input,
			new DeviceTypeInformation(DeviceType.Input, SecurityNamespaceIdentifier.Input, "Input")
		},
		{
			DeviceType.Output,
			new DeviceTypeInformation(DeviceType.Output, SecurityNamespaceIdentifier.Output, "Output")
		},
		{
			DeviceType.Metadata,
			new DeviceTypeInformation(DeviceType.Metadata, SecurityNamespaceIdentifier.Metadata, "Metadata")
		}
	};

	public DeviceType DeviceType { get; private set; }

	public Guid SecurityNamespace { get; private set; }

	public string DatabaseString { get; private set; }

	private DeviceTypeInformation(DeviceType deviceType, Guid securityNamespace, string databaseString)
	{
		DeviceType = deviceType;
		SecurityNamespace = securityNamespace;
		DatabaseString = databaseString;
	}
}
