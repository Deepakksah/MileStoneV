using System;
using VideoOS.Common.Integration.Communication.Internal;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class EventMetaDataDto
{
	public string Name;

	public MetaTypeDto MetaType;

	public MetaDataTypeDto DataType;

	public string Value;

	public static EventMetaDataDto Create(EventMetaData eventMetaData)
	{
		if (eventMetaData == null)
		{
			throw new ArgumentNullException("eventMetaData");
		}
		return new EventMetaDataDto
		{
			Name = eventMetaData.Name,
			MetaType = DtoConvert.ToMetaTypeDto(eventMetaData.MetaType),
			DataType = DtoConvert.ToMetaDataTypeDto(eventMetaData.DataType),
			Value = eventMetaData.Value
		};
	}

	public static EventMetaData CreateEventMetaData(EventMetaDataDto eventMetaDataDto)
	{
		if (eventMetaDataDto == null)
		{
			throw new ArgumentNullException("eventMetaDataDto");
		}
		EventMetaData.MetaTypeEnum metaType = DtoConvert.ToMetaTypeEnum(eventMetaDataDto.MetaType);
		EventMetaData.DataTypeEnum dataType = DtoConvert.ToDataTypeEnum(eventMetaDataDto.DataType);
		return new EventMetaData(eventMetaDataDto.Name, metaType, dataType, eventMetaDataDto.Value);
	}
}
