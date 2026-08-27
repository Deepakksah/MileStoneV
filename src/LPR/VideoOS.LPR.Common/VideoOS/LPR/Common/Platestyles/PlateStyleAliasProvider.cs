using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Utilities.Validation;

namespace VideoOS.LPR.Common.Platestyles;

public class PlateStyleAliasProvider : IPlateStyleAliasProvider
{
	private readonly List<LicensePlateStyle> _allPlateStyles;

	public PlateStyleAliasProvider(IEnumerable<LicensePlateStyle> allPlateStyles)
	{
		allPlateStyles.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\Platestyles\\PlateStyleAliasProvider.cs", 15);
		_allPlateStyles = allPlateStyles.ToList();
	}

	public string GetAliasOrStyle(string plateStyleCode)
	{
		plateStyleCode.NullCheck("", "GetAliasOrStyle", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\Platestyles\\PlateStyleAliasProvider.cs", 21);
		if (string.IsNullOrEmpty(plateStyleCode))
		{
			return "Unknown";
		}
		LicensePlateStyle licensePlateStyle = _allPlateStyles.FirstOrDefault((LicensePlateStyle s) => s.Code.Equals(plateStyleCode, StringComparison.Ordinal));
		if (licensePlateStyle == null)
		{
			return plateStyleCode;
		}
		return licensePlateStyle.Style;
	}

	public int GetAliasIdentifier(string plateStyleCode)
	{
		plateStyleCode.NullCheck("", "GetAliasIdentifier", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\Platestyles\\PlateStyleAliasProvider.cs", 31);
		LicensePlateStyle licensePlateStyle = _allPlateStyles.FirstOrDefault((LicensePlateStyle s) => s.Code.Equals(plateStyleCode, StringComparison.Ordinal));
		if (licensePlateStyle == null || licensePlateStyle.LicensePlateStyleAlias == null)
		{
			return 0;
		}
		return licensePlateStyle.LicensePlateStyleAlias.PlateStyleAliasId;
	}
}
