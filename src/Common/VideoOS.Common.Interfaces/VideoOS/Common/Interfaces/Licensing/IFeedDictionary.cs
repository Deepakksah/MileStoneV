using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IFeedDictionary : IDictionary<string, ILicenseFeed>, ICollection<KeyValuePair<string, ILicenseFeed>>, IEnumerable<KeyValuePair<string, ILicenseFeed>>, IEnumerable
{
	void IncrementCount(string feedType, int count);

	void IncrementCount(string feedType);
}
