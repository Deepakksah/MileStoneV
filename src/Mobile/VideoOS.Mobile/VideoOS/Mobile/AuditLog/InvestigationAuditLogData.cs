using System;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Platform;

namespace VideoOS.Mobile.AuditLog;

public class InvestigationAuditLogData
{
	public string Name { get; set; }

	public DateTime Start { get; set; }

	public DateTime End { get; set; }

	public InvestigationState State { get; set; }

	public ExportAuditLogData[] Exports { get; set; }

	public Item[] Cameras { get; set; }

	public InvestigationAuditLogData(IInvestigationEntry entry)
	{
		Name = entry.Name;
		Start = entry.StartTime;
		End = entry.EndTime;
		State = entry.State;
	}
}
