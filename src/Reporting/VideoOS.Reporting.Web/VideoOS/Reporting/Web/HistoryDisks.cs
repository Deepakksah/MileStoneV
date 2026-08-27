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

public class HistoryDisks : BasePage
{
	protected Guid serverId;

	protected int dataCollectorId;

	protected string serverType;

	protected string hostName;

	private RecorderOverview serverOverview;

	private Timescale selectedScale;

	protected HistoryHeader HistoryHeader;

	protected Repeater repDisks;

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
		HistoryHeader.HeaderText = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", serverOverview.Name, HttpContext.GetGlobalResourceObject("DiskDetails", "Header", _culture).ToString());
		HistoryHeader.HostName = hostName;
		HistoryHeader.Culture = _culture;
		if (!base.IsPostBack)
		{
			Update(base.Timescale);
		}
		LabelTimeZoneInfo.Text = base.TimeZoneInfo;
	}

	private void Update(Timescale scale)
	{
		selectedScale = scale;
		DiskOverview[] dataSource = Database.DiskOverview(serverId);
		repDisks.DataSource = dataSource;
		repDisks.DataBind();
	}

	protected void RepDisks_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			DiskOverview diskOverview = e.Item.DataItem as DiskOverview;
			(e.Item.FindControl("LabelDisk") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0}:", HttpContext.GetGlobalResourceObject("DiskDetails", "Drive", _culture).ToString());
			(e.Item.FindControl("LabelFreeSpace") as Label).Text = string.Format(CultureInfo.InvariantCulture, "({0}) {1} {2} {3} {4} {5}", diskOverview.DiskName, Math.Round(diskOverview.FreeData.Value, 2), diskOverview.FreeData.Unit, HttpContext.GetGlobalResourceObject("DiskDetails", "FreeOf", _culture).ToString(), Math.Round(diskOverview.TotalDataSize, 2), diskOverview.TotalDataSizeUnit);
			(e.Item.FindControl("LabelMediaRecordings") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0}:", HttpContext.GetGlobalResourceObject("DiskDetails", "MediaRecording", _culture).ToString());
			(e.Item.FindControl("LabelMediaRecordingsValue") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", Math.Round(diskOverview.MediaDataUsed.Value, 2), diskOverview.MediaDataUsed.Unit);
			(e.Item.FindControl("LabelOtherData") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0}:", HttpContext.GetGlobalResourceObject("DiskDetails", "OtherData", _culture).ToString());
			(e.Item.FindControl("LabelOtherDataValue") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", Math.Round(diskOverview.OtherDataUsed.Value, 2), diskOverview.OtherDataUsed.Unit);
			DiskUsagesGraph diskUsagesGraph = e.Item.FindControl("DataUsed") as DiskUsagesGraph;
			diskUsagesGraph.Culture = _culture;
			diskUsagesGraph.ChartTitle = HttpContext.GetGlobalResourceObject("DiskUsageMultipleSources", "ChartTitle", _culture).ToString();
			List<DiskHistoryData> data = Database.GetDiskHistoryData(selectedScale, diskOverview.DiskName, serverId, (int)_offset.TotalMinutes, diskOverview.TotalSizeInBytes).ToList();
			diskUsagesGraph.Unit = diskOverview.TotalDataSizeUnit;
			diskUsagesGraph.Refresh(selectedScale, data, diskOverview.TotalSizeInBytes);
			TimescaleGraph timescaleGraph = e.Item.FindControl("DisRead") as TimescaleGraph;
			timescaleGraph.Culture = _culture;
			timescaleGraph.ChartTitle = HttpContext.GetGlobalResourceObject("DiskReadSpeed", "ChartTitle", _culture).ToString();
			List<HistoryData> list = Database.GetHistoryData(selectedScale, diskOverview.DiskReadSpeedInstanceId, (int)_offset.TotalMinutes).ToList();
			timescaleGraph.Unit = DataUnitUtility.GetCalculatedUnit(list.Max((HistoryData m) => m.Value), new VideoOS.Reporting.Web.Plugin.Live.Unit[4]
			{
				VideoOS.Reporting.Web.Plugin.Live.Unit.BytesSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.KBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.MBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.GBSec
			}, 1024.0, 1024u).Unit;
			timescaleGraph.Refresh(selectedScale, list);
			TimescaleGraph timescaleGraph2 = e.Item.FindControl("DiskWrite") as TimescaleGraph;
			timescaleGraph2.Culture = _culture;
			timescaleGraph2.ChartTitle = HttpContext.GetGlobalResourceObject("DiskWriteSpeed", "ChartTitle", _culture).ToString();
			List<HistoryData> list2 = Database.GetHistoryData(selectedScale, diskOverview.DiskWriteSpeedInstanceId, (int)_offset.TotalMinutes).ToList();
			timescaleGraph2.Unit = DataUnitUtility.GetCalculatedUnit(list2.Max((HistoryData m) => m.Value), new VideoOS.Reporting.Web.Plugin.Live.Unit[4]
			{
				VideoOS.Reporting.Web.Plugin.Live.Unit.BytesSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.KBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.MBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.GBSec
			}, 1024.0, 1024u).Unit;
			timescaleGraph2.Refresh(selectedScale, list2);
		}
	}
}
