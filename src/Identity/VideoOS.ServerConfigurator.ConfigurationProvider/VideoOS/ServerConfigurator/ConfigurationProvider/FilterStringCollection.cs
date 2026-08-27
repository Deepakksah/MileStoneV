using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[CollectionDataContract(Namespace = "")]
public class FilterStringCollection : List<string>
{
	public FilterStringCollection()
	{
	}

	public FilterStringCollection(IEnumerable<string> collection)
		: base(collection ?? Enumerable.Empty<string>())
	{
	}
}
