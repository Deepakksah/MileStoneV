using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class RecorderOverview
{
	public bool Authorized { get; set; }

	public Guid RecorderId { get; set; }

	public string Name { get; set; }

	public int DataCollectorId { get; set; }

	public string HostName { get; set; }

	public int CpuInstanceId { get; set; }

	public int MemoryInstanceId { get; set; }
}
