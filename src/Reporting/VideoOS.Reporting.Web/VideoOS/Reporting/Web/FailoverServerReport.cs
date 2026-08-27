using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoOS.Management.Common;
using VideoOS.Reporting.Web.Controls;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;
using VideoOS.Reporting.Web.Plugin.Settings;

namespace VideoOS.Reporting.Web;

public class FailoverServerReport : BasePage
{
	protected Timer Timer;

	protected Label LabelServerName;

	protected UpdatePanel UpdatePanel_ServerStatus;

	protected FailoverServerStatus FailoverServerStatus;

	protected UpdatePanel UpdatePanel_CPUUsage;

	protected LiveCounter CPUUsage;

	protected UpdatePanel UpdatePanel_MemoryAvailable;

	protected LiveCounter MemoryAvailable;

	protected HyperLink hlHistory;

	protected Label LabelHistory;

	protected Label LabelMonitoredRecordingServers;

	protected Timer MonitoredRecordingServers_Timer;

	protected UpdatePanel Update_MonitoredRecordingServers;

	protected DataGrid dgRecordingServers;

	private Guid ServerId
	{
		get
		{
			return new Guid(ViewState["ServerId"] as string);
		}
		set
		{
			ViewState["ServerId"] = value.ToString();
		}
	}

	private int DataCollectorId
	{
		get
		{
			return int.Parse(ViewState["DataCollectorId"] as string, CultureInfo.InvariantCulture);
		}
		set
		{
			ViewState["DataCollectorId"] = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	private string HostName
	{
		get
		{
			return ViewState["HostName"] as string;
		}
		set
		{
			ViewState["HostName"] = value;
		}
	}

	private bool CertificateConfigured
	{
		get
		{
			return bool.Parse(ViewState["CertificateConfigured"] as string);
		}
		set
		{
			ViewState["CertificateConfigured"] = value.ToString();
		}
	}

	private Uri StatusUri => new UriBuilder(CertificateConfigured ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, HostName, 8990).Uri;

	protected void Page_Load(object sender, EventArgs e)
	{
		Guid result;
		if (Page.IsPostBack)
		{
			UpdateNonControlStateValues();
		}
		else if (Guid.TryParse(base.Request.QueryString["Id"], out result))
		{
			ServerId = result;
			ServerOverview serverOverview = Database.ServerOverview(ServerId);
			if (serverOverview == null)
			{
				base.Response.Redirect("ErrorPage.aspx", endResponse: false);
				Context.ApplicationInstance.CompleteRequest();
				return;
			}
			if (!string.Equals(serverOverview.ServerType, "FailoverServer", StringComparison.Ordinal))
			{
				base.Response.Redirect("ErrorPage.aspx", endResponse: false);
				Context.ApplicationInstance.CompleteRequest();
				return;
			}
			DataCollectorId = serverOverview.DataCollectorId;
			HostName = serverOverview.HostName;
			CertificateConfigured = ConfigSettings.Secure;
			LabelServerName.Text = HostName;
			LabelMonitoredRecordingServers.Text = HttpContext.GetGlobalResourceObject("FailoverDetails", "MonitoredRecordingServersHeader", _culture).ToString();
			dgRecordingServers.Columns[0].HeaderText = HttpContext.GetGlobalResourceObject("FailoverDetails", "ColumnMonitoredRecordingServerName", _culture).ToString();
			dgRecordingServers.Columns[1].HeaderText = HttpContext.GetGlobalResourceObject("FailoverDetails", "ColumnMonitoredRecordingServerCommunicationErrors", _culture).ToString();
			dgRecordingServers.Columns[2].HeaderText = HttpContext.GetGlobalResourceObject("FailoverDetails", "ColumnMonitoredRecordingServerPosition", _culture).ToString();
			hlHistory.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "~/HistoryServer.aspx{0}", _queryString);
			LabelHistory.Text = HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString();
			UpdateNonControlStateValues();
			FailoverStatus failoverStatus = LiveStatusProvider.GetFailoverStatus(ServerId, StatusUri);
			RefreshServerStatusData(failoverStatus);
			RefreshRecordingServersData(failoverStatus);
		}
		else
		{
			string url = FormattableString.Invariant(FormattableStringFactory.Create("ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace(" ? ", " & ")));
			base.Response.Redirect(url, endResponse: false);
			Context.ApplicationInstance.CompleteRequest();
		}
	}

	private void UpdateNonControlStateValues()
	{
		CPUUsage.Title = HttpContext.GetGlobalResourceObject("CPU", "ChartTitle", _culture).ToString();
		CPUUsage.ErrorText = HttpContext.GetGlobalResourceObject("Global", "lblError", _culture).ToString();
		MemoryAvailable.Title = HttpContext.GetGlobalResourceObject("Memory", "ChartTitle", _culture).ToString();
		MemoryAvailable.ErrorText = HttpContext.GetGlobalResourceObject("Global", "lblError", _culture).ToString();
	}

	protected void Timer_Tick(object sender, EventArgs e)
	{
		FailoverStatus failoverStatus = LiveStatusProvider.GetFailoverStatus(ServerId, StatusUri);
		RefreshServerStatusData(failoverStatus);
	}

	private void RefreshServerStatusData(FailoverStatus status)
	{
		FailoverServerStatus.Refresh(status, _culture);
		ILiveResult liveCpu = LiveDataProvider.GetLiveCpu(HostName);
		if (liveCpu.DisplayUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
		{
			CPUUsage.Refresh(liveCpu.ValueInPercent, string.Format(CultureInfo.InvariantCulture, HttpContext.GetGlobalResourceObject("CPU", "Units", _culture).ToString(), liveCpu.DisplayValue.ToString("0", _culture)), liveCpu.Error, WarningCriteria.Above);
		}
		int maxPhysicalMemory = Database.GetMaxPhysicalMemory(DataCollectorId);
		ILiveResult liveMemory = LiveDataProvider.GetLiveMemory(HostName, maxPhysicalMemory);
		if (liveMemory.DisplayUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
		{
			MemoryAvailable.Refresh(liveMemory.ValueInPercent, FormattableString.Invariant(FormattableStringFactory.Create("{0} {1}", liveMemory.DisplayValue.ToString("N", _culture), liveMemory.DisplayUnit)), liveMemory.Error, WarningCriteria.Below);
		}
	}

	protected void MonitoredRecordingServers_Timer_Tick(object sender, EventArgs e)
	{
		FailoverStatus failoverStatus = LiveStatusProvider.GetFailoverStatus(ServerId, StatusUri);
		RefreshRecordingServersData(failoverStatus);
	}

	private void RefreshRecordingServersData(FailoverStatus status)
	{
		RecorderName[] recorderNames = null;
		if (status.RecorderWatchItems.Any())
		{
			recorderNames = Database.QueryRecorderNames(status.RecorderWatchItems.Select((RecorderWatchItem r) => r.RecorderId));
			dgRecordingServers.Visible = true;
			MonitoredRecordingServers_Timer.Interval = 15000;
		}
		else
		{
			recorderNames = new RecorderName[0];
			dgRecordingServers.Visible = false;
			MonitoredRecordingServers_Timer.Interval = 6000;
		}
		dgRecordingServers.DataSource = from r in status.RecorderWatchItems
			select new
			{
				FailoverStatus = status,
				WatchItem = r,
				RecorderName = (recorderNames.FirstOrDefault((RecorderName rn) => rn.RecorderId == r.RecorderId)?.Name ?? "-")
			} into d
			orderby d.RecorderName
			select d;
		dgRecordingServers.DataBind();
	}

	protected void DataGridRecordingServers_ItemDataBound(object sender, DataGridItemEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
		{
			return;
		}
		dynamic dataItem = e.Item.DataItem;
		RecorderWatchItem recorderWatchItem = dataItem.WatchItem as RecorderWatchItem;
		string text = dataItem.RecorderName as string;
		(e.Item.FindControl("LabelName") as Label).Text = text;
		(e.Item.FindControl("LabelPosition") as Label).Text = recorderWatchItem.Position.ToString(CultureInfo.InvariantCulture);
		Image image = e.Item.FindControl("StatusImage") as Image;
		Label label = e.Item.FindControl("LabelStatus") as Label;
		if (recorderWatchItem.CommunicationErrors == CommunicationErrors.None)
		{
			image.Style["visibility"] = "hidden";
			label.Text = HttpContext.GetGlobalResourceObject("FailoverDetails", "RecordingServerCommunicationErrorNone", _culture).ToString();
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (recorderWatchItem.CommunicationErrors.HasFlag(CommunicationErrors.SslCommunication))
		{
			stringBuilder.Append(HttpContext.GetGlobalResourceObject("FailoverDetails", "RecordingServerCommunicationErrorSsl", _culture).ToString());
		}
		if (recorderWatchItem.CommunicationErrors.HasFlag(CommunicationErrors.AuthorizationError))
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine("<br/>");
			}
			stringBuilder.Append(HttpContext.GetGlobalResourceObject("FailoverDetails", "RecordingServerCommunicationErrorAuthorization", _culture).ToString());
		}
		label.Text = stringBuilder.ToString();
	}
}
