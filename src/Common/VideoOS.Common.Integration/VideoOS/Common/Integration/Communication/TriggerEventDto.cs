using System;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class TriggerEventDto
{
	public Guid EventTypeId;

	public DateTime TimeUtc;

	public Guid SourceId;

	public bool IsRemoteEvent;

	public EventMetaDataCollectionDto MetaData;

	public static TriggerEventDto Create(TriggerEvent triggerEvent)
	{
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		TriggerEventDto triggerEventDto = new TriggerEventDto();
		triggerEventDto.SourceId = triggerEvent.SourceId;
		triggerEventDto.EventTypeId = triggerEvent.EventTypeId;
		triggerEventDto.TimeUtc = triggerEvent.Time.ToUniversalTime();
		triggerEventDto.IsRemoteEvent = triggerEvent.IsRemoteEvent;
		triggerEventDto.MetaData = new EventMetaDataCollectionDto();
		if (triggerEvent.MetaData != null)
		{
			foreach (EventMetaData value in triggerEvent.MetaData.Values)
			{
				EventMetaDataDto item = EventMetaDataDto.Create(value);
				triggerEventDto.MetaData.Add(item);
			}
		}
		return triggerEventDto;
	}

	public static TriggerEvent CreateTriggerEvent(TriggerEventDto triggerEventDto)
	{
		if (triggerEventDto == null)
		{
			throw new ArgumentNullException("triggerEventDto");
		}
		TriggerEvent triggerEvent = new TriggerEvent(triggerEventDto.SourceId, triggerEventDto.EventTypeId, triggerEventDto.TimeUtc);
		triggerEvent.IsRemoteEvent = triggerEventDto.IsRemoteEvent;
		if (triggerEventDto.MetaData != null)
		{
			foreach (EventMetaDataDto item in triggerEventDto.MetaData)
			{
				EventMetaData eventMetaData = EventMetaDataDto.CreateEventMetaData(item);
				triggerEvent.MetaData.Add(eventMetaData.Name, eventMetaData);
			}
		}
		return triggerEvent;
	}
}
