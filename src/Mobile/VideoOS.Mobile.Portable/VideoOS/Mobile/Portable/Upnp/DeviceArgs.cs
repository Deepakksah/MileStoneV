using System;

namespace VideoOS.Mobile.Portable.Upnp;

public class DeviceArgs : EventArgs
{
	public string LocalAddress { get; set; }

	public string Location { get; set; }

	public string DeviceDescription { get; set; }
}
