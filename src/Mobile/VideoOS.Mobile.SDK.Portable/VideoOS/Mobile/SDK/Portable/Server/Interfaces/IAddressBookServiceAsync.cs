using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IAddressBookServiceAsync
{
	IAsyncResult CheckServerAccessibility(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null);

	IAsyncResult SetServerEndpoints(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null);

	IAsyncResult GetServerEndpoints(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null);

	IAsyncResult SendInvitation(InvitationParams invitationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult GetInvitationLinks(OnResponse<GetInvitationLinksResponse> onSuccess = null, OnResponse<GetInvitationLinksResponse> onFail = null);
}
