using System;

namespace VideoOS.Management.VmoClient;

public class PatrollingProfileCollection : VmoObjectGuidBasedCollection<PatrollingProfile>
{
	public PatrollingProfileCollection(LoadMethod loadPatrollingProfiles, VmoObject parent, bool isInitialized)
		: base((Func<bool, PatrollingProfile[]>)loadPatrollingProfiles.Invoke, parent, isInitialized)
	{
	}
}
