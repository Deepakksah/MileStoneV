using System.Collections.Generic;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class MetadataSource : DataSource
{
	public const string DisplayNameKey = "DisplayName";

	public const string DisplayIdKey = "DisplayId";

	public const string ValidTimeKey = "ValidTime";

	public MetadataSource(Item item)
		: base(item)
	{
		if (item.FQID.Kind != Kind.Metadata)
		{
			throw new ImplementationMIPException("Only Metadata items are supported by MetadataSource");
		}
	}

	public override List<DataType> GetTypes()
	{
		return InternalConfigService.Instance.GetMetadataTypes(Item.FQID);
	}
}
