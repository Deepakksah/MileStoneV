using System.Collections.Generic;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class StreamDataSource : DataSource
{
	public StreamDataSource(Item item)
		: base(item)
	{
		if (item == null)
		{
			throw new ArgumentNullMIPException("item");
		}
		if (item.FQID.Kind != Kind.Camera)
		{
			throw new ImplementationMIPException("Only Camera Items are supported by StreamDataSource");
		}
		new TrackPageViewData(typeof(StreamDataSource)).Send();
	}

	public override List<DataType> GetTypes()
	{
		StreamType[] streamTypes = InternalConfigService.Instance.GetStreamTypes(Item.FQID);
		List<DataType> list = new List<DataType>();
		StreamType[] array = streamTypes;
		foreach (StreamType streamType in array)
		{
			list.Add(new DataType
			{
				Id = streamType.Id,
				Name = streamType.Name,
				Properties = streamType.Properties
			});
		}
		return list;
	}
}
