using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public interface IInvestigationExports
{
	Guid OriginDbExportId { get; }

	Guid DbExportId { get; }

	Guid AviExportId { get; }

	Guid MkvExportId { get; }

	Guid BinaryExportId { get; }

	Guid GetExportId(ExportType type);
}
