using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public static class WebApiConvert
{
	public static string ToString(DevicePosition position)
	{
		return position switch
		{
			DevicePosition.Ceiling => "ceiling", 
			DevicePosition.Wall => "wall", 
			DevicePosition.Ground => "ground", 
			_ => "unknown mount", 
		};
	}
}
