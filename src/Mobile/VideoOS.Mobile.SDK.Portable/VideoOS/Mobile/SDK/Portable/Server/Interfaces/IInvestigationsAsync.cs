using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IInvestigationsAsync
{
	IAsyncResult CreateInvestigation(InvestigationParams investigationParams, OnResponse<CreateInvestigationResponse> onSuccess = null, OnResponse<CreateInvestigationResponse> onFail = null);

	IAsyncResult UpdateInvestigation(InvestigationParams investigationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult UpdateInvestigationData(InvestigationParams investigationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult CancelInvestigationUpdate(Guid investigationId, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult DeleteInvestigation(Guid investigationId, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult GetInvestigation(Guid investigationId, bool includePackages = false, OnResponse<GetInvestigationsResponse> onSuccess = null, OnResponse<GetInvestigationsResponse> onFail = null);

	IAsyncResult CheckForInvestigationChanges(OnResponse<CheckForChangesResponse> onSuccess = null, OnResponse<CheckForChangesResponse> onFail = null);
}
