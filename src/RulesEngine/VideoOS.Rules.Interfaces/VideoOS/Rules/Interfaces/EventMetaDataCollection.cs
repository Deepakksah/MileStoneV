using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Rules.Interfaces;

public sealed class EventMetaDataCollection : Dictionary<string, EventMetaData>
{
	public EventMetaDataCollection()
	{
	}

	public EventMetaDataCollection(EventMetaDataCollection source)
		: base((IDictionary<string, EventMetaData>)source)
	{
	}

	public EventMetaDataCollection(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}

	public void SetMetadata(string name, string metaType, string dataType, string value)
	{
		base[name] = new EventMetaData(name, metaType, dataType, value);
	}

	public void SetMetadata(string name, EventMetaData.MetaTypeEnum metaType, EventMetaData.DataTypeEnum dataType, string value)
	{
		base[name] = new EventMetaData(name, metaType, dataType, value);
	}

	public EventMetaData SetMetadataAsBool(string name, bool value)
	{
		base[name] = new EventMetaData(name, EventMetaData.MetaTypeEnum.Data, EventMetaData.DataTypeEnum.Bool, Convert.ToString(value, CultureInfo.InvariantCulture));
		return base[name];
	}

	public EventMetaData SetMetadataAsInt(string name, int value)
	{
		base[name] = new EventMetaData(name, EventMetaData.MetaTypeEnum.Data, EventMetaData.DataTypeEnum.Int, Convert.ToString(value, CultureInfo.InvariantCulture));
		return base[name];
	}

	public EventMetaData SetMetadataAsDouble(string name, double value)
	{
		base[name] = new EventMetaData(name, EventMetaData.MetaTypeEnum.Data, EventMetaData.DataTypeEnum.Double, Convert.ToString(value, CultureInfo.InvariantCulture));
		return base[name];
	}

	public EventMetaData SetMetadataAsString(string name, string value)
	{
		base[name] = new EventMetaData(name, EventMetaData.MetaTypeEnum.Data, EventMetaData.DataTypeEnum.String, value);
		return base[name];
	}

	public EventMetaData SetMetadataAsGuid(string name, Guid value)
	{
		return SetMetadataAsGuid(name, value, EventMetaData.MetaTypeEnum.Data);
	}

	public EventMetaData SetMetadataAsGuid(string name, Guid value, EventMetaData.MetaTypeEnum metadataType)
	{
		base[name] = new EventMetaData(name, metadataType, EventMetaData.DataTypeEnum.Guid, value.ToString());
		return base[name];
	}
}
