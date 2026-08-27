using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IInvestigationExportsAsync
{
	IAsyncResult StartInvestigationExport(ExportParams exportParams, OnResponse<StartExportResponse> onSuccess = null, OnResponse<StartExportResponse> onFail = null);

	IAsyncResult DeleteInvestigationExport(ExportParams exportParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}
