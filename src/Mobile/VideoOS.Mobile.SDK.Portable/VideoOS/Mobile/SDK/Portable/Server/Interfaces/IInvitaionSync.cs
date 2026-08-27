using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IInvitaionSync
{
	BaseCommandResponse SendInvitation(InvitationParams invitationParams, TimeSpan timeout);

	GetInvitationLinksResponse GetInvitationLinks(TimeSpan timeout);
}
