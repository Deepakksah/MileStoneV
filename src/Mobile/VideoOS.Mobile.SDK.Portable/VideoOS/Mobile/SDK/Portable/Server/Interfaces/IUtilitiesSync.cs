using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IUtilitiesSync
{
	GetServerStatusResponse GetServerStatus(GetServerStatusParams serverStatusParams, TimeSpan timeout);

	GetConfigurationResponse GetConfiguration(string name, Guid id, TimeSpan timeout);

	CameraCapabilitiesResponse GetCapabilities(CapabilitiesParams capabilitiesParams, TimeSpan timeout);

	ServerCapabilitiesResponse GetServerCapabilities(TimeSpan timeout);
}
