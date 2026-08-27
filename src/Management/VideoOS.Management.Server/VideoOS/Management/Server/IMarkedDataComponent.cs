using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IMarkedDataComponent : IDisposable
{
	Guid[] CreateMarkedData(Guid id, string[] deviceIds, string reference, string userName, string header, string description, MarkedDataDefinitions.MarkedDataTypes markedDataType, DateTime startTime, DateTime tagTime, DateTime endTime, OptionType retentionType, int retentionUnits, DateTime retentionExpire, long retentionSize, Guid sessionId);

	Guid[] UpdateMarkedData(Guid markedDataId, string[] deviceIds, string reference, string header, string description, DateTime startTime, DateTime tagTime, DateTime endTime, OptionType retentionType, int retentionUnits, DateTime retentionExpire, long retentionSize, IEnumerable<StickerSize2> stickerSizes, Guid sessionId);

	void UpdateMarkedDataSizes(Guid markedDataId, IEnumerable<StickerSize2> stickerSizes);

	Guid[] DeleteMarkedData(Guid markedDataId, Guid sessionId);

	void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus);

	MarkedDataDataContract[] QueryBookmarksSearchDeviceFilter(BookmarkSearchParametersDeviceFilter searchParameters);

	MarkedDataDataContract[] QueryBookmarksSearchNoDeviceFilter(string[] roleIds, BookmarkSearchParametersNoDeviceFilter searchParameters);

	MarkedDataDataContract QueryMarkedDataItemNoSecurity(string markedDataId);

	MarkedDataDataContract[] QueryMarkedDataSearch(string[] roleIds, MarkedDataSearchParams searchParams);

	MarkedDataDataContract[] QueryExpiredMarkedData();

	int NextSequenceCounterValue(string counterName);

	MarkedDataDataContract[] QueryRecordingServerMarkedData(string recordingServerId);

	bool RecorderHasLockedDeviceDataDistributions(string recorderId);

	string[] QueryOldSchemeRecordingServers();
}
