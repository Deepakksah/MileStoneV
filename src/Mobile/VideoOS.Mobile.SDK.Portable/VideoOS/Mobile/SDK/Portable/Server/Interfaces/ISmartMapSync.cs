using System;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface ISmartMapSync
{
	GetTileServersResponse GetTileServers(TimeSpan timeout);
}
