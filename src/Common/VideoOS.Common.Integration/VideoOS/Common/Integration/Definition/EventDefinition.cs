using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class EventDefinition
{
	private MainDefinition mainDefinition;

	private eventType @event;

	private CapabilitiesCollection capabilities;

	private MetaDataCollection metaData;

	public MainDefinition MainDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return mainDefinition;
		}
	}

	public eventType Base
	{
		[DebuggerStepThrough]
		get
		{
			return @event;
		}
	}

	public CapabilitiesCollection Capabilities
	{
		get
		{
			if (capabilities != null)
			{
				return capabilities;
			}
			lock (this)
			{
				if (capabilities != null)
				{
					return capabilities;
				}
				capabilities = new CapabilitiesCollection(mainDefinition, @event.capabilities);
				return capabilities;
			}
		}
	}

	public MetaDataCollection MetaData
	{
		get
		{
			if (metaData != null)
			{
				return metaData;
			}
			lock (this)
			{
				if (metaData != null)
				{
					return metaData;
				}
				metaData = new MetaDataCollection(mainDefinition, @event.metadata);
				return metaData;
			}
		}
	}

	public bool IsDynamic
	{
		[DebuggerStepThrough]
		get
		{
			return string.Equals("True", @event.dynamic, StringComparison.OrdinalIgnoreCase);
		}
	}

	public Guid ReferenceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(@event.referenceid);
		}
		set
		{
			@event.referenceid = value.ToString();
		}
	}

	public bool HasCounterEvent
	{
		[DebuggerStepThrough]
		get
		{
			return @event.countereventreferenceid != null;
		}
	}

	public Guid CounterEventReferenceId
	{
		get
		{
			if (@event.countereventreferenceid == null)
			{
				return new Guid(@event.referenceid);
			}
			return new Guid(@event.countereventreferenceid);
		}
		set
		{
			@event.countereventreferenceid = value.ToString();
		}
	}

	public EventHwid Hwid
	{
		get
		{
			return new EventHwid(@event.capabilities.hwid);
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			@event.capabilities.hwid = value.Text;
		}
	}

	public string DisplayNameResolved => DefinitionUtility.ResolveDisplayName(@event.displayname);

	public Guid DisplayNameReferenceId
	{
		get
		{
			return new Guid(@event.displayname.referenceid);
		}
		set
		{
			@event.displayname.referenceid = value.ToString();
		}
	}

	public EventDefinition(MainDefinition mainDefinition, eventType @event)
	{
		this.mainDefinition = mainDefinition;
		this.@event = @event;
	}
}
