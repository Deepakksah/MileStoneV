using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using VideoOS.Common.Integration.Properties;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public class LicenseWarningReportGenerator : BaseHtmlLicenseReport, ILicenseReportGenerator
{
	private const string WarningReportStyles = "<style>\nbody {width:960px;margin:80px auto 160px auto;}\nh1 {font-family: \"Open Sans\",Sans-Serif;font-size: 32px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 0;}\nh2 {font-family: \"Open Sans\",Sans-Serif;font-size: 20px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 24px 0 2px 75px;}\nh3 {font-family: \"Open Sans\",Sans-Serif;font-size: 20px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 60px 0 0 0;max-width: 787px;}\nh6 {font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 32px 0 0 0;}\n.warning {float:left;display: block;margin: 42px 30px 30px 0; }\n.hint {font-family: \"Open Sans\",Sans-Serif;font-size: 11px;font-weight: normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #7f7f7f;}\n.headerText {font-family: \"Open Sans\",Sans-Serif;font-size: 16px;margin: 0 0 5px 87px;}\ntable {border-collapse: collapse;border-spacing: 0;width: 100%;}\nth {text-align: left;padding: 16px;background-color: #023852;font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #ffffff;}\ntd {text-align: left;padding: 16px;font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight:normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;}\ntr {text-align: left;padding: 16px;background-color: #ffffff;font-family: \"Open Sans\",Sans-Serif;font-size: 12px;font-weight: normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;}\ntr:nth-child(even) {padding: 16px;background-color: #f3f3f3;}\n@media print and (color) {* {-webkit-print-color-adjust: exact;print-color-adjust: exact;}}\n</style>\n";

	public byte[] GenerateReport(IEnumerable<ILicenseCalculationResult> calculationResults, ILicenseDocument licenseDocument, Dictionary<string, string> reportProperties)
	{
		if (calculationResults == null)
		{
			throw new ArgumentNullException("calculationResults");
		}
		if (licenseDocument == null)
		{
			throw new ArgumentNullException("licenseDocument");
		}
		if (reportProperties == null)
		{
			throw new ArgumentNullException("reportProperties");
		}
		return Encoding.UTF8.GetBytes(GenerateWarningReport(calculationResults, licenseDocument, reportProperties));
	}

	private static string GenerateWarningReport(IEnumerable<ILicenseCalculationResult> calculationResults, ILicenseDocument licenseDocument, Dictionary<string, string> reportProperties)
	{
		string text = string.Empty;
		if (reportProperties.ContainsKey("OemLink"))
		{
			text = reportProperties["OemLink"];
			if (!Uri.IsWellFormedUriString(text, UriKind.Absolute))
			{
				text = string.Empty;
			}
		}
		int num = 0;
		int num2 = calculationResults.Sum((ILicenseCalculationResult cr) => cr.ChannelLicenses) - licenseDocument.ChannelLicenseCount;
		List<ILicenseCalculationResult> list = calculationResults.OrderByDescending((ILicenseCalculationResult o) => o.ChannelLicenses).ToList();
		StringBuilder stringBuilder = new StringBuilder("<!DOCTYPE html>\n", 4096);
		stringBuilder.Append(BaseHtmlLicenseReport.CreateHtmlHeader(ReportResources.WarningTitle));
		stringBuilder.Append("<body>\n");
		stringBuilder.Append("<style>\nbody {width:960px;margin:80px auto 160px auto;}\nh1 {font-family: \"Open Sans\",Sans-Serif;font-size: 32px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 0;}\nh2 {font-family: \"Open Sans\",Sans-Serif;font-size: 20px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 24px 0 2px 75px;}\nh3 {font-family: \"Open Sans\",Sans-Serif;font-size: 20px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 60px 0 0 0;max-width: 787px;}\nh6 {font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 32px 0 0 0;}\n.warning {float:left;display: block;margin: 42px 30px 30px 0; }\n.hint {font-family: \"Open Sans\",Sans-Serif;font-size: 11px;font-weight: normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #7f7f7f;}\n.headerText {font-family: \"Open Sans\",Sans-Serif;font-size: 16px;margin: 0 0 5px 87px;}\ntable {border-collapse: collapse;border-spacing: 0;width: 100%;}\nth {text-align: left;padding: 16px;background-color: #023852;font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #ffffff;}\ntd {text-align: left;padding: 16px;font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight:normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;}\ntr {text-align: left;padding: 16px;background-color: #ffffff;font-family: \"Open Sans\",Sans-Serif;font-size: 12px;font-weight: normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;}\ntr:nth-child(even) {padding: 16px;background-color: #f3f3f3;}\n@media print and (color) {* {-webkit-print-color-adjust: exact;print-color-adjust: exact;}}\n</style>\n");
		stringBuilder.Append(BaseHtmlLicenseReport.CreateSvgImageInSpan("warning", "<svg width=\"44px\" height=\"37px\" viewBox=\"0 0 44 37\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"><title>ic-warning</title><g id=\"✅-Installer-(incl-playground)\" stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\"><g id=\"MISSING-licenses---HTML-file-opened-in-browser-960-grid\" transform=\"translate(-248.000000, -120.000000)\" fill=\"#F2741B\"><path d=\"M270,120 C271.472039,120 272.837242,120.746071 273.600683,121.967742 L273.600683,121.967742 L291.435643,150.868333 C292.183907,152.126141 292.188375,153.674893 291.447378,154.936749 C290.706382,156.198605 289.331218,156.984037 287.83496,157 L287.83496,157 L252.16504,157 C250.668782,156.984037 249.293618,156.198605 248.552622,154.936749 C247.811625,153.674893 247.816093,152.126141 248.564357,150.868333 L248.564357,150.868333 L266.399317,121.967742 C267.162758,120.746071 268.527961,120 270,120 Z M270.00995,147 L269.757968,147.013455 C268.768239,147.128986 268,147.974328 268,149 C268,150.104569 268.890976,151 269.99005,151 L269.99005,151 L270.242032,150.986545 C271.231761,150.871014 272,150.025672 272,149 C272,147.895431 271.109024,147 270.00995,147 L270.00995,147 Z M269.99005,131 C268.890976,131 268,131.895431 268,133 L268,133 L268,141 L268.013389,141.233242 C268.128344,142.22792 268.969481,143 269.99005,143 C271.089124,143 271.9801,142.104569 271.9801,141 L271.9801,141 L271.9801,133 L271.966711,132.766758 C271.851756,131.77208 271.010619,131 269.99005,131 Z\" id=\"ic-warning\"></path></g></g></svg>"));
		stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<h1>{0}</h1>\n", ReportResources.HeaderMissingLicenses));
		stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<h2>{0}</h2>\n", string.Format(CultureInfo.CurrentCulture, ReportResources.TotalPurchasedLicenses, licenseDocument.ChannelLicenseCount)));
		stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<h2>{0}</h2>\n", string.Format(CultureInfo.CurrentCulture, ReportResources.TotalMissingLicenses, num2)));
		if (!string.IsNullOrEmpty(text))
		{
			stringBuilder.Append(BaseHtmlLicenseReport.CreatePNode("headerText", string.Format(CultureInfo.CurrentCulture, ReportResources.SupportedDeviceListLink, text)));
			stringBuilder.Append(BaseHtmlLicenseReport.CreatePNode("headerText", ReportResources.ContactResellerAdditionalLicenses));
		}
		stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<h3>{0}</h3>\n", ReportResources.DeviceListHeader));
		stringBuilder.Append(BaseHtmlLicenseReport.CreatePNode("hint", ReportResources.PrintHint));
		stringBuilder.Append("<table>\n");
		stringBuilder.Append(BaseHtmlLicenseReport.CreateTableHeaderRow(ReportResources.HeaderDriverName, ReportResources.HeaderDeviceModelName, ReportResources.UserAssignedName, ReportResources.DeviceLicensesRequired));
		foreach (ILicenseCalculationResult item in list)
		{
			if (item.ChannelLicenses > 1)
			{
				stringBuilder.Append(BaseHtmlLicenseReport.CreateTableRow(item.DriverName, item.ModelName, item.HardwareName, item.ChannelLicenses.ToString(CultureInfo.CurrentCulture)));
			}
			else
			{
				num += item.ChannelLicenses;
			}
		}
		stringBuilder.Append("</table>\n");
		if (num > 0)
		{
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<h6>{0}</h6>\n", string.Format(CultureInfo.CurrentCulture, ReportResources.SingleLicensableDeviceCount, num)));
		}
		stringBuilder.Append("</body>\n");
		stringBuilder.Append("</html>\n");
		return stringBuilder.ToString();
	}
}
