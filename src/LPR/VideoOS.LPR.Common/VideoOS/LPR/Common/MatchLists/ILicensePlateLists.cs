using System;
using System.Threading.Tasks;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.MatchLists;

public interface ILicensePlateLists
{
	Task<bool> AddMatchListAsync(Guid id, string name);

	Task<bool> DeleteMatchListAsync(Guid id);

	Task<bool> ModifyMatchListAsync(Guid id, string name, LicensePlatePropertyDefinition[] propertyDefinitions, FQID[] triggerEvents);

	Task<LicensePlateList[]> GetMatchListsAsync();
}
