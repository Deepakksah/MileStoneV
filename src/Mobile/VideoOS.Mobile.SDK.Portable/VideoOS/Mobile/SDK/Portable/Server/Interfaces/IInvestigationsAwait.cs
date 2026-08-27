using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IInvestigationsAwait
{
	Task<CreateInvestigationResponse> CreateInvestigationAsync(InvestigationParams investigationParams, TimeSpan timeout);

	Task<BaseCommandResponse> UpdateInvestigationAsync(InvestigationParams investigationParams, TimeSpan timeout);

	Task<BaseCommandResponse> UpdateInvestigationDataAsync(InvestigationParams investigationParams, TimeSpan timeout);

	Task<BaseCommandResponse> CancelInvestigationUpdateAsync(Guid investigationId, TimeSpan timeout);

	Task<BaseCommandResponse> DeleteInvestigationAsync(Guid investigationId, TimeSpan timeout);

	Task<GetInvestigationsResponse> GetInvestigationAsync(Guid investigationId, TimeSpan timeout, bool includePackages = false);

	Task<CheckForChangesResponse> CheckForInvestigationChangesAsync(TimeSpan timeout);
}
