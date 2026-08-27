using System;
using System.Collections.ObjectModel;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class MetaDataCollection : KeyedCollection<string, MetaDataDefinition>
{
	private MainDefinition mainDefinition;

	private metadataentryType[] metaData;

	public MetaDataCollection(MainDefinition mainDefinition, metadataentryType[] metaData)
	{
		this.mainDefinition = mainDefinition;
		this.metaData = metaData;
		if (this.metaData != null)
		{
			metadataentryType[] array = this.metaData;
			foreach (metadataentryType entry in array)
			{
				Add(new MetaDataDefinition(this.mainDefinition, entry));
			}
		}
	}

	protected override string GetKeyForItem(MetaDataDefinition item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.Name;
	}
}
