using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

[Serializable]
public sealed class PortDictionary : Dictionary<string, IPort>, IPortDictionary, IDictionary<string, IPort>, ICollection<KeyValuePair<string, IPort>>, IEnumerable<KeyValuePair<string, IPort>>, IEnumerable
{
	public PortDictionary()
	{
	}

	private PortDictionary(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public void IncrementCount(string feedType, int count)
	{
		if (feedType == null)
		{
			throw new ArgumentNullException("feedType");
		}
		if (!ContainsKey(feedType))
		{
			Add(feedType, new Port(feedType, count));
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
