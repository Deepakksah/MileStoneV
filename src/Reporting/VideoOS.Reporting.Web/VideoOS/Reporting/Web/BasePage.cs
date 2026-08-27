using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using VideoOS.Management.Common;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Settings;

namespace VideoOS.Reporting.Web;

public class BasePage : Page
{
	protected string _userToken;

	protected CultureInfo _culture;

	protected string _queryString = string.Empty;

	protected TimeZoneInfo _timeZoneInfo;

	protected TimeSpan _offset;

	protected virtual string NamespaceIdentifier => SecurityNamespaceIdentifier.SystemMonitor.ToString();

	protected virtual string SecurityAction => "GENERIC_READ";

	protected virtual string SecurityObject => "/";

	public string TimeZoneInfo => string.Format(CultureInfo.InvariantCulture, "{0} GMT{1} {2}", HttpContext.GetGlobalResourceObject("Global", "TimeZoneText", _culture).ToString(), _offset.TotalHours.ToString("+00;-00", _culture), HttpContext.GetGlobalResourceObject("Global", "TimeHours", _culture).ToString());

	public Timescale Timescale => base.Request.QueryString["GraphTimeScale"]?.ToString().ToUpperInvariant() switch
	{
		"DAY" => Timescale.Day, 
		"MONTH" => Timescale.Month, 
		_ => Timescale.Week, 
	};

	public AlarmTimescale AlarmTimescale => base.Request.QueryString["GraphTimeScale"]?.ToString().ToUpperInvariant() switch
	{
		"DAY" => AlarmTimescale.Day, 
		"MONTH" => AlarmTimescale.Month, 
		"SIXMONTH" => AlarmTimescale.SixMonth, 
		"YEAR" => AlarmTimescale.Year, 
		_ => AlarmTimescale.Week, 
	};

	public AlarmReportType AlarmReportType => base.Request.QueryString["ReportType"]?.ToString().ToUpperInvariant() switch
	{
		"EMPTY" => AlarmReportType.Empty, 
		"CATEGORY" => AlarmReportType.Category, 
		"STATE" => AlarmReportType.State, 
		"PRIORITY" => AlarmReportType.Priority, 
		"REASONCODE" => AlarmReportType.ReasonCode, 
		"SITE" => AlarmReportType.Site, 
		"RESPONSETIME" => AlarmReportType.ResponseTime, 
		_ => AlarmReportType.Empty, 
	};

	protected void Page_PreInit(object sender, EventArgs e)
	{
		_culture = new CultureInfo(base.Request.QueryString["Culture"] ?? "en-us");
		((BaseMaster)base.Master).ErrorHeaderText = HttpContext.GetGlobalResourceObject("ErrorPage", "ErrorHeader", _culture).ToString();
		string text = base.Request.QueryString["ThemeType"]?.ToString().ToUpperInvariant();
		if (!(text == "DARK"))
		{
			if (!(text == "LIGHT"))
			{
			}
			Page.Theme = "Light";
		}
		else
		{
			Page.Theme = "Dark";
		}
		_queryString = string.Format(CultureInfo.InvariantCulture, "?ThemeType={0}&Culture={1}&Id={2}&TimeZone={3}&UserToken={4}&GraphTimeScale={5}", Page.Theme, _culture.Name, base.Request.QueryString["Id"], base.Request.QueryString["TimeZone"], base.Server.UrlEncode(base.Request.QueryString["UserToken"]), base.Request.QueryString["GraphTimeScale"]);
		try
		{
			_timeZoneInfo = System.TimeZoneInfo.FindSystemTimeZoneById(base.Request.QueryString["TimeZone"]);
			_offset = _timeZoneInfo.GetUtcOffset(DateTime.Now);
			if (ConfigSettings.Secure && !base.Request.IsSecureConnection)
			{
				base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=Unauthorized{0}", _queryString.Replace("?", "&")));
			}
		}
		catch
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=InvalidTimeZone{0}", _queryString.Replace("?", "&")));
		}
	}

	protected void Page_Init(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(base.Request.QueryString["UserToken"]))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=Unauthorized{0}", _queryString.Replace("?", "&")));
		}
		else
		{
			_userToken = base.Request.QueryString["UserToken"];
		}
		if (!((BaseMaster)base.Master).IsAsyncPostBack)
		{
			IdentityTokenInfo identityTokenInfo = ReportSecurity.AuthenticateToken(_userToken);
			if (identityTokenInfo == null || !ReportSecurity.AuthorizedToRead(identityTokenInfo, NamespaceIdentifier, SecurityAction, SecurityObject))
			{
				base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=Unauthorized{0}", _queryString.Replace("?", "&")));
			}
		}
	}

	public Guid GetSubReportType()
	{
		return GetSubReportType(isCompareValue: false);
	}

	public Guid GetSubReportType(bool isCompareValue)
	{
		if (!Guid.TryParse(base.Request.QueryString[isCompareValue ? "SubReportTypeCompare" : "SubReportType"]?.ToString(), out var result))
		{
			return Guid.NewGuid();
		}
		return result;
	}

	public ResponseTime GetAlarmResponseTime()
	{
		return GetAlarmResponseTime(isCompareValue: false);
	}

	public ResponseTime GetAlarmResponseTime(bool isCompareValue)
	{
		return base.Request.QueryString[isCompareValue ? "ResponseTimeCompare" : "ResponseTime"]?.ToString().ToUpperInvariant() switch
		{
			"THIRTYMINUTES" => ResponseTime.ThirtyMinutes, 
			"TWOHOURS" => ResponseTime.TwoHours, 
			"EIGHTHOURS" => ResponseTime.EightHours, 
			"ONEDAY" => ResponseTime.OneDay, 
			"TWODAYS" => ResponseTime.TwoDays, 
			"ONEWEEK" => ResponseTime.OneWeek, 
			_ => ResponseTime.OneHour, 
		};
	}
}
