using System;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Drivers;

public class DriverEventArgs : EventArgs
{
	public Guid SourceId { get; private set; }

	public Guid Type { get; set; }

	public object Data { get; private set; }

	public EventMetaDataCollection MetaData { get; private set; }

	public bool IsDelegatedEvent { get; private set; }

	public DriverEventArgs(Guid sourceId, Guid type)
		: this(sourceId, type, delegateEvent: false, null, null)
	{
	}

	public DriverEventArgs(Guid sourceId, Guid type, bool delegateEvent)
		: this(sourceId, type, delegateEvent, null, null)
	{
	}

	public DriverEventArgs(Guid sourceId, Guid type, bool delegateEvent, object data)
		: this(sourceId, type, delegateEvent, data, null)
	{
	}

	public DriverEventArgs(Guid sourceId, Guid type, bool delegateEvent, object data, EventMetaDataCollection metadata)
	{
		SourceId = sourceId;
		Type = type;
		Data = data;
		if (metadata != null)
		{
			MetaData = metadata;
		}
		else
		{
			MetaData = new EventMetaDataCollection();
		}
		IsDelegatedEvent = delegateEvent;
	}
}
