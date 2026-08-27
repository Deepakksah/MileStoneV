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

public class HistoryStorage : BasePage
{
	protected Guid serverId;

	protected Guid storageId;

	protected string serverType;

	protected string hostName;

	private RecorderOverview serverOverview;

	private Timescale selectedScale;

	protected HistoryHeader HistoryHeader;

	protected Repeater repStorage;

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
		if (!Guid.TryParse(base.Request.QueryString["StorageId"], out storageId))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		StorageOverview storageOverview = (from s in Database.StorageOverview(serverId)
			where s.StorageId.ToUpperInvariant() == storageId.ToString().ToUpperInvariant()
			where string.IsNullOrEmpty(s.ArchiveId)
			select s).First();
		hostName = serverOverview.HostName;
		HistoryHeader.HeaderText = string.Format(CultureInfo.InvariantCulture, "{0} - {1} - {2}", serverOverview.Name, storageOverview?.Name, HttpContext.GetGlobalResourceObject("StorageDetails", "Header", _culture).ToString());
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
		repStorage.DataSource = from s in Database.StorageOverview(serverId)
			where s.StorageId.ToUpperInvariant() == storageId.ToString().ToUpperInvariant()
			where string.IsNullOrEmpty(s.ArchiveId)
			select s;
		repStorage.DataBind();
	}

	protected void RepStorage_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			StorageOverview storageOverview = e.Item.DataItem as StorageOverview;
			(e.Item.FindControl("LabelName") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "Name", _culture).ToString();
			(e.Item.FindControl("LabelNameValue") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "BankNameRec", _culture).ToString();
			(e.Item.FindControl("LabelParth") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "PathLbl", _culture).ToString();
			(e.Item.FindControl("LabelParthValue") as Label).Text = storageOverview.Path;
			(e.Item.FindControl("LabelSize") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "SizeLbl", _culture).ToString();
			(e.Item.FindControl("LabelSizeValue") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", Math.Round(storageOverview.Size, 2).ToString("N", _culture), storageOverview.SizeUnit);
			(e.Item.FindControl("LabelUsed") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "UsedLbl", _culture).ToString();
			(e.Item.FindControl("LabelUsedValue") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", Math.Round(storageOverview.Used, 2).ToString("N", _culture), storageOverview.UsedUnit);
			TimescaleGraph timescaleGraph = e.Item.FindControl("DiskUsage") as TimescaleGraph;
			timescaleGraph.Culture = _culture;
			timescaleGraph.ChartTitle = HttpContext.GetGlobalResourceObject("StorageDiskUsage", "ChartTitle", _culture).ToString();
			List<HistoryData> list = Database.GetHistoryData(selectedScale, storageOverview.DiskUsageInstanceId, (int)_offset.TotalMinutes).ToList();
			VideoOS.Reporting.Web.Plugin.Live.Unit item = DataUnitUtility.GetCalculatedUnit(list.Max((HistoryData m) => m.Value) / 1000.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[2]
			{
				VideoOS.Reporting.Web.Plugin.Live.Unit.KB,
				VideoOS.Reporting.Web.Plugin.Live.Unit.MB
			}, 1024.0, 1024u).Unit;
			timescaleGraph.Unit = item;
			list.ForEach(delegate(HistoryData v)
			{
				v.Value /= 1000000.0;
			});
			timescaleGraph.Refresh(selectedScale, list);
			Repeater repeater = e.Item.FindControl("repArchive") as Repeater;
			repeater.DataSource = from s in Database.StorageOverview(serverId)
				where s.StorageId.ToUpperInvariant() == storageId.ToString().ToUpperInvariant()
				where !string.IsNullOrEmpty(s.ArchiveId)
				select s;
			repeater.DataBind();
		}
	}

	protected void RepArchive_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			StorageOverview storageOverview = e.Item.DataItem as StorageOverview;
			(e.Item.FindControl("LabelName") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "Name", _culture).ToString();
			(e.Item.FindControl("LabelNameValue") as Label).Text = storageOverview.Name;
			(e.Item.FindControl("LabelParth") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "PathLbl", _culture).ToString();
			(e.Item.FindControl("LabelParthValue") as Label).Text = storageOverview.Path;
			(e.Item.FindControl("LabelSize") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "SizeLbl", _culture).ToString();
			(e.Item.FindControl("LabelSizeValue") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", Math.Round(storageOverview.Size, 2).ToString("N", _culture), storageOverview.SizeUnit);
			(e.Item.FindControl("LabelUsed") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "UsedLbl", _culture).ToString();
			(e.Item.FindControl("LabelUsedValue") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", Math.Round(storageOverview.Used, 2).ToString("N", _culture), storageOverview.UsedUnit);
			TimescaleGraph timescaleGraph = e.Item.FindControl("DiskUsage") as TimescaleGraph;
			timescaleGraph.Culture = _culture;
			timescaleGraph.ChartTitle = HttpContext.GetGlobalResourceObject("StorageDiskUsage", "ChartTitle", _culture).ToString();
			List<HistoryData> list = Database.GetHistoryData(selectedScale, storageOverview.DiskUsageInstanceId, (int)_offset.TotalMinutes).ToList();
			VideoOS.Reporting.Web.Plugin.Live.Unit item = DataUnitUtility.GetCalculatedUnit(list.Max((HistoryData m) => m.Value) / 1000.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[2]
			{
				VideoOS.Reporting.Web.Plugin.Live.Unit.KB,
				VideoOS.Reporting.Web.Plugin.Live.Unit.MB
			}, 1024.0, 1024u).Unit;
			timescaleGraph.Unit = item;
			list.ForEach(delegate(HistoryData v)
			{
				v.Value /= 1000000.0;
			});
			timescaleGraph.Refresh(selectedScale, list);
		}
	}
}
