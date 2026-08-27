using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.Search;

public class SearchResultDataMetadataObjects
{
	public Item MetadataItem { get; }

	public ReadOnlyCollection<int> ObjectIds { get; }

	public DateTime From { get; }

	public DateTime To { get; }

	public SearchResultDataMetadataObjects(Item metadataItem, IEnumerable<int> objectIds, DateTime from, DateTime to)
	{
		if (metadataItem == null)
		{
			throw new ArgumentNullException("metadataItem");
		}
		if (objectIds == null)
		{
			throw new ArgumentNullException("objectIds");
		}
		if (metadataItem.FQID.Kind != Kind.Metadata)
		{
			throw new ArgumentException("Kind must be Metadata", "metadataItem");
		}
		MetadataItem = metadataItem;
		ObjectIds = objectIds.ToList().AsReadOnly();
		From = from;
		To = to;
	}
}
