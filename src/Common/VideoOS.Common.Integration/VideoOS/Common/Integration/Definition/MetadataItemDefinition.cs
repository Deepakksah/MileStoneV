using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class MetadataItemDefinition
{
	private DeviceDefinition deviceDefinition;

	private metadataitemType MetadataItem;

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

	public metadataitemType Base
	{
		[DebuggerStepThrough]
		get
		{
			return MetadataItem;
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return MetadataItem.name;
		}
	}

	public string DisplayNameResolved => Base.displayname.Resolve();

	public MetadataItemHwid Hwid
	{
		get
		{
			return new MetadataItemHwid(MetadataItem.capabilities.hwid);
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			MetadataItem.capabilities.hwid = value.Text;
		}
	}

	public Guid ReferenceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(MetadataItem.referenceid);
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
				capabilities = new CapabilitiesCollection(MainDefinition, MetadataItem.capabilities);
				return capabilities;
			}
		}
	}

	public MetadataItemDefinition(DeviceDefinition deviceDefinition, metadataitemType MetadataItemType)
	{
		this.deviceDefinition = deviceDefinition;
		MetadataItem = MetadataItemType;
	}
}
