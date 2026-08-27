using System;
using VideoOS.Mobile.Portable.Interfaces;

namespace VideoOS.Mobile.UPNP.DeviceDiscoveryService;

public interface IDiscoverableService : IControl, IDisposable
{
	string ExternalHttpPath { get; set; }

	ushort ExternalHttpPort { get; set; }
}
