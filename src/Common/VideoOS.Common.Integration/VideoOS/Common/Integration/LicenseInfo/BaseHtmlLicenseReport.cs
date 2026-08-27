using System.Globalization;
using System.Text;

namespace VideoOS.Common.Integration.LicenseInfo;

public class BaseHtmlLicenseReport
{
	private const string HeadBlock = "<html xmlns=\"http://www.w3.org/1999/xhtml\" >\n<head>\n<meta charset=\"utf-8\" />\n<title>{0}</title>\n</head>\n";

	private const string SpanImageClass = "<span class=\"{0}\"><img src=\"{1}\"></span>\n";

	private const string SpanImageNoClass = "<span><img src=\"{0}\"></span>\n";

	private const string SpanSvgImageClass = "<span class=\"{0}\">{1}</span>\n";

	private const string SpanSvgImageNoClass = "<span>{0}</span>\n";

	private const string NodePWithClass = "<p class=\"{0}\">{1}</p>\n";

	private const string NodePNoClass = "<p>{0}</p>\n";

	private const string TableRowStart = "<tr>\n";

	private const string TableRowEnd = "</tr>\n";

	private const string TableHeaderCellStart = "<th>\n";

	private const string TableHeaderCellEnd = "</th>\n";

	private const string TableCellStart = "<td>\n";

	private const string TableCellEnd = "</td>\n";

	protected const string HtmlDocType = "<!DOCTYPE html>\n";

	protected const string HtmlEnd = "</html>\n";

	protected const string BodyStart = "<body>\n";

	protected const string BodyEnd = "</body>\n";

	protected const string NodeH1 = "<h1>{0}</h1>\n";

	protected const string NodeH2 = "<h2>{0}</h2>\n";

	protected const string NodeH3 = "<h3>{0}</h3>\n";

	protected const string NodeH6 = "<h6>{0}</h6>\n";

	protected const string TableStart = "<table>\n";

	protected const string TableEnd = "</table>\n";

	protected const string WarningIconSVG = "<svg width=\"44px\" height=\"37px\" viewBox=\"0 0 44 37\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"><title>ic-warning</title><g id=\"✅-Installer-(incl-playground)\" stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\"><g id=\"MISSING-licenses---HTML-file-opened-in-browser-960-grid\" transform=\"translate(-248.000000, -120.000000)\" fill=\"#F2741B\"><path d=\"M270,120 C271.472039,120 272.837242,120.746071 273.600683,121.967742 L273.600683,121.967742 L291.435643,150.868333 C292.183907,152.126141 292.188375,153.674893 291.447378,154.936749 C290.706382,156.198605 289.331218,156.984037 287.83496,157 L287.83496,157 L252.16504,157 C250.668782,156.984037 249.293618,156.198605 248.552622,154.936749 C247.811625,153.674893 247.816093,152.126141 248.564357,150.868333 L248.564357,150.868333 L266.399317,121.967742 C267.162758,120.746071 268.527961,120 270,120 Z M270.00995,147 L269.757968,147.013455 C268.768239,147.128986 268,147.974328 268,149 C268,150.104569 268.890976,151 269.99005,151 L269.99005,151 L270.242032,150.986545 C271.231761,150.871014 272,150.025672 272,149 C272,147.895431 271.109024,147 270.00995,147 L270.00995,147 Z M269.99005,131 C268.890976,131 268,131.895431 268,133 L268,133 L268,141 L268.013389,141.233242 C268.128344,142.22792 268.969481,143 269.99005,143 C271.089124,143 271.9801,142.104569 271.9801,141 L271.9801,141 L271.9801,133 L271.966711,132.766758 C271.851756,131.77208 271.010619,131 269.99005,131 Z\" id=\"ic-warning\"></path></g></g></svg>";

	protected static string CreateHtmlHeader(string title)
	{
		if (title == null)
		{
			title = string.Empty;
		}
		return string.Format(CultureInfo.CurrentCulture, "<html xmlns=\"http://www.w3.org/1999/xhtml\" >\n<head>\n<meta charset=\"utf-8\" />\n<title>{0}</title>\n</head>\n", title);
	}

	protected static string CreateSvgImageInSpan(string className, string svgImageSource)
	{
		if (svgImageSource == null)
		{
			svgImageSource = string.Empty;
		}
		if (string.IsNullOrEmpty(className))
		{
			return string.Format(CultureInfo.CurrentCulture, "<span>{0}</span>\n", svgImageSource);
		}
		return string.Format(CultureInfo.CurrentCulture, "<span class=\"{0}\">{1}</span>\n", className, svgImageSource);
	}

	protected static string CreateImageInSpan(string className, string imageLocation)
	{
		if (imageLocation == null)
		{
			imageLocation = string.Empty;
		}
		if (string.IsNullOrEmpty(className))
		{
			return string.Format(CultureInfo.CurrentCulture, "<span><img src=\"{0}\"></span>\n", imageLocation);
		}
		return string.Format(CultureInfo.CurrentCulture, "<span class=\"{0}\"><img src=\"{1}\"></span>\n", className, imageLocation);
	}

	protected static string CreatePNode(string className, string text)
	{
		if (text == null)
		{
			text = string.Empty;
		}
		if (string.IsNullOrEmpty(className))
		{
			return string.Format(CultureInfo.CurrentCulture, "<p>{0}</p>\n", text);
		}
		return string.Format(CultureInfo.CurrentCulture, "<p class=\"{0}\">{1}</p>\n", className, text);
	}

	protected static string CreateTableRow(params string[] columnHeaders)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		stringBuilder.Append("<tr>\n");
		foreach (string value in columnHeaders)
		{
			stringBuilder.Append("<td>\n");
			stringBuilder.Append(value);
			stringBuilder.Append("</td>\n");
		}
		stringBuilder.Append("</tr>\n");
		return stringBuilder.ToString();
	}

	protected static string CreateTableHeaderRow(params string[] columnHeaders)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		stringBuilder.Append("<tr>\n");
		foreach (string value in columnHeaders)
		{
			stringBuilder.Append("<th>\n");
			stringBuilder.Append(value);
			stringBuilder.Append("</th>\n");
		}
		stringBuilder.Append("</tr>\n");
		return stringBuilder.ToString();
	}
}
