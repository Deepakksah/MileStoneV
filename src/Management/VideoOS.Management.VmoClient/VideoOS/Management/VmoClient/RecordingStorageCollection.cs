using System;

namespace VideoOS.Management.VmoClient;

public class RecordingStorageCollection : VmoObjectGuidBasedCollection<RecordingStorage>
{
	public RecordingStorageCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, RecordingStorage[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
