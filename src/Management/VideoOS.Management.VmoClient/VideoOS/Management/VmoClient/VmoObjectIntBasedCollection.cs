using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Management.VmoClient;

public abstract class VmoObjectIntBasedCollection<T> : VmoCollection<T>, IEnumerable<T>, IEnumerable where T : VmoObjectIntBased
{
	public T this[int index] => GetObjectByIndex(index.ToString(CultureInfo.InvariantCulture));

	public VmoObjectIntBasedCollection(Func<bool, T[]> loadData, VmoObject parent, bool isInitialized)
		: base(loadData, parent, isInitialized)
	{
	}

	public bool Contains(int id)
	{
		return this[id] != null;
	}
}
