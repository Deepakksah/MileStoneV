using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Management.VmoClient;

public abstract class VmoObjectGuidBasedCollection<T> : VmoCollection<T>, IEnumerable<T>, IEnumerable where T : VmoObjectGuidBased
{
	public T this[Guid index] => GetObjectByIndex(index.ToString());

	public VmoObjectGuidBasedCollection(Func<bool, T[]> loadData, VmoObject parent, bool isInitialized)
		: base(loadData, parent, isInitialized)
	{
	}

	public bool Contains(Guid id)
	{
		return this[id] != null;
	}
}
