using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

[Serializable]
public sealed class FeedDictionary : Dictionary<string, ILicenseFeed>, IFeedDictionary, IDictionary<string, ILicenseFeed>, ICollection<KeyValuePair<string, ILicenseFeed>>, IEnumerable<KeyValuePair<string, ILicenseFeed>>, IEnumerable
{
	public FeedDictionary()
	{
	}

	private FeedDictionary(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public void IncrementCount(string feedType, int count)
	{
		if (!ContainsKey(feedType))
		{
			Add(feedType, new Feed(feedType, count));
		}
		else
		{
			base[feedType].Count += count;
		}
	}

	public void IncrementCount(string feedType)
	{
		IncrementCount(feedType, 1);
	}
}
