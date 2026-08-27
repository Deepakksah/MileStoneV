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

public class HistoryNetwork : BasePage
{
	protected int dataCollectorId;

	protected string serverType;

	protected string hostName;

	private RecorderOverview serverOverview;

	private Timescale selectedScale;

	protected HistoryHeader HistoryHeader;

	protected Repeater repNetwork;

	protected Label LabelTimeZoneInfo;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Guid.TryParse(base.Request.QueryString["Id"], out var result))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		serverOverview = Database.RecorderOverview(result);
		if (serverOverview == null)
		{
			base.Response.Redirect("ErrorPage.aspx");
		}
		dataCollectorId = serverOverview.DataCollectorId;
		hostName = serverOverview.HostName;
		HistoryHeader.HeaderText = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", serverOverview.Name, HttpContext.GetGlobalResourceObject("NetworkDetails", "Header", _culture).ToString());
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
		if (!Guid.TryParse(base.Request.QueryString["Id"], out var result))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		selectedScale = scale;
		AdapterOverview[] dataSource = Database.AdapterOverview(result);
		repNetwork.DataSource = dataSource;
		repNetwork.DataBind();
	}

	protected void RepNetwork_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			AdapterOverview adapterOverview = e.Item.DataItem as AdapterOverview;
			(e.Item.FindControl("LabelAdapterName") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0}:", HttpContext.GetGlobalResourceObject("NetworkDetails", "TextName", _culture).ToString());
			(e.Item.FindControl("LabelAdapterNameValue") as Label).Text = adapterOverview.AdapterName;
			TimescaleGraph timescaleGraph = e.Item.FindControl("TotalNetworkLoad") as TimescaleGraph;
			timescaleGraph.Culture = _culture;
			timescaleGraph.ChartTitle = HttpContext.GetGlobalResourceObject("NetworkLoad", "ChartTitle", _culture).ToString();
			List<HistoryData> list = Database.GetHistoryData(selectedScale, adapterOverview.AdapterTotal.InstanceId, (int)_offset.TotalMinutes).ToList();
			timescaleGraph.Unit = DataUnitUtility.GetCalculatedUnit(list.Max((HistoryData m) => m.Value) / 10.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[4]
			{
				VideoOS.Reporting.Web.Plugin.Live.Unit.BytesSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.KBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.MBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.GBSec
			}, 1024.0, 1024u).Unit;
			timescaleGraph.Refresh(selectedScale, list);
			TimescaleGraph timescaleGraph2 = e.Item.FindControl("ReceivedByAdapter") as TimescaleGraph;
			timescaleGraph2.Culture = _culture;
			timescaleGraph2.ChartTitle = HttpContext.GetGlobalResourceObject("NetworkLoadRecieved", "ChartTitle", _culture).ToString();
			List<HistoryData> list2 = Database.GetHistoryData(selectedScale, adapterOverview.AdapterReceived.InstanceId, (int)_offset.TotalMinutes).ToList();
			timescaleGraph2.Unit = DataUnitUtility.GetCalculatedUnit(list2.Max((HistoryData m) => m.Value) / 10.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[4]
			{
				VideoOS.Reporting.Web.Plugin.Live.Unit.BytesSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.KBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.MBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.GBSec
			}, 1024.0, 1024u).Unit;
			timescaleGraph2.Refresh(selectedScale, list2);
			TimescaleGraph timescaleGraph3 = e.Item.FindControl("SendByAdapter") as TimescaleGraph;
			timescaleGraph3.Culture = _culture;
			timescaleGraph3.ChartTitle = HttpContext.GetGlobalResourceObject("NetworkLoadSent", "ChartTitle", _culture).ToString();
			List<HistoryData> list3 = Database.GetHistoryData(selectedScale, adapterOverview.AdapterSent.InstanceId, (int)_offset.TotalMinutes).ToList();
			timescaleGraph3.Unit = DataUnitUtility.GetCalculatedUnit(list3.Max((HistoryData m) => m.Value) / 10.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[4]
			{
				VideoOS.Reporting.Web.Plugin.Live.Unit.BytesSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.KBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.MBSec,
				VideoOS.Reporting.Web.Plugin.Live.Unit.GBSec
			}, 1024.0, 1024u).Unit;
			timescaleGraph3.Refresh(selectedScale, list3);
		}
	}
}
