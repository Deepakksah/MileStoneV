using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.MatchLists;

public class MatchListItemViewModel
{
	public string DisplayValue { get; set; }

	public string Value { get; set; }

	public LicensePlateProperty[] Properties { get; set; }

	public MatchListItemType MatchListItemType { get; set; }

	public MatchListItemViewModel()
	{
		Value = string.Empty;
		DisplayValue = string.Empty;
		MatchListItemType = MatchListItemType.LicensePlate;
	}
}
