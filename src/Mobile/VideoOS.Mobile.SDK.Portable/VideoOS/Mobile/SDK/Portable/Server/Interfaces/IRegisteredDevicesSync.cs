using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IRegisteredDevicesSync
{
	GetRegisteredDevicesResponse GetRegisteredDevices(TimeSpan timeout);

	BaseCommandResponse SetRegisteredDevicesState(IEnumerable<RegisteredDevice> devices, TimeSpan timeout);
}
