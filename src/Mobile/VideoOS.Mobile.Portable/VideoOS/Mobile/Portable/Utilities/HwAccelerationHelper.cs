using System;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.Portable.Utilities;

public class HwAccelerationHelper
{
	public const string NoAcceleration = "ipp";

	public const string HwAccelerationPrefix = "hardware";

	public const string IntelAcceleration = "hardwareIntel";

	public const string NvidiaAcceleration = "hardwareNvidia";

	public const string Intel = "Intel";

	public const string Nvidia = "Nvidia";

	public static void GetHwAccelerationCount(ServerStatus status, out int intelAccelerated, out int nvidiaAccelerated)
	{
		intelAccelerated = (nvidiaAccelerated = 0);
		if (status?.MobileUsers == null)
		{
			return;
		}
		int intelAcceleratedCount = 0;
		int nvidiaAcceleratedCount = 0;
		status.MobileUsers.Foreach(delegate(MobileUser user)
		{
			user.UserConnections.Foreach(delegate(UserConnection stream)
			{
				if (!string.IsNullOrEmpty(stream.HardwareDecodingStatus))
				{
					if (HasAcceleration(stream, "hardwareIntel"))
					{
						intelAcceleratedCount++;
					}
					else if (HasAcceleration(stream, "hardwareNvidia"))
					{
						nvidiaAcceleratedCount++;
					}
				}
			});
		});
		intelAccelerated = intelAcceleratedCount;
		nvidiaAccelerated = nvidiaAcceleratedCount;
	}

	public static string GetHwAccelerationString(ServerStatus status, out int intelAccelerated, out int nvidiaAccelerated)
	{
		GetHwAccelerationCount(status, out intelAccelerated, out nvidiaAccelerated);
		return GetHwAccelerationString(intelAccelerated, nvidiaAccelerated);
	}

	public static string GetHwAccelerationString(int intelAccelerated, int nvidiaAccelerated)
	{
		return GetHwAccelerationString(intelAccelerated > 0, nvidiaAccelerated > 0);
	}

	public static string GetHwAccelerationString(bool intelAccelerated, bool nvidiaAccelerated)
	{
		if (nvidiaAccelerated && intelAccelerated)
		{
			return "Nvidia, Intel";
		}
		if (nvidiaAccelerated)
		{
			return "Nvidia";
		}
		if (intelAccelerated)
		{
			return "Intel";
		}
		return "";
	}

	public static bool HasAcceleration(ServerStatus status, string accelerationType)
	{
		return status.MobileUsers.Exists((MobileUser user) => user.UserConnections.Exists((UserConnection stream) => HasAcceleration(stream, accelerationType)));
	}

	public static bool HasAcceleration(UserConnection stream, string accelerationType)
	{
		return stream.HardwareDecodingStatus.Equals(accelerationType, StringComparison.Ordinal);
	}
}
