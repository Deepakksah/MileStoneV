using System;

namespace VideoOS.Management.VmoClient;

public class RecordingServerCollection : VmoObjectGuidBasedCollection<RecordingServer>
{
	public RecordingServerCollection(LoadMethod loadRecorders, VmoObject parent, bool isInitialized)
		: base((Func<bool, RecordingServer[]>)loadRecorders.Invoke, parent, isInitialized)
	{
	}
}
