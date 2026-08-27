using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Controls;

public class HistoryHeader : UserControl
{
	protected Label Header1;

	protected Image ImageServer;

	protected Image ImageCamera;

	protected Label LabelServerName;

	protected HyperLink hlDay;

	protected Label LabelDay;

	protected HyperLink hlWeek;

	protected Label LabelWeek;

	protected HyperLink hlMonth;

	protected Label LabelMonth;

	protected LinkButton LinkButtonPrint;

	protected Image ImagePrint;

	protected HyperLink hlRefresh;

	protected Image ImageRefresh;

	public string HeaderText { get; set; }

	public ReportType ReportType { get; set; }

	public string HostName { get; set; }

	public CultureInfo Culture { get; set; } = new CultureInfo("en-us");

	protected void Page_Load(object sender, EventArgs e)
	{
		Header1.Text = HeaderText;
		LabelServerName.Text = HostName;
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(base.Request.Url.Query);
		nameValueCollection["GraphTimeScale"] = Timescale.Day.ToString();
		LabelDay.Text = HttpContext.GetGlobalResourceObject("Global", "Day", Culture).ToString();
		hlDay.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString());
		nameValueCollection["GraphTimeScale"] = Timescale.Week.ToString();
		LabelWeek.Text = HttpContext.GetGlobalResourceObject("Global", "Week", Culture).ToString();
		hlWeek.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString());
		nameValueCollection["GraphTimeScale"] = Timescale.Month.ToString();
		LabelMonth.Text = HttpContext.GetGlobalResourceObject("Global", "Month", Culture).ToString();
		hlMonth.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "{0}?{1}", base.Request.Url.AbsolutePath, nameValueCollection.ToString());
		SetScale((Page as BasePage).Timescale);
		LinkButtonPrint.ToolTip = HttpContext.GetGlobalResourceObject("Global", "TTPrintPDF", Culture).ToString();
		hlRefresh.ToolTip = HttpContext.GetGlobalResourceObject("Global", "TTRefresh", Culture).ToString();
		hlRefresh.NavigateUrl = base.Request.Url.ToString();
		switch (ReportType)
		{
		case ReportType.Server:
			ImageServer.Visible = true;
			break;
		case ReportType.Camera:
			ImageCamera.Visible = true;
			break;
		}
	}

	private void SetScale(Timescale scale)
	{
		switch (scale)
		{
		case Timescale.Day:
			LabelDay.CssClass = "MenuBlockSelected ThemeAlignCenter";
			LabelWeek.CssClass = "MenuBlock ThemeAlignCenter";
			LabelMonth.CssClass = "MenuBlock ThemeAlignCenter";
			break;
		case Timescale.Week:
			LabelDay.CssClass = "MenuBlock ThemeAlignCenter";
			LabelWeek.CssClass = "MenuBlockSelected ThemeAlignCenter";
			LabelMonth.CssClass = "MenuBlock ThemeAlignCenter";
			break;
		case Timescale.Month:
			LabelDay.CssClass = "MenuBlock ThemeAlignCenter";
			LabelWeek.CssClass = "MenuBlock ThemeAlignCenter";
			LabelMonth.CssClass = "MenuBlockSelected ThemeAlignCenter";
			break;
		}
	}

	protected void LinkButtonPrint_Click(object sender, EventArgs e)
	{
		HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
		PdfDocument pdfDocument = htmlToPdfConverter.Convert(Regex.Replace(base.Request.Url.ToString(), "ThemeType=Dark", "ThemeType=Light", RegexOptions.IgnoreCase));
		pdfDocument.Save("Report.pdf", HttpContext.Current.Response, HttpReadType.Save);
		pdfDocument.Close(completely: true);
	}
}
