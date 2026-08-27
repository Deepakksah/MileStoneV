using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseReportGenerator
{
	byte[] GenerateReport(IEnumerable<ILicenseCalculationResult> calculationResults, ILicenseDocument licenseDocument, Dictionary<string, string> reportProperties);
}
