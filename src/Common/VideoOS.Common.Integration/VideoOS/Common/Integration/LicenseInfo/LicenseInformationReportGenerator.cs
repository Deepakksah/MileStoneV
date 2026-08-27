using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using VideoOS.Common.Integration.Properties;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public class LicenseInformationReportGenerator : BaseHtmlLicenseReport, ILicenseReportGenerator
{
	private const string WarningReportStyles = "<style>\nbody {width:944px;margin:80px auto 160px auto;}\nh1 {font-family: \"Open Sans\",Sans-Serif;font-size: 32px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 0;}\nh2 {font-family: \"Open Sans\",Sans-Serif;font-size: 20px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 40px 0 0 0;max-width: 787px;}\nh3 {font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 32px 0 0 0;}\n.hint {font-family: \"Open Sans\",Sans-Serif;font-size: 11px;font-weight: normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #7f7f7f;margin: 8px 0 24px 0;}\n.headerText {font-family: \"Open Sans\",Sans-Serif;font-size: 24px;margin: 0 0 5px 0;}\ntable {border-collapse: collapse;border-spacing: 0;width: 100%;}\nth {text-align: left;padding: 16px;background-color: #023852;font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #ffffff;}\ntd {text-align: left;padding: 16px;font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight:normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;}\ntr {text-align: left;padding: 16px;background-color: #ffffff;font-family: \"Open Sans\",Sans-Serif;font-size: 12px;font-weight: normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;}\ntr:nth-child(even) {padding: 16px;background-color: #f3f3f3;}\n@media print and (color) {* {-webkit-print-color-adjust: exact;print-color-adjust: exact;}}\n</style>\n";

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
		return Encoding.UTF8.GetBytes(GenerateInformationReport(calculationResults, reportProperties));
	}

	private static string GenerateInformationReport(IEnumerable<ILicenseCalculationResult> calculationResults, Dictionary<string, string> reportProperties)
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
		List<ILicenseCalculationResult> list = calculationResults.OrderByDescending((ILicenseCalculationResult o) => o.ChannelLicenses).ToList();
		StringBuilder stringBuilder = new StringBuilder("<!DOCTYPE html>\n", 4096);
		stringBuilder.Append(BaseHtmlLicenseReport.CreateHtmlHeader(ReportResources.InformationTitle));
		stringBuilder.Append("<body>\n");
		stringBuilder.Append("<style>\nbody {width:944px;margin:80px auto 160px auto;}\nh1 {font-family: \"Open Sans\",Sans-Serif;font-size: 32px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 0;}\nh2 {font-family: \"Open Sans\",Sans-Serif;font-size: 20px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 40px 0 0 0;max-width: 787px;}\nh3 {font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;margin: 32px 0 0 0;}\n.hint {font-family: \"Open Sans\",Sans-Serif;font-size: 11px;font-weight: normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #7f7f7f;margin: 8px 0 24px 0;}\n.headerText {font-family: \"Open Sans\",Sans-Serif;font-size: 24px;margin: 0 0 5px 0;}\ntable {border-collapse: collapse;border-spacing: 0;width: 100%;}\nth {text-align: left;padding: 16px;background-color: #023852;font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight: 600;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #ffffff;}\ntd {text-align: left;padding: 16px;font-family: \"Open Sans\",Sans-Serif;font-size: 14px;font-weight:normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;}\ntr {text-align: left;padding: 16px;background-color: #ffffff;font-family: \"Open Sans\",Sans-Serif;font-size: 12px;font-weight: normal;font-stretch: normal;font-style: normal;line-height: normal;letter-spacing: normal;color: #000000;}\ntr:nth-child(even) {padding: 16px;background-color: #f3f3f3;}\n@media print and (color) {* {-webkit-print-color-adjust: exact;print-color-adjust: exact;}}\n</style>\n");
		stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<h1>{0}</h1>\n", ReportResources.HeaderInformationLicenses));
		if (!string.IsNullOrEmpty(text))
		{
			stringBuilder.Append(BaseHtmlLicenseReport.CreatePNode("headerText", string.Format(CultureInfo.CurrentCulture, ReportResources.SupportedDeviceListLink, text) + " " + ReportResources.ContactResellerAdditionalLicenses));
		}
		stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<h2>{0}</h2>\n", ReportResources.DeviceListHeader));
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
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<h3>{0}</h3>\n", string.Format(CultureInfo.CurrentCulture, ReportResources.SingleLicensableDeviceCount, num)));
		}
		stringBuilder.Append("</body>\n");
		stringBuilder.Append("</html>\n");
		return stringBuilder.ToString();
	}
}
