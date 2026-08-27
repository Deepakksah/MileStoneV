using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IInvestigationsSync
{
	CreateInvestigationResponse CreateInvestigation(InvestigationParams investigationParams, TimeSpan timeout);

	BaseCommandResponse UpdateInvestigation(InvestigationParams investigationParams, TimeSpan timeout);

	BaseCommandResponse UpdateInvestigationData(InvestigationParams investigationParams, TimeSpan timeout);

	BaseCommandResponse CancelInvestigationUpdate(Guid investigationId, TimeSpan timeout);

	BaseCommandResponse DeleteInvestigation(Guid investigationId, TimeSpan timeout);

	GetInvestigationsResponse GetInvestigation(Guid investigationId, TimeSpan timeout, bool includePackages = false);

	CheckForChangesResponse CheckForInvestigationChanges(TimeSpan timeout);
}
