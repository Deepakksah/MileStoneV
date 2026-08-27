using System;

namespace VideoOS.Management.VmoClient;

public class MetadataObjectDefinitionCollection : VmoObjectGuidBasedCollection<MetadataObjectDefinition>
{
	public MetadataObjectDefinitionCollection(LoadMethod loadObjectDefinitions, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataObjectDefinition[]>)loadObjectDefinitions.Invoke, parent, isInitialized)
	{
	}
}
