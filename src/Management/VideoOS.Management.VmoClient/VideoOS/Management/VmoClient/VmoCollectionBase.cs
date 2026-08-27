using System;

namespace VideoOS.Management.VmoClient;

public abstract class VmoCollectionBase
{
	public abstract Type ContentType { get; }

	public abstract void AddVmoObject(VmoObject vmoObject);

	public abstract void RemoveVmoObject(VmoObject vmoObject);
}
