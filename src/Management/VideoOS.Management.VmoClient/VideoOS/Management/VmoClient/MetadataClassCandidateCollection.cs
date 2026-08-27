using System;

namespace VideoOS.Management.VmoClient;

public class MetadataClassCandidateCollection : VmoObjectGuidBasedCollection<MetadataClassCandidate>
{
	public MetadataClassCandidateCollection(LoadMethod loadClassCandidates, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataClassCandidate[]>)loadClassCandidates.Invoke, parent, isInitialized)
	{
	}
}
