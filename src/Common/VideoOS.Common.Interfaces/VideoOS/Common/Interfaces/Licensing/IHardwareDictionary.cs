using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IHardwareDictionary : IDictionary<string, List<IHardwareInfo>>, ICollection<KeyValuePair<string, List<IHardwareInfo>>>, IEnumerable<KeyValuePair<string, List<IHardwareInfo>>>, IEnumerable
{
	int InterconnectedCount { get; }

	int NonInterconnectedCount { get; }

	List<IHardwareInfo> InterconnectHardware { get; }

	List<IHardwareInfo> NonInterconnectHardware { get; }

	void AddHardware(IHardwareInfo hardware);

	int GetLicenseCount(string macAddress);

	int GetChannelLicenseCount(string macAddress);

	int[] EnabledCameraIndexes(string macAddress);

	int NumberOfUniqueEnabledCameras(string macAddress);

	void UpdateHardwareAggregates();

	IAggregateFeedDictionary GetAggregateFeeds(string macAddress);

	void UpdateTotalFeedCountsAcrossAllHardware(IFeedDictionary allHardwareFeedDictionary);

	void UpdateTotalICPFeedCountsAcrossAllHardware(IFeedDictionary allHardwareFeedDictionary);
}
