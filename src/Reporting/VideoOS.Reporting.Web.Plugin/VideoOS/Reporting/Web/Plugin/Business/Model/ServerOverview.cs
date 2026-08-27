using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class ServerOverview
{
	public Guid ServerId { get; set; }

	public string ServerName { get; set; }

	public int DataCollectorId { get; set; }

	public string ServerType { get; set; }

	public string HostName { get; set; }

	public int CpuInstanceId { get; set; }

	public int MemoryInstanceId { get; set; }
}
