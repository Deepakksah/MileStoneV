using System;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Proxy.EvidenceLockApi;

public interface IEvidenceLockApiClient
{
	void Initialize(LoginSettings loginSettings, bool debugLog);

	void Close();

	EvidenceLockDataModel EvidenceLockGet(Guid evidenceLockId);

	string EvidenceLockGetNewReference();

	EvidenceLockDataModel EvidenceLockCreate(Guid evidenceLockId, Guid[] deviceIds, DateTime startTime, DateTime tagTime, DateTime endTime, string reference, string header, string description, DateTime retentionExpire, int retentionUnits = 0, string retentionType = null);

	void EvidenceLockDelete(Guid evidenceLockId);

	void EvidenceLockUpdate(Guid evidenceLockId, Guid[] deviceIds, DateTime? startTime, DateTime? tagTime, DateTime? endTime, string reference, string header, string description, int retentionUnits, string retentionType, DateTime? retentionExpire);

	EvidenceLockDataModel[] EvidenceLockSearch(Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerContains, string descriptionContains, string[] users, DateTime? createdFromTime, DateTime? createdToTime, DateTime? markedDataFromTime, DateTime? markedDataToTime, DateTime? tagTimeFromTime, DateTime? tagTimeToTime, DateTime? retentionExpireFrom, DateTime? retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending);
}
