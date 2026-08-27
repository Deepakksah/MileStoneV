using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IPortDictionary : IDictionary<string, IPort>, ICollection<KeyValuePair<string, IPort>>, IEnumerable<KeyValuePair<string, IPort>>, IEnumerable
{
	void IncrementCount(string feedType, int count);

	void IncrementCount(string feedType);
}
