using System.Collections.Generic;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.MatchLists;

public class LicensePlateStyleAliasComparer : IEqualityComparer<LicensePlateStyleAlias>
{
	public bool Equals(LicensePlateStyleAlias x, LicensePlateStyleAlias y)
	{
		if (x != y)
		{
			if (x != null && y != null && x.GetType() == y.GetType() && x.PlateStyleAliasId == y.PlateStyleAliasId)
			{
				return x.Alias == y.Alias;
			}
			return false;
		}
		return true;
	}

	public int GetHashCode(LicensePlateStyleAlias obj)
	{
		return (obj.PlateStyleAliasId * 397) ^ ((obj.Alias != null) ? obj.Alias.GetHashCode() : 0);
	}
}
