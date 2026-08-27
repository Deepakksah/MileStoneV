using System;

namespace VideoOS.Management.VmoClient;

public class ArchiveStorageCollection : VmoObjectGuidBasedCollection<ArchiveStorage>
{
	public ArchiveStorageCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, ArchiveStorage[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
