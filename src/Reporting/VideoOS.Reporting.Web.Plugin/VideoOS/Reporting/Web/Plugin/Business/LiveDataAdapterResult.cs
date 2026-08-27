using System;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Plugin.Business;

public class LiveDataAdapterResult
{
	public string InstanceName { get; set; }

	public float SentSec { get; set; }

	public Unit SentSecUnit { get; set; }

	public float ReceivedSec { get; set; }

	public Unit ReceivedSecUnit { get; set; }

	public float Total { get; set; }

	public Unit TotalUnit { get; set; }

	public float CurrentBandwidth { get; set; }

	public Unit CurrentBandwidthUnit { get; set; }

	public float Utilization { get; set; }

	public Unit UtilizationUnit { get; set; }

	public bool Error { get; set; }

	public bool Empty { get; set; }

	public static LiveDataAdapterResult Convert(AdapterOverview overview)
	{
		if (overview == null)
		{
			throw new ArgumentNullException("overview");
		}
		return new LiveDataAdapterResult
		{
			CurrentBandwidthUnit = Unit.Unknown,
			InstanceName = overview.AdapterName,
			ReceivedSec = (float)overview.AdapterReceived.Value,
			ReceivedSecUnit = overview.AdapterReceived.Unit,
			SentSec = (float)overview.AdapterSent.Value,
			SentSecUnit = overview.AdapterSent.Unit,
			Total = (float)overview.AdapterTotal.Value,
			TotalUnit = overview.AdapterTotal.Unit,
			UtilizationUnit = Unit.Unknown
		};
	}
}
