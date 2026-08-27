using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Controls;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web;

public class ServerReport : BasePage
{
	protected Guid serverId;

	protected int dataCollectorId;

	protected string serverType;

	protected string hostName;

	protected Timer Timer;

	protected Label LabelServerName;

	protected UpdatePanel CPUUsage_UpdatePanel;

	protected LiveCounter CPUUsage;

	protected UpdatePanel MemoryAvailable_UpdatePanel;

	protected LiveCounter MemoryAvailable;

	protected HyperLink hlHistory;

	protected Label LabelHistory;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Guid.TryParse(base.Request.QueryString["Id"], out serverId))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		ServerOverview serverOverview = Database.ServerOverview(serverId);
		if (serverOverview == null)
		{
			base.Response.Redirect("ErrorPage.aspx");
			return;
		}
		dataCollectorId = serverOverview.DataCollectorId;
		serverType = HttpContext.GetGlobalResourceObject("Global", serverOverview.ServerType, _culture)?.ToString();
		hostName = serverOverview.HostName;
		if (!Page.IsPostBack)
		{
			if (!string.IsNullOrEmpty(serverType))
			{
				LabelServerName.Text = string.Format(CultureInfo.InvariantCulture, "{0} ( {1} )", serverType, hostName);
			}
			else
			{
				LabelServerName.Text = hostName;
			}
			CPUUsage.Title = HttpContext.GetGlobalResourceObject("CPU", "ChartTitle", _culture).ToString();
			CPUUsage.ErrorText = HttpContext.GetGlobalResourceObject("Global", "lblError", _culture).ToString();
			MemoryAvailable.Title = HttpContext.GetGlobalResourceObject("Memory", "ChartTitle", _culture).ToString();
			MemoryAvailable.ErrorText = HttpContext.GetGlobalResourceObject("Global", "lblError", _culture).ToString();
			hlHistory.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "~/HistoryServer.aspx{0}", _queryString);
			LabelHistory.Text = HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString();
		}
	}

	protected void Timer_Tick(object sender, EventArgs e)
	{
		ILiveResult liveCpu = LiveDataProvider.GetLiveCpu(hostName);
		if (liveCpu.DisplayUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
		{
			CPUUsage.Refresh(liveCpu.ValueInPercent, string.Format(_culture, HttpContext.GetGlobalResourceObject("CPU", "Units", _culture).ToString(), liveCpu.DisplayValue.ToString("0", _culture)), liveCpu.Error, WarningCriteria.Above);
		}
		int maxPhysicalMemory = Database.GetMaxPhysicalMemory(dataCollectorId);
		ILiveResult liveMemory = LiveDataProvider.GetLiveMemory(hostName, maxPhysicalMemory);
		if (liveMemory.DisplayUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
		{
			MemoryAvailable.Refresh(liveMemory.ValueInPercent, string.Format(CultureInfo.InvariantCulture, "{0} {1}", liveMemory.DisplayValue.ToString("N", _culture), liveMemory.DisplayUnit), liveMemory.Error, WarningCriteria.Below);
		}
	}
}
