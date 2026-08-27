using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Controls;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web;

public class HistoryRecordingServer : BasePage
{
	protected Guid serverId;

	protected int dataCollectorId;

	protected string serverType;

	protected string hostName;

	private RecorderOverview serverOverview;

	protected HistoryHeader HistoryHeader;

	protected TimescaleGraph CPUUsage;

	protected TimescaleGraph MemoryAvailable;

	protected Label LabelTimeZoneInfo;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Guid.TryParse(base.Request.QueryString["Id"], out serverId))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		serverOverview = Database.RecorderOverview(serverId);
		if (serverOverview == null)
		{
			base.Response.Redirect("ErrorPage.aspx");
		}
		dataCollectorId = serverOverview.DataCollectorId;
		hostName = serverOverview.HostName;
		HistoryHeader.HeaderText = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", serverOverview.Name, HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString());
		HistoryHeader.HostName = hostName;
		HistoryHeader.Culture = _culture;
		CPUUsage.Culture = _culture;
		MemoryAvailable.Culture = _culture;
		CPUUsage.ChartTitle = HttpContext.GetGlobalResourceObject("CPU", "ChartTitle", _culture).ToString();
		MemoryAvailable.ChartTitle = HttpContext.GetGlobalResourceObject("Memory", "ChartTitle", _culture).ToString();
		if (!base.IsPostBack)
		{
			Update(base.Timescale);
		}
		LabelTimeZoneInfo.Text = base.TimeZoneInfo;
	}

	private void Update(Timescale scale)
	{
		HistoryData[] historyData = Database.GetHistoryData(scale, serverOverview.CpuInstanceId, (int)_offset.TotalMinutes);
		CPUUsage.Refresh(scale, historyData.ToList());
		List<HistoryData> list = Database.GetHistoryData(scale, serverOverview.MemoryInstanceId, (int)_offset.TotalMinutes).ToList();
		MemoryAvailable.Unit = DataUnitUtility.GetCalculatedUnit(list.Max((HistoryData m) => m.Value) / 10.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[3]
		{
			VideoOS.Reporting.Web.Plugin.Live.Unit.MB,
			VideoOS.Reporting.Web.Plugin.Live.Unit.GB,
			VideoOS.Reporting.Web.Plugin.Live.Unit.TB
		}, 1024.0, 1024u).Unit;
		MemoryAvailable.Refresh(scale, list);
	}
}
