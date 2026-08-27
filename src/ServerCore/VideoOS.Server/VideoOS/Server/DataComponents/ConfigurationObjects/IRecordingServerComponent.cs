using System;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public interface IRecordingServerComponent : IDisposable
{
	Tuple<bool, bool> CreateOrUpdateRecorder(Guid recorderId, Guid serviceId, RecordingServerServiceType serviceType, string name, string hostName, Uri webServerUri, string scanRanges, string timeZoneName, string devicePackInfo, ushort failoverServerPort, int databasePort, Guid failoverId, Version version, bool isMulticastLicensed);

	RecorderServiceVersion[] QueryRecorderServiceVersions();

	RecordingServer QueryRecordingServerActive(Guid recorderId);
}
