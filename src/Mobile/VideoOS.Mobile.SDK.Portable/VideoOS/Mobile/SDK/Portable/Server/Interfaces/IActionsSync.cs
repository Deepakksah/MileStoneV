using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IActionsSync
{
	GetActionsResponse GetActions(OutputsEventsParams actionsParams, TimeSpan timeout);

	BaseCommandResponse ActivateAction(ActivateParams activateParams, TimeSpan timeout);
}
