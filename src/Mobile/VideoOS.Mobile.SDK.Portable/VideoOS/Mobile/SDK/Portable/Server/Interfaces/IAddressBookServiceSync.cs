using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IAddressBookServiceSync
{
	AddressBookServiceResponse CheckServerAccessibility(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout);

	AddressBookServiceResponse SetServerEndpoints(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout);

	AddressBookServiceResponse GetServerEndpoints(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout);

	BaseCommandResponse SendInvitation(InvitationParams invitationParams, TimeSpan timeout);

	GetInvitationLinksResponse GetInvitationLinks(TimeSpan timeout);
}
