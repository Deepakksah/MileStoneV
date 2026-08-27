using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public interface IInvestigationEntry
{
	Guid Id { get; }

	DateTime StartTime { get; }

	DateTime EndTime { get; }

	string Name { get; }

	InvestigationState State { get; }

	InvestigationSatus Status { get; }

	int StatusProgress { get; }

	IEnumerable<IInvestigationCamera> Cameras { get; }

	IInvestigationEntry PreviousParams { get; }

	DateTime CreatedAt { get; }

	DateTime ModifiedAt { get; }

	DateTime? ExpirationTime { get; }

	IInvestigationExports Exports { get; }

	IEnumerable<IExportEntry> ExportEntries { get; }

	string CreatedBy { get; }

	bool ContainsAudio { get; }
}
