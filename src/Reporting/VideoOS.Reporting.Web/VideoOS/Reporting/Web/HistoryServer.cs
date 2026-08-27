using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Controls;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web;

public class HistoryServer : BasePage
{
	protected Guid serverId;

	protected int dataCollectorId;

	protected string serverType;

	protected string hostName;

	private ServerOverview serverOverview;

	protected HistoryHeader HistoryHeader;

	protected TimescaleGraph CPUUsage;

	protected TimescaleGraph MemoryAvailable;

	protected Panel PanelIncidents;

	protected Label Header1;

	protected Timer Incidents_Timer;

	protected UpdatePanel UpdatePanel_Incidents;

	protected DataGrid dgIncidents;

	protected Label LabelTimeZoneInfo;

	public SortDirection IncidentsSortDirection
	{
		get
		{
			return (SortDirection)Enum.Parse(typeof(SortDirection), ViewState["Incidents_SortDirection"].ToString());
		}
		set
		{
			ViewState["Incidents_SortDirection"] = value.ToString();
		}
	}

	public string IncidentsSortExpression
	{
		get
		{
			return ViewState["Incidents_SortExpression"].ToString();
		}
		set
		{
			ViewState["Incidents_SortExpression"] = value;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Guid.TryParse(base.Request.QueryString["Id"], out serverId))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
			return;
		}
		serverOverview = Database.ServerOverview(serverId);
		if (serverOverview == null)
		{
			base.Response.Redirect("ErrorPage.aspx");
			return;
		}
		dataCollectorId = serverOverview.DataCollectorId;
		serverType = HttpContext.GetGlobalResourceObject("Global", serverOverview.ServerType, _culture)?.ToString();
		hostName = serverOverview.HostName;
		if (!string.IsNullOrEmpty(serverType))
		{
			HistoryHeader.HeaderText = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", serverType, HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString());
		}
		else
		{
			HistoryHeader.HeaderText = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", hostName, HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString());
		}
		HistoryHeader.HostName = hostName;
		HistoryHeader.Culture = _culture;
		CPUUsage.Culture = _culture;
		MemoryAvailable.Culture = _culture;
		CPUUsage.ChartTitle = HttpContext.GetGlobalResourceObject("CPU", "ChartTitle", _culture).ToString();
		MemoryAvailable.ChartTitle = HttpContext.GetGlobalResourceObject("Memory", "ChartTitle", _culture).ToString();
		if (!base.IsPostBack)
		{
			Update(base.Timescale);
			if (serverOverview.ServerType == "FailoverServer")
			{
				PanelIncidents.Visible = true;
				Header1.Text = HttpContext.GetGlobalResourceObject("FailoverDetails", "FailoverIncidents", _culture).ToString();
				List<FailoverIncident> dataSource = Database.GetFailoverIncidents(serverId, base.Timescale).ToList();
				dgIncidents.DataSource = dataSource;
				dgIncidents.Columns[0].HeaderText = HttpContext.GetGlobalResourceObject("FailoverDetails", "RecorderHeader", _culture).ToString();
				dgIncidents.Columns[1].HeaderText = HttpContext.GetGlobalResourceObject("FailoverDetails", "FailoverStartHeader", _culture).ToString();
				dgIncidents.Columns[2].HeaderText = HttpContext.GetGlobalResourceObject("FailoverDetails", "FailoverStopHeader", _culture).ToString();
				IncidentsSortExpression = "RecorderName";
				IncidentsSortDirection = SortDirection.Ascending;
				dgIncidents.DataBind();
			}
		}
		LabelTimeZoneInfo.Text = base.TimeZoneInfo;
	}

	private void Update(Timescale scale)
	{
		HistoryData[] historyData = Database.GetHistoryData(scale, serverOverview.CpuInstanceId, (int)_offset.TotalMinutes);
		CPUUsage.Refresh(scale, historyData.ToList());
		List<HistoryData> list = Database.GetHistoryData(scale, serverOverview.MemoryInstanceId, (int)_offset.TotalMinutes).ToList();
		VideoOS.Reporting.Web.Plugin.Live.Unit item = DataUnitUtility.GetCalculatedUnit(list.Max((HistoryData m) => m.Value) / 10.0, new VideoOS.Reporting.Web.Plugin.Live.Unit[3]
		{
			VideoOS.Reporting.Web.Plugin.Live.Unit.MB,
			VideoOS.Reporting.Web.Plugin.Live.Unit.GB,
			VideoOS.Reporting.Web.Plugin.Live.Unit.TB
		}, 1024.0, 1024u).Unit;
		MemoryAvailable.Unit = item;
		MemoryAvailable.Refresh(scale, list);
	}

	protected void DataGridIncidents_ItemDataBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			FailoverIncident failoverIncident = e.Item.DataItem as FailoverIncident;
			(e.Item.FindControl("LabelName") as Label).Text = failoverIncident.RecorderName;
			(e.Item.FindControl("LabelStartTime") as Label).Text = failoverIncident.StartTime.ToString(_culture);
			(e.Item.FindControl("LabelEndTime") as Label).Text = failoverIncident.EndTime.ToString(_culture);
		}
	}

	protected void DataGridIncidents_SortCommand(object source, DataGridSortCommandEventArgs e)
	{
		if (e.SortExpression != IncidentsSortExpression)
		{
			IncidentsSortDirection = SortDirection.Ascending;
		}
		else if (IncidentsSortDirection.Equals(SortDirection.Ascending))
		{
			IncidentsSortDirection = SortDirection.Descending;
		}
		else
		{
			IncidentsSortDirection = SortDirection.Ascending;
		}
		IncidentsSortExpression = e.SortExpression;
		UpdateColumnHeaders(dgIncidents, e.SortExpression, IncidentsSortDirection);
		List<FailoverIncident> source2 = Database.GetFailoverIncidents(serverId, base.Timescale).ToList();
		dgIncidents.DataSource = (IncidentsSortDirection.Equals(SortDirection.Ascending) ? source2.OrderBy((FailoverIncident x) => x.GetType().GetProperty(IncidentsSortExpression).GetValue(x, null)) : source2.OrderByDescending((FailoverIncident x) => x.GetType().GetProperty(IncidentsSortExpression).GetValue(x, null)));
		dgIncidents.DataBind();
	}

	protected void Incidents_Timer_Tick(object sender, EventArgs e)
	{
		List<FailoverIncident> source = Database.GetFailoverIncidents(serverId, base.Timescale).ToList();
		if (source.Count() > 0)
		{
			dgIncidents.DataSource = (IncidentsSortDirection.Equals(SortDirection.Ascending) ? source.OrderBy((FailoverIncident x) => x.GetType().GetProperty(IncidentsSortExpression).GetValue(x, null)) : source.OrderByDescending((FailoverIncident x) => x.GetType().GetProperty(IncidentsSortExpression).GetValue(x, null)));
			dgIncidents.DataBind();
		}
	}

	private static void UpdateColumnHeaders(DataGrid dataGrid, string sortExpression, SortDirection sortDirection)
	{
		foreach (DataGridColumn column in dataGrid.Columns)
		{
			column.HeaderText = column.HeaderText.Replace("&#x25B2;", string.Empty).Replace("&#x25BC;", string.Empty);
			if (column.SortExpression == sortExpression)
			{
				if (sortDirection.Equals(SortDirection.Ascending))
				{
					column.HeaderText += "&#x25B2;";
				}
				else
				{
					column.HeaderText += "&#x25BC;";
				}
			}
		}
	}
}
