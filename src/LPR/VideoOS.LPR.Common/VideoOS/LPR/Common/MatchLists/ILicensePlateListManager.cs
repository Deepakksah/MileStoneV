using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.LPR.Common.MatchLists.Telemetry;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.MatchLists;

public interface ILicensePlateListManager
{
	Task<LicensePlateInfo[]> GetLicensePlatesAsync(Guid listId, int startIndex, int count, string searchFilter, bool showErrorDialogOnFail);

	Task<int> AddLicensePlatesAsync(Guid listId, IEnumerable<LicensePlateInfo> newPlates, bool showErrorDialogOnFail);

	Task<int> DeleteLicensePlatesAsync(Guid listId, IEnumerable<LicensePlate> licensePlates, bool showErrorDialogOnFail);

	Task<bool> ClearListContentsAsync(Guid listId);

	Task<int> GetLicensePlateCountAsync(Guid listId, string searchFilter);

	IMatchListTelemetryClient GetMatchListTelemetryClient();
}
