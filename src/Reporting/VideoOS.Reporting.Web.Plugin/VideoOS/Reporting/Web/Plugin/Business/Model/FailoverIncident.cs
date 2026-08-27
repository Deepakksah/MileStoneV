using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class FailoverIncident
{
	public string RecorderName { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }
}
