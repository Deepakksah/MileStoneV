using System.Collections.Generic;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class LicenseReportInformation
{
	private List<ILicenseCalculationResult> _licenseCalculationResults;

	public bool MissingLicenses { get; set; }

	public bool MultiLicenseHardware { get; set; }

	public List<ILicenseCalculationResult> LicenseCalculationResults
	{
		get
		{
			return _licenseCalculationResults ?? (_licenseCalculationResults = new List<ILicenseCalculationResult>());
		}
		set
		{
			_licenseCalculationResults = value;
		}
	}
}
