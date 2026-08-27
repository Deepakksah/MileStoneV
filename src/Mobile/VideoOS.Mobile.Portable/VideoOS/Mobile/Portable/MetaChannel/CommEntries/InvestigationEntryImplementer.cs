using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class InvestigationEntryImplementer : IInvestigationEntry
{
	public Guid Id { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public string Name { get; set; }

	public InvestigationState State { get; set; }

	public InvestigationSatus Status { get; set; }

	public int StatusProgress { get; set; }

	public IEnumerable<IInvestigationCamera> Cameras { get; set; }

	public IInvestigationEntry PreviousParams { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public DateTime? ExpirationTime { get; set; }

	public IInvestigationExports Exports { get; set; }

	public IEnumerable<IExportEntry> ExportEntries { get; set; }

	public string CreatedBy { get; set; }

	public bool ContainsAudio { get; set; }

	public void InitFromEntry(IInvestigationEntry entry)
	{
		Id = entry.Id;
		StartTime = entry.StartTime;
		EndTime = entry.EndTime;
		Name = entry.Name;
		State = entry.State;
		Status = entry.Status;
		StatusProgress = entry.StatusProgress;
		Cameras = entry.Cameras;
		PreviousParams = entry.PreviousParams;
		CreatedAt = entry.CreatedAt;
		ModifiedAt = entry.ModifiedAt;
		Exports = entry.Exports;
		CreatedBy = entry.CreatedBy;
		ContainsAudio = entry.ContainsAudio;
	}

	public InvestigationEntryImplementer()
	{
	}

	public InvestigationEntryImplementer(IInvestigationEntry entry)
	{
		InitFromEntry(entry);
	}
}
