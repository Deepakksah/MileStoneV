using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IUtilitiesAsync
{
	IAsyncResult GetServerStatus(GetServerStatusParams tServerStatusParams = null, OnResponse<GetServerStatusResponse> onSuccess = null, OnResponse<GetServerStatusResponse> onFail = null);

	IAsyncResult GetConfiguration(string name, Guid id, OnResponse<GetConfigurationResponse> onSuccess = null, OnResponse<GetConfigurationResponse> onFail = null);

	IAsyncResult GetCapabilities(CapabilitiesParams capabilitiesParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult GetServerCapabilities(CapabilitiesParams capabilitiesParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}
