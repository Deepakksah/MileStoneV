using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace VideoOS.Reporting.Web;

public class ErrorPage : Page
{
	protected CultureInfo culture = new CultureInfo("en-us");

	protected HtmlGenericControl h1ErrorHeader;

	protected Label LabelErrorText;

	protected void Page_PreInit(object sender, EventArgs e)
	{
		culture = new CultureInfo(base.Request.QueryString["Culture"] ?? "en-us");
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
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		h1ErrorHeader.InnerText = HttpContext.GetGlobalResourceObject("ErrorPage", "ErrorHeader", culture).ToString();
		switch (base.Request.QueryString["Error"]?.ToString().ToUpperInvariant())
		{
		case "UNAUTHORIZED":
			LabelErrorText.Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", HttpContext.GetGlobalResourceObject("ErrorPage", "ErrorDescription", culture).ToString(), HttpContext.GetGlobalResourceObject("AlarmErrorMessages", "ErrorMsg7", culture).ToString());
			break;
		default:
			LabelErrorText.Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", HttpContext.GetGlobalResourceObject("ErrorPage", "ErrorDescription", culture).ToString(), HttpContext.GetGlobalResourceObject("ErrorPage", "GeneralError_SystemError", culture).ToString());
			break;
		}
	}
}
