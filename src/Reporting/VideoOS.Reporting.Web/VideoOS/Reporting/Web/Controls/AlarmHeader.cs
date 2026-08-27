using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Controls;

public class AlarmHeader : UserControl
{
	protected Label Header6;

	protected HyperLink hlDay;

	protected Label LabelDay;

	protected HyperLink hlWeek;

	protected Label LabelWeek;

	protected HyperLink hlMonth;

	protected Label LabelMonth;

	protected HyperLink hlSixMonth;

	protected Label LabelSixMonth;

	protected HyperLink hlYear;

	protected Label LabelYear;

	protected LinkButton LinkButtonPrint;

	protected Image ImagePrint;

	protected LinkButton LinkButtonRefresh;

	protected Image ImageRefresh;

	protected Label LabelSelectReportType;

	protected DropDownList DropDownListReport;

	public string HeaderText { get; set; }

	public string HostName { get; set; }

	public CultureInfo Culture { get; set; } = new CultureInfo("en-us");

	protected void Page_Load(object sender, EventArgs e)
	{
		Header6.Text = HeaderText;
		LabelSelectReportType.Text = HttpContext.GetGlobalResourceObject("AlarmStrings", "labelSelectReportType", Culture).ToString();
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(base.Request.Url.Query);
		nameValueCollection["GraphTimeScale"] = AlarmTimescale.Day.ToString();
		LabelDay.Text = HttpContext.GetGlobalResourceObject("AlarmGlobal", "Day", Culture).ToString();
		hlDay.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString());
		nameValueCollection["GraphTimeScale"] = AlarmTimescale.Week.ToString();
		LabelWeek.Text = HttpContext.GetGlobalResourceObject("AlarmGlobal", "Week", Culture).ToString();
		hlWeek.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString());
		nameValueCollection["GraphTimeScale"] = AlarmTimescale.Month.ToString();
		LabelMonth.Text = HttpContext.GetGlobalResourceObject("AlarmGlobal", "Month", Culture).ToString();
		hlMonth.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString());
		nameValueCollection["GraphTimeScale"] = AlarmTimescale.SixMonth.ToString();
		LabelSixMonth.Text = HttpContext.GetGlobalResourceObject("AlarmGlobal", "SixMonth", Culture).ToString();
		hlSixMonth.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString());
		nameValueCollection["GraphTimeScale"] = AlarmTimescale.Year.ToString();
		LabelYear.Text = HttpContext.GetGlobalResourceObject("AlarmGlobal", "Year", Culture).ToString();
		hlYear.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString());
		SetScale((Page as BasePage).AlarmTimescale);
		LinkButtonPrint.ToolTip = HttpContext.GetGlobalResourceObject("Global", "TTPrintPDF", Culture).ToString();
		LinkButtonRefresh.ToolTip = HttpContext.GetGlobalResourceObject("Global", "TTRefresh", Culture).ToString();
		if (!base.IsPostBack)
		{
			DropDownListReport.Items.Add(new ListItem(HttpContext.GetGlobalResourceObject("AlarmStrings", "ReportEmpty", Culture).ToString(), AlarmReportType.Empty.ToString()));
			DropDownListReport.Items.Add(new ListItem(HttpContext.GetGlobalResourceObject("AlarmStrings", "ReportCategory", Culture).ToString(), AlarmReportType.Category.ToString()));
			DropDownListReport.Items.Add(new ListItem(HttpContext.GetGlobalResourceObject("AlarmStrings", "ReportState", Culture).ToString(), AlarmReportType.State.ToString()));
			DropDownListReport.Items.Add(new ListItem(HttpContext.GetGlobalResourceObject("AlarmStrings", "ReportPriority", Culture).ToString(), AlarmReportType.Priority.ToString()));
			DropDownListReport.Items.Add(new ListItem(HttpContext.GetGlobalResourceObject("AlarmStrings", "ReportReasonCode", Culture).ToString(), AlarmReportType.ReasonCode.ToString()));
			DropDownListReport.Items.Add(new ListItem(HttpContext.GetGlobalResourceObject("AlarmStrings", "ReportSite", Culture).ToString(), AlarmReportType.Site.ToString()));
			DropDownListReport.Items.Add(new ListItem(HttpContext.GetGlobalResourceObject("AlarmStrings", "ReportResponseTime", Culture).ToString(), AlarmReportType.ResponseTime.ToString()));
			DropDownListReport.SelectedValue = (Page as BasePage).AlarmReportType.ToString();
		}
	}

	private void SetScale(AlarmTimescale scale)
	{
		switch (scale)
		{
		case AlarmTimescale.Day:
			LabelDay.CssClass = "MenuBlockSelected ThemeAlignCenter";
			LabelWeek.CssClass = "MenuBlock ThemeAlignCenter";
			LabelMonth.CssClass = "MenuBlock ThemeAlignCenter";
			LabelSixMonth.CssClass = "MenuBlock ThemeAlignCenter";
			LabelYear.CssClass = "MenuBlock ThemeAlignCenter";
			break;
		case AlarmTimescale.Week:
			LabelDay.CssClass = "MenuBlock ThemeAlignCenter";
			LabelWeek.CssClass = "MenuBlockSelected ThemeAlignCenter";
			LabelMonth.CssClass = "MenuBlock ThemeAlignCenter";
			LabelSixMonth.CssClass = "MenuBlock ThemeAlignCenter";
			LabelYear.CssClass = "MenuBlock ThemeAlignCenter";
			break;
		case AlarmTimescale.Month:
			LabelDay.CssClass = "MenuBlock ThemeAlignCenter";
			LabelWeek.CssClass = "MenuBlock ThemeAlignCenter";
			LabelMonth.CssClass = "MenuBlockSelected ThemeAlignCenter";
			LabelSixMonth.CssClass = "MenuBlock ThemeAlignCenter";
			LabelYear.CssClass = "MenuBlock ThemeAlignCenter";
			break;
		case AlarmTimescale.SixMonth:
			LabelDay.CssClass = "MenuBlock ThemeAlignCenter";
			LabelWeek.CssClass = "MenuBlock ThemeAlignCenter";
			LabelMonth.CssClass = "MenuBlock ThemeAlignCenter";
			LabelSixMonth.CssClass = "MenuBlockSelected ThemeAlignCenter";
			LabelYear.CssClass = "MenuBlock ThemeAlignCenter";
			break;
		case AlarmTimescale.Year:
			LabelDay.CssClass = "MenuBlock ThemeAlignCenter";
			LabelWeek.CssClass = "MenuBlock ThemeAlignCenter";
			LabelMonth.CssClass = "MenuBlock ThemeAlignCenter";
			LabelSixMonth.CssClass = "MenuBlock ThemeAlignCenter";
			LabelYear.CssClass = "MenuBlockSelected ThemeAlignCenter";
			break;
		}
	}

	protected void DropDownListReport_SelectedIndexChanged(object sender, EventArgs e)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(base.Request.Url.Query);
		if (!Enum.TryParse<AlarmReportType>(DropDownListReport.SelectedValue.ToString(), out var result))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx{0}", base.Request.Url.Query));
		}
		nameValueCollection["ReportType"] = result.ToString();
		base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString()));
	}

	protected void LinkButtonPrint_Click(object sender, EventArgs e)
	{
		HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
		using PdfDocument pdfDocument = htmlToPdfConverter.Convert(Regex.Replace(base.Request.Url.ToString(), "ThemeType=Dark", "ThemeType=Light", RegexOptions.IgnoreCase));
		pdfDocument.Save("Report.pdf", HttpContext.Current.Response, HttpReadType.Save);
		pdfDocument.Close(completely: true);
	}

	protected void LinkButtonRefresh_Click(object sender, EventArgs e)
	{
		if (Guid.TryParse(base.Request.QueryString["SessionId"], out var result))
		{
			AlarmProvider.ClearAlarmCache(result);
			base.Response.Redirect(base.Request.Url.ToString());
		}
	}
}
