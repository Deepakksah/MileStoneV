using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IAggregateFeedDictionary : IDictionary<string, ILicenseFeed>, ICollection<KeyValuePair<string, ILicenseFeed>>, IEnumerable<KeyValuePair<string, ILicenseFeed>>, IEnumerable
{
	int[] EnabledCameraIndexes { get; }
}
