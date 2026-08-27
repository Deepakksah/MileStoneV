using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using VideoOS.Management.Common;
using VideoOS.Reporting.Web.Controls;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web;

public class HistoryAlarms : BasePage
{
	protected Guid sessionId;

	protected string eventServer;

	protected AlarmHeader AlarmHeader;

	protected Panel PanelBody;

	protected Label LabelIncomplete;

	protected Label LabelType;

	protected DropDownList DropDownListType;

	protected Label LabelAdditionalType;

	protected DropDownList DropDownListAdditionalType;

	protected AlarmGraph AlarmGraph;

	protected AlarmSummaryGraph AlarmSummaryGraph;

	protected Label LabelTypeCompare;

	protected DropDownList DropDownListTypeCompare;

	protected Label LabelAdditionalTypeCompare;

	protected DropDownList DropDownListAdditionalTypeCompare;

	protected AlarmGraph AlarmGraphCompare;

	protected AlarmSummaryGraph AlarmSummaryGraphCompare;

	protected Label LabelUnresponsive;

	protected DataGrid DataGridServers;

	protected Label LabelTimeZoneInfo;

	protected override string NamespaceIdentifier => SecurityNamespaceIdentifier.Alarms.ToString();

	protected override string SecurityAction => "GENERIC_READ";

