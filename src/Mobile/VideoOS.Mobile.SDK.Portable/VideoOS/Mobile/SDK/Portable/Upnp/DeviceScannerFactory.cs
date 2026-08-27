using VideoOS.Mobile.Portable.Upnp;
using VideoOS.Mobile.Portable.Upnp.SsdpMessage;

namespace VideoOS.Mobile.SDK.Portable.Upnp;

public class DeviceScannerFactory
{
	public static DeviceScanner CreateMobileServerDeviceScanner(int searchTimeSec)
	{
		return new DeviceScanner(DeviceSearchType.BasicDevice, searchTimeSec, (SsdpMessageSearchResponse response) => response.Usn.Contains(MobileServiceDescription.MobileServiceToken.ToString()));
	}
}
