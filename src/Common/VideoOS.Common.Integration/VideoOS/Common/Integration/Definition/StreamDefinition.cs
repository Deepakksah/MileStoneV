using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class StreamDefinition
{
	private DeviceDefinition deviceDefinition;

	private streamType stream;

	private CapabilitiesCollection capabilities;

	public MainDefinition MainDefinition => deviceDefinition.MainDefinition;

	public DeviceDefinition DeviceDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return deviceDefinition;
		}
	}

	public streamType Base
	{
		[DebuggerStepThrough]
		get
		{
			return stream;
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return stream.name;
		}
	}

	public string DisplayNameResolved => Base.displayname.Resolve();

	public StreamHwid Hwid
	{
		get
		{
			return new StreamHwid(stream.capabilities.hwid);
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			stream.capabilities.hwid = value.Text;
		}
	}

	public Guid ReferenceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(stream.referenceid);
		}
	}

	public Guid StreamType
	{
		get
		{
			if (stream.streamtype != null && stream.streamtype != string.Empty)
			{
				return new Guid(stream.streamtype);
			}
			return Guid.Empty;
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
				capabilities = new CapabilitiesCollection(MainDefinition, stream.capabilities);
				return capabilities;
			}
		}
	}

	public StreamDefinition(DeviceDefinition deviceDefinition, streamType streamType)
	{
		this.deviceDefinition = deviceDefinition;
		stream = streamType;
	}
}
