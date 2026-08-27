using System;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface ISmartMapAsync
{
	IAsyncResult GetTileServers(OnResponse<GetTileServersResponse> onSuccess = null, OnResponse<GetTileServersResponse> onFail = null);
}
