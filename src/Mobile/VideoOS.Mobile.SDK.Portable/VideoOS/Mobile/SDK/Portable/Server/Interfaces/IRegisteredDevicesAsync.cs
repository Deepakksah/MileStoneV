using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IRegisteredDevicesAsync
{
	IAsyncResult GetRegisteredDevices(OnResponse<GetRegisteredDevicesResponse> onSuccess = null, OnResponse<GetRegisteredDevicesResponse> onFail = null);

	IAsyncResult SetRegisteredDevicesState(IEnumerable<RegisteredDevice> devices, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}
