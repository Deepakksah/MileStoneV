namespace VideoOS.Platform.SDK.Config;

public static class ISystemInfoExtensions
{
	public static IDevice GetDevice(this ISystemInfo systemInfo, FQID deviceId)
	{
		if (deviceId == null)
		{
			return null;
		}
		if (deviceId.Kind == Kind.Camera)
		{
			if (systemInfo.Cameras.ContainsKey(deviceId.ObjectId))
			{
				return systemInfo.Cameras[deviceId.ObjectId];
			}
		}
		else if (deviceId.Kind == Kind.Microphone)
		{
			if (systemInfo.Microphones.ContainsKey(deviceId.ObjectId))
			{
				return systemInfo.Microphones[deviceId.ObjectId];
			}
		}
		else if (deviceId.Kind == Kind.Speaker)
		{
			if (systemInfo.Speakers.ContainsKey(deviceId.ObjectId))
			{
				return systemInfo.Speakers[deviceId.ObjectId];
			}
		}
		else if (deviceId.Kind == Kind.Metadata)
		{
			if (systemInfo.MetadataDevices.TryGetValue(deviceId.ObjectId, out var value))
			{
				return value;
			}
		}
		else if (deviceId.Kind == Kind.Output && systemInfo.OutputDevices.ContainsKey(deviceId.ObjectId))
		{
			return systemInfo.OutputDevices[deviceId.ObjectId];
		}
		return null;
	}
}
