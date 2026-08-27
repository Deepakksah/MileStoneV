using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IInvitaionAsync
{
	IAsyncResult SendInvitation(InvitationParams invitationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult GetInvitationLinks(OnResponse<GetInvitationLinksResponse> onSuccess = null, OnResponse<GetInvitationLinksResponse> onFail = null);
}
