using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.Portable.Utilities;

public static class ServerStatusExtensions
{
	public static void FillHwAccelratingInfoForTrayManager(this ServerStatus serverStatus)
	{
		serverStatus.HwAcceleratedType = HwAccelerationHelper.GetHwAccelerationString(serverStatus, out var intelAccelerated, out var nvidiaAccelerated);
		serverStatus.HwAcceleratedCount = intelAccelerated + nvidiaAccelerated;
	}
}
