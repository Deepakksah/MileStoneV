using System;

namespace VideoOS.Management.VmoClient;

public sealed class RetentionOptionRoleRelationCollection : VmoObjectGuidBasedCollection<RetentionOptionRoleRelation>
{
	public RetentionOptionRoleRelationCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, RetentionOptionRoleRelation[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