	protected override string SecurityObject => SecurityNamespaceIdentifier.Alarms.ToString();

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Guid.TryParse(base.Request.QueryString["SessionId"], out sessionId))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		if (string.IsNullOrEmpty(base.Request.QueryString["EventServer"]))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		else
		{
			eventServer = base.Request.QueryString["EventServer"];
		}
		if (!base.IsPostBack)
		{
			LabelAdditionalType.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelDataSize", _culture).ToString();
			DropDownListAdditionalType.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "30 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeMinutes", _culture).ToString()), ResponseTime.ThirtyMinutes.ToString()));
			DropDownListAdditionalType.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "1 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeHour", _culture).ToString()), ResponseTime.OneHour.ToString()));
			DropDownListAdditionalType.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "2 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeHours", _culture).ToString()), ResponseTime.TwoHours.ToString()));
			DropDownListAdditionalType.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "8 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeHours", _culture).ToString()), ResponseTime.EightHours.ToString()));
			DropDownListAdditionalType.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "1 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeDay", _culture).ToString()), ResponseTime.OneDay.ToString()));
			DropDownListAdditionalType.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "2 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeDays", _culture).ToString()), ResponseTime.TwoDays.ToString()));
			DropDownListAdditionalType.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "1 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeWeek", _culture).ToString()), ResponseTime.OneWeek.ToString()));
			LabelAdditionalTypeCompare.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelDataSize", _culture).ToString();
			DropDownListAdditionalTypeCompare.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "30 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeMinutes", _culture).ToString()), ResponseTime.ThirtyMinutes.ToString()));
			DropDownListAdditionalTypeCompare.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "1 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeHour", _culture).ToString()), ResponseTime.OneHour.ToString()));
			DropDownListAdditionalTypeCompare.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "2 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeHours", _culture).ToString()), ResponseTime.TwoHours.ToString()));
			DropDownListAdditionalTypeCompare.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "8 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeHours", _culture).ToString()), ResponseTime.EightHours.ToString()));
			DropDownListAdditionalTypeCompare.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "1 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeDay", _culture).ToString()), ResponseTime.OneDay.ToString()));
			DropDownListAdditionalTypeCompare.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "2 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeDays", _culture).ToString()), ResponseTime.TwoDays.ToString()));
			DropDownListAdditionalTypeCompare.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, "1 {0}", HttpContext.GetGlobalResourceObject("AlarmStrings", "dataSizeWeek", _culture).ToString()), ResponseTime.OneWeek.ToString()));
			Update(base.AlarmTimescale);
			Update(base.AlarmReportType);
			UpdateGraph();
		}
		LabelIncomplete.Text = string.Format(CultureInfo.InvariantCulture, "<h4>{0}</h4>", HttpContext.GetGlobalResourceObject("AlarmGlobal", "Incomplete", _culture).ToString());
		LabelUnresponsive.Text = string.Format(CultureInfo.InvariantCulture, "<h4>{0}</h4>", HttpContext.GetGlobalResourceObject("AlarmGlobal", "UnrespSvr", _culture).ToString());
		DataGridServers.Columns[0].HeaderText = HttpContext.GetGlobalResourceObject("AlarmStrings", "ServerName", _culture).ToString();
		DataGridServers.Columns[1].HeaderText = HttpContext.GetGlobalResourceObject("AlarmStrings", "ServerState", _culture).ToString();
		LabelTimeZoneInfo.Text = base.TimeZoneInfo;
		AlarmHeader.Culture = _culture;
	}

	private void Update(AlarmTimescale scale)
	{
		switch (scale)
		{
		case AlarmTimescale.Day:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "lavelYAlarmsPerHour", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "lavelYAlarmsPerHour", _culture).ToString();
			break;
		case AlarmTimescale.Week:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerDay", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerDay", _culture).ToString();
			break;
		case AlarmTimescale.Month:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerDay", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerDay", _culture).ToString();
			break;
		case AlarmTimescale.SixMonth:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerMonth", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerMonth", _culture).ToString();
			break;
		case AlarmTimescale.Year:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerMonth", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerMonth", _culture).ToString();
			break;
		}
	}

	private void Update(AlarmReportType reportType)
	{
		if (reportType.Equals(AlarmReportType.Empty))
		{
			PanelBody.Visible = false;
			return;
		}
		PanelBody.Visible = true;
		LabelAdditionalType.Visible = false;
		DropDownListAdditionalType.Visible = false;
		LabelAdditionalTypeCompare.Visible = false;
		DropDownListAdditionalTypeCompare.Visible = false;
		AlarmGraph.Visible = true;
		AlarmGraphCompare.Visible = true;
		AlarmSummaryGraph.Visible = false;
		AlarmSummaryGraphCompare.Visible = false;
		switch (reportType)
		{
		case AlarmReportType.Category:
			LabelType.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectCategoryType", _culture).ToString();
			LabelTypeCompare.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectCategoryType", _culture).ToString();
			break;
		case AlarmReportType.State:
			LabelType.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectStateType", _culture).ToString();
			LabelTypeCompare.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectStateType", _culture).ToString();
			break;
		case AlarmReportType.Priority:
			LabelType.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectPriorityType", _culture).ToString();
			LabelTypeCompare.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectPriorityType", _culture).ToString();
			break;
		case AlarmReportType.ReasonCode:
			LabelType.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectReasonCodeType", _culture).ToString();
			LabelTypeCompare.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectReasonCodeType", _culture).ToString();
			break;
		case AlarmReportType.Site:
			LabelType.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectSiteType", _culture).ToString();
			LabelTypeCompare.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectSiteType", _culture).ToString();
			break;
		case AlarmReportType.ResponseTime:
			LabelAdditionalType.Visible = true;
			DropDownListAdditionalType.Visible = true;
			LabelAdditionalTypeCompare.Visible = true;
			DropDownListAdditionalTypeCompare.Visible = true;
			AlarmGraph.Visible = false;
			AlarmGraphCompare.Visible = false;
			AlarmSummaryGraph.Visible = true;
			AlarmSummaryGraphCompare.Visible = true;
			DropDownListAdditionalType.SelectedValue = GetAlarmResponseTime().ToString();
			DropDownListAdditionalTypeCompare.SelectedValue = GetAlarmResponseTime(isCompareValue: true).ToString();
			LabelType.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectResponseTimeType", _culture).ToString();
			LabelTypeCompare.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectResponseTimeType", _culture).ToString();
			break;
		}
		List<AlarmSubReport> reports = new List<AlarmSubReport>
		{
			new AlarmSubReport
			{
				Name = string.Empty,
				ReportId = Guid.Empty
			}
		};
		AlarmProvider.GetSubReports(sessionId, reportType, base.AlarmTimescale, eventServer, _timeZoneInfo, _userToken, _culture.Name).ForEach(delegate(AlarmSubReport r)
		{
			reports.Add(r);
		});
		DropDownListType.DataSource = reports;
		DropDownListType.DataBind();
		DropDownListType.SelectedValue = GetSubReportType().ToString();
		DropDownListTypeCompare.DataSource = reports;
		DropDownListTypeCompare.DataBind();
		DropDownListTypeCompare.SelectedValue = GetSubReportType(isCompareValue: true).ToString();
		IEnumerable<ServerData> enumerable = from s in AlarmProvider.GetServers(sessionId, reportType, base.AlarmTimescale, eventServer, _timeZoneInfo, _userToken, _culture.Name)
			where s.StateField != "Online"
			select s;
		LabelIncomplete.Visible = enumerable.Any();
		LabelUnresponsive.Visible = enumerable.Any();
		DataGridServers.Visible = enumerable.Any();
		DataGridServers.DataSource = enumerable;
		DataGridServers.DataBind();
	}

	protected void DropDownListType_SelectedIndexChanged(object sender, EventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(base.Request.Url.Query);
		if (!Guid.TryParse(DropDownListType.SelectedValue.ToString(), out var result))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx{0}", base.Request.Url.Query));
		}
		nameValueCollection["SubReportType"] = result.ToString();
		base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString()));
	}

	protected void DropDownListTypeCompare_SelectedIndexChanged(object sender, EventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(base.Request.Url.Query);
		if (!Guid.TryParse(DropDownListTypeCompare.SelectedValue.ToString(), out var result))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx{0}", base.Request.Url.Query));
		}
		nameValueCollection["SubReportTypeCompare"] = result.ToString();
		base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString()));
	}

	protected void DropDownListAdditionalType_SelectedIndexChanged(object sender, EventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(base.Request.Url.Query);
		if (!Enum.TryParse<ResponseTime>(DropDownListAdditionalType.SelectedValue.ToString(), out var result))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx{0}", base.Request.Url.Query));
		}
		nameValueCollection["ResponseTime"] = result.ToString();
		base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString()));
	}

	protected void DropDownListAdditionalTypeCompare_SelectedIndexChanged(object sender, EventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(base.Request.Url.Query);
		if (!Enum.TryParse<ResponseTime>(DropDownListAdditionalTypeCompare.SelectedValue.ToString(), out var result))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx{0}", base.Request.Url.Query));
		}
		nameValueCollection["ResponseTimeCompare"] = result.ToString();
		base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString()));
	}

	private void UpdateGraph()
	{
		AlarmReportType alarmReportType = base.AlarmReportType;
		AlarmTimescale alarmTimescale = base.AlarmTimescale;
		if (alarmReportType.Equals(AlarmReportType.Empty))
		{
			return;
		}
		if (alarmReportType != AlarmReportType.ResponseTime)
		{
			SetAlarmGraphTitles(alarmTimescale);
			if (Guid.Parse(DropDownListType.SelectedValue) != Guid.Empty)
			{
				AlarmGraph.Refresh(alarmTimescale, AlarmProvider.GetHistoryData(sessionId, Guid.Parse(DropDownListType.SelectedValue), alarmReportType, alarmTimescale, eventServer, _timeZoneInfo, _userToken, _culture.Name));
			}
			else
			{
				AlarmGraph.Refresh(alarmTimescale, new List<HistoryData>
				{
					new HistoryData()
				});
			}
			if (Guid.Parse(DropDownListTypeCompare.SelectedValue) != Guid.Empty)
			{
				AlarmGraphCompare.Refresh(alarmTimescale, AlarmProvider.GetHistoryData(sessionId, Guid.Parse(DropDownListTypeCompare.SelectedValue), alarmReportType, alarmTimescale, eventServer, _timeZoneInfo, _userToken, _culture.Name));
				return;
			}
			AlarmGraphCompare.Refresh(alarmTimescale, new List<HistoryData>
			{
				new HistoryData()
			});
			return;
		}
		SetAlarmSummaryGraphTitle();
		SetAlarmSummaryCompareGraph();
		if (Guid.Parse(DropDownListType.SelectedValue) != Guid.Empty)
		{
			AlarmSummaryGraph.Refresh((ResponseTime)Enum.Parse(typeof(ResponseTime), DropDownListAdditionalType.SelectedValue), GetSummaryData(sessionId, Guid.Parse(DropDownListType.SelectedValue), alarmReportType, alarmTimescale, (ResponseTime)Enum.Parse(typeof(ResponseTime), DropDownListAdditionalType.SelectedValue), eventServer, _timeZoneInfo, _userToken, _culture.Name));
		}
		else
		{
			AlarmSummaryGraph.Refresh((ResponseTime)Enum.Parse(typeof(ResponseTime), DropDownListAdditionalType.SelectedValue), new List<SummaryData>
			{
				new SummaryData()
			});
		}
		if (Guid.Parse(DropDownListTypeCompare.SelectedValue) != Guid.Empty)
		{
			AlarmSummaryGraphCompare.Refresh((ResponseTime)Enum.Parse(typeof(ResponseTime), DropDownListAdditionalTypeCompare.SelectedValue), GetSummaryData(sessionId, Guid.Parse(DropDownListTypeCompare.SelectedValue), alarmReportType, alarmTimescale, (ResponseTime)Enum.Parse(typeof(ResponseTime), DropDownListAdditionalTypeCompare.SelectedValue), eventServer, _timeZoneInfo, _userToken, _culture.Name));
			return;
		}
		AlarmSummaryGraphCompare.Refresh((ResponseTime)Enum.Parse(typeof(ResponseTime), DropDownListAdditionalTypeCompare.SelectedValue), new List<SummaryData>
		{
			new SummaryData()
		});
	}

	private void SetAlarmSummaryCompareGraph()
	{
		AlarmSummaryGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYRespTime", _culture).ToString();
		switch ((ResponseTime)Enum.Parse(typeof(ResponseTime), DropDownListAdditionalTypeCompare.SelectedValue))
		{
		case ResponseTime.ThirtyMinutes:
			AlarmSummaryGraphCompare.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisMinutes", _culture).ToString();
			break;
		case ResponseTime.OneHour:
			AlarmSummaryGraphCompare.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisMinutes", _culture).ToString();
			break;
		case ResponseTime.TwoHours:
			AlarmSummaryGraphCompare.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisMinutes", _culture).ToString();
			break;
		case ResponseTime.EightHours:
			AlarmSummaryGraphCompare.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisHours", _culture).ToString();
			break;
		case ResponseTime.OneDay:
			AlarmSummaryGraphCompare.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisHours", _culture).ToString();
			break;
		case ResponseTime.TwoDays:
			AlarmSummaryGraphCompare.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisHours", _culture).ToString();
			break;
		case ResponseTime.OneWeek:
			AlarmSummaryGraphCompare.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisDays", _culture).ToString();
			break;
		}
	}

	private void SetAlarmSummaryGraphTitle()
	{
		AlarmSummaryGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYRespTime", _culture).ToString();
		switch ((ResponseTime)Enum.Parse(typeof(ResponseTime), DropDownListAdditionalType.SelectedValue))
		{
		case ResponseTime.ThirtyMinutes:
			AlarmSummaryGraph.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisMinutes", _culture).ToString();
			break;
		case ResponseTime.OneHour:
			AlarmSummaryGraph.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisMinutes", _culture).ToString();
			break;
		case ResponseTime.TwoHours:
			AlarmSummaryGraph.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisMinutes", _culture).ToString();
			break;
		case ResponseTime.EightHours:
			AlarmSummaryGraph.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisHours", _culture).ToString();
			break;
		case ResponseTime.OneDay:
			AlarmSummaryGraph.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisHours", _culture).ToString();
			break;
		case ResponseTime.TwoDays:
			AlarmSummaryGraph.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisHours", _culture).ToString();
			break;
		case ResponseTime.OneWeek:
			AlarmSummaryGraph.XAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelXAxisDays", _culture).ToString();
			break;
		}
	}

	private void SetAlarmGraphTitles(AlarmTimescale alarmTimescale)
	{
		AlarmGraph.XAxisTitle = HttpContext.GetGlobalResourceObject("Global", "Time", _culture).ToString();
		AlarmGraphCompare.XAxisTitle = HttpContext.GetGlobalResourceObject("Global", "Time", _culture).ToString();
		switch (alarmTimescale)
		{
		case AlarmTimescale.Day:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "lavelYAlarmsPerHour", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "lavelYAlarmsPerHour", _culture).ToString();
			break;
		case AlarmTimescale.Week:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerDay", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerDay", _culture).ToString();
			break;
		case AlarmTimescale.Month:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerDay", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerDay", _culture).ToString();
			break;
		case AlarmTimescale.SixMonth:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerMonth", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerMonth", _culture).ToString();
			break;
		case AlarmTimescale.Year:
			AlarmGraph.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerMonth", _culture).ToString();
			AlarmGraphCompare.YAxisTitle = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelYAlarmsPerMonth", _culture).ToString();
			break;
		}
	}

	private static List<SummaryData> GetSummaryData(Guid sessionId, Guid reportId, AlarmReportType reportType, AlarmTimescale alarmTimescale, ResponseTime responseTimes, string eventServerAddress, TimeZoneInfo timeZoneInfo, string token, string cultureName)
	{
		List<SummaryData> summaryData = AlarmProvider.GetSummaryData(sessionId, reportId, reportType, alarmTimescale, responseTimes, eventServerAddress, timeZoneInfo, token, cultureName);
		for (int i = 0; i < summaryData.Count; i++)
		{
			summaryData[i].SummaryStep += 0.5;
		}
		summaryData.Insert(0, new SummaryData
		{
			SummaryStep = 0.0,
			Value = 0.0
		});
		return summaryData;
	}
}
