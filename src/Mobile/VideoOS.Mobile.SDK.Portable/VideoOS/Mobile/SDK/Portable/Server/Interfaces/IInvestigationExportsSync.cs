using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IInvestigationExportsSync
{
	StartExportResponse StartInvestigationExport(ExportParams exportParams, TimeSpan timeout);

	BaseCommandResponse DeleteInvestigationExport(ExportParams exportParams, TimeSpan timeout);
}
