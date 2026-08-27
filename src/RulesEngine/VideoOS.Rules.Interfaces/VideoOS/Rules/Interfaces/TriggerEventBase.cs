using System;
using System.Diagnostics;

namespace VideoOS.Rules.Interfaces;

public abstract class TriggerEventBase
{
	private DateTime timeUtc;

	private Guid eventTypeId;

	private Guid sourceId;

	private bool isRemoteEvent;

	public DateTime Time
	{
		[DebuggerStepThrough]
		get
		{
			return timeUtc;
		}
		[DebuggerStepThrough]
		set
		{
			timeUtc = value;
		}
	}

	public Guid EventTypeId
	{
		[DebuggerStepThrough]
		get
		{
			return eventTypeId;
		}
		[DebuggerStepThrough]
		set
		{
			eventTypeId = value;
		}
	}

	public Guid SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return sourceId;
		}
		[DebuggerStepThrough]
		set
		{
			sourceId = value;
		}
	}

	public bool IsRemoteEvent
	{
		[DebuggerStepThrough]
		get
		{
			return isRemoteEvent;
		}
		[DebuggerStepThrough]
		set
		{
			isRemoteEvent = value;
		}
	}

	protected TriggerEventBase(Guid sourceId, Guid eventTypeId, DateTime eventTimeUtc)
	{
		timeUtc = eventTimeUtc.ToUniversalTime();
		this.eventTypeId = eventTypeId;
		this.sourceId = sourceId;
		isRemoteEvent = false;
	}

	protected TriggerEventBase(Guid sourceId, Guid eventTypeId)
	{
		timeUtc = DateTime.UtcNow;
		this.eventTypeId = eventTypeId;
		this.sourceId = sourceId;
		isRemoteEvent = false;
	}
}
