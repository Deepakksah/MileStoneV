using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class DiskHistoryData
{
	public double MediaBytes { get; set; }

	public double OtherBytes { get; set; }

	public double FreeBytes { get; set; }

	public DateTime Time { get; set; }
}
