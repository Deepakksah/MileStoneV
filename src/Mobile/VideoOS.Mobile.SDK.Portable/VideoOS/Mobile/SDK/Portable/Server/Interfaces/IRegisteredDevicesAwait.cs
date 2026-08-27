using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IRegisteredDevicesAwait
{
	Task<GetRegisteredDevicesResponse> GetRegisteredDevicesAsync(TimeSpan timeout);

	Task<BaseCommandResponse> SetRegisteredDevicesStateAsync(IEnumerable<RegisteredDevice> devices, TimeSpan timeout);
}
