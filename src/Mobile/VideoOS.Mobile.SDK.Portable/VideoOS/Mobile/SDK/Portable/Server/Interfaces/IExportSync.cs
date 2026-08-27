using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IExportSync
{
	GetExportResponse GetExport(Guid exportId, TimeSpan timeout);

	CreateExportDownloadLinkResponse CreateExportDownloadLink(ExportParams exportParams, TimeSpan timeout);
}
