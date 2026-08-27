using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IExportAsync
{
	IAsyncResult GetExport(Guid exportId, OnResponse<GetExportResponse> onSuccess = null, OnResponse<GetExportResponse> onFail = null);

	IAsyncResult CreateExportDownloadLink(ExportParams exportParams, OnResponse<CreateExportDownloadLinkResponse> onSuccess = null, OnResponse<CreateExportDownloadLinkResponse> onFail = null);
}
