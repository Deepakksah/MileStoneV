using VideoOS.Common.Integration.Definition;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public static class DeviceTypeExtensions
{
	public static DeviceTypeEnum ToDeviceTypeEnum(this DeviceType deviceType)
	{
		return deviceType switch
		{
			DeviceType.Camera => DeviceTypeEnum.Camera, 
			DeviceType.Microphone => DeviceTypeEnum.AudioInput, 
			DeviceType.Input => DeviceTypeEnum.Input, 
			DeviceType.Output => DeviceTypeEnum.Output, 
			DeviceType.Speaker => DeviceTypeEnum.AudioOutput, 
			DeviceType.Metadata => DeviceTypeEnum.Metadata, 
			_ => DeviceTypeEnum.Unknown, 
		};
	}
}
