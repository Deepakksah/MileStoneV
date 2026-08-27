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

public class HistoryCamera : BasePage
{
	protected Guid deviceId;

	protected string serverType;

	protected string hostName;

	private RecorderOverview serverOverview;

	protected HistoryHeader HistoryHeader;

	protected TimescaleGraph DataReceived;

	protected TimescaleGraph DiskUsage;

	protected Label LabelTimeZoneInfo;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Guid.TryParse(base.Request.QueryString["Id"], out deviceId))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		DeviceOverview deviceOverview = Database.GetDeviceOverview(deviceId).First();
		serverOverview = Database.RecorderOverview(deviceOverview.RecorderId);
		if (serverOverview == null)
		{
			base.Response.Redirect("ErrorPage.aspx");
		}
		hostName = serverOverview.HostName;
		HistoryHeader.HeaderText = string.Format(CultureInfo.InvariantCulture, "{0} - {1} - {2}", serverOverview.Name, deviceOverview?.DeviceName, HttpContext.GetGlobalResourceObject("CameraDetails", "Header", _culture).ToString());
		HistoryHeader.HostName = deviceOverview.DeviceHostName.Trim();
		HistoryHeader.Culture = _culture;
		if (!base.IsPostBack)
		{
			Update(base.Timescale);
		}
		LabelTimeZoneInfo.Text = base.TimeZoneInfo;
	}

	private void Update(Timescale scale)
	{
		DeviceInstancesOverview deviceInstancesOverview = Database.GetDeviceInstancesOverview(deviceId);
		DataReceived.Culture = _culture;
		DataReceived.ChartTitle = HttpContext.GetGlobalResourceObject("DeviceNetworkLoad", "ChartTitle", _culture).ToString();
		List<HistoryData> list = Database.GetHistoryData(scale, deviceInstancesOverview.NetworkLoadInstanceId, (int)_offset.TotalMinutes).ToList();
		VideoOS.Reporting.Web.Plugin.Live.Unit item = DataUnitUtility.GetCalculatedUnit(list.Max((HistoryData m) => m.Value) / 10.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[4]
		{
			VideoOS.Reporting.Web.Plugin.Live.Unit.BytesSec,
			VideoOS.Reporting.Web.Plugin.Live.Unit.KBSec,
			VideoOS.Reporting.Web.Plugin.Live.Unit.MBSec,
			VideoOS.Reporting.Web.Plugin.Live.Unit.GBSec
		}, 1024.0, 1024u).Unit;
		DataReceived.Unit = item;
		DataReceived.Refresh(scale, list);
		DiskUsage.Culture = _culture;
		DiskUsage.ChartTitle = HttpContext.GetGlobalResourceObject("DeviceDiskUsage", "ChartTitle", _culture).ToString();
		list = Database.GetHistoryData(scale, deviceInstancesOverview.StorageUsageInstanceId, (int)_offset.TotalMinutes).ToList();
		item = DataUnitUtility.GetCalculatedUnit(list.Max((HistoryData m) => m.Value) / 1000.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[2]
		{
			VideoOS.Reporting.Web.Plugin.Live.Unit.KB,
			VideoOS.Reporting.Web.Plugin.Live.Unit.MB
		}, 1024.0, 1024u).Unit;
		DiskUsage.Unit = item;
		list.ForEach(delegate(HistoryData v)
		{
			v.Value /= 1000000.0;
		});
		DiskUsage.Refresh(scale, list);
	}
}
