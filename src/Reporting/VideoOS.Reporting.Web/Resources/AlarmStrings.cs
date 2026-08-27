using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Resources;

[GeneratedCode("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class AlarmStrings
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager resourceManager = new ResourceManager("Resources.AlarmStrings", Assembly.Load("App_GlobalResources"));
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string dataSizeDay => ResourceManager.GetString("dataSizeDay", resourceCulture);

	public static string dataSizeDays => ResourceManager.GetString("dataSizeDays", resourceCulture);

	public static string dataSizeHour => ResourceManager.GetString("dataSizeHour", resourceCulture);

	public static string dataSizeHours => ResourceManager.GetString("dataSizeHours", resourceCulture);

	public static string dataSizeMinutes => ResourceManager.GetString("dataSizeMinutes", resourceCulture);

	public static string dataSizeWeek => ResourceManager.GetString("dataSizeWeek", resourceCulture);

	public static string ErrorOccurred => ResourceManager.GetString("ErrorOccurred", resourceCulture);

	public static string interval1Day => ResourceManager.GetString("interval1Day", resourceCulture);

	public static string interval1Month => ResourceManager.GetString("interval1Month", resourceCulture);

	public static string interval1Week => ResourceManager.GetString("interval1Week", resourceCulture);

	public static string interval1Year => ResourceManager.GetString("interval1Year", resourceCulture);

	public static string interval6Months => ResourceManager.GetString("interval6Months", resourceCulture);

	public static string labelDataSize => ResourceManager.GetString("labelDataSize", resourceCulture);

	public static string labelSelectCategoryType => ResourceManager.GetString("labelSelectCategoryType", resourceCulture);

	public static string labelSelectInterval => ResourceManager.GetString("labelSelectInterval", resourceCulture);

	public static string labelSelectPriorityType => ResourceManager.GetString("labelSelectPriorityType", resourceCulture);

	public static string labelSelectReasonCodeType => ResourceManager.GetString("labelSelectReasonCodeType", resourceCulture);

	public static string labelSelectReportType => ResourceManager.GetString("labelSelectReportType", resourceCulture);

	public static string labelSelectResponseTimeType => ResourceManager.GetString("labelSelectResponseTimeType", resourceCulture);

	public static string labelSelectSiteType => ResourceManager.GetString("labelSelectSiteType", resourceCulture);

	public static string labelSelectStateType => ResourceManager.GetString("labelSelectStateType", resourceCulture);

	public static string labelXAxisDays => ResourceManager.GetString("labelXAxisDays", resourceCulture);

	public static string labelXAxisHours => ResourceManager.GetString("labelXAxisHours", resourceCulture);

	public static string labelXAxisMinutes => ResourceManager.GetString("labelXAxisMinutes", resourceCulture);

	public static string labelYAlarmsPerDay => ResourceManager.GetString("labelYAlarmsPerDay", resourceCulture);

	public static string labelYAlarmsPerMonth => ResourceManager.GetString("labelYAlarmsPerMonth", resourceCulture);

	public static string labelYDefault => ResourceManager.GetString("labelYDefault", resourceCulture);

	public static string labelYRespTime => ResourceManager.GetString("labelYRespTime", resourceCulture);

	public static string lavelYAlarmsPerHour => ResourceManager.GetString("lavelYAlarmsPerHour", resourceCulture);

	public static string ReportCategory => ResourceManager.GetString("ReportCategory", resourceCulture);

	public static string ReportEmpty => ResourceManager.GetString("ReportEmpty", resourceCulture);

	public static string reportNameCategory => ResourceManager.GetString("reportNameCategory", resourceCulture);

	public static string reportNameEmpty => ResourceManager.GetString("reportNameEmpty", resourceCulture);

	public static string reportNamePriority => ResourceManager.GetString("reportNamePriority", resourceCulture);

	public static string reportNameReasonCode => ResourceManager.GetString("reportNameReasonCode", resourceCulture);

	public static string reportNameResponseTime => ResourceManager.GetString("reportNameResponseTime", resourceCulture);

	public static string reportNameSite => ResourceManager.GetString("reportNameSite", resourceCulture);

	public static string reportNameState => ResourceManager.GetString("reportNameState", resourceCulture);

	public static string ReportPriority => ResourceManager.GetString("ReportPriority", resourceCulture);

	public static string ReportReasonCode => ResourceManager.GetString("ReportReasonCode", resourceCulture);

	public static string ReportResponseTime => ResourceManager.GetString("ReportResponseTime", resourceCulture);

	public static string ReportSite => ResourceManager.GetString("ReportSite", resourceCulture);

	public static string ReportState => ResourceManager.GetString("ReportState", resourceCulture);

	public static string ServerName => ResourceManager.GetString("ServerName", resourceCulture);

	public static string ServerState => ResourceManager.GetString("ServerState", resourceCulture);

	public static string textYAxisDay => ResourceManager.GetString("textYAxisDay", resourceCulture);

	public static string textYAxisHalfYear => ResourceManager.GetString("textYAxisHalfYear", resourceCulture);

	public static string textYAxisHour => ResourceManager.GetString("textYAxisHour", resourceCulture);

	public static string textYAxisMonth => ResourceManager.GetString("textYAxisMonth", resourceCulture);

	public static string textYAxisWeek => ResourceManager.GetString("textYAxisWeek", resourceCulture);

	public static string textYAxisYear => ResourceManager.GetString("textYAxisYear", resourceCulture);

	public AlarmStrings()
	{
	}
}
