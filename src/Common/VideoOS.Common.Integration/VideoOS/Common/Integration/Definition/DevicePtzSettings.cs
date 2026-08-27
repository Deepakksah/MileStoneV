using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class DevicePtzSettings
{
	private MainDefinition mainDefinition;

	private deviceptzsettingsType ptzSettings;

	private CapabilitiesCollection capabilities;

	public MainDefinition MainDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return mainDefinition;
		}
	}

	public deviceptzsettingsType Base
	{
		[DebuggerStepThrough]
		get
		{
			return ptzSettings;
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
				capabilities = new CapabilitiesCollection(mainDefinition, ptzSettings.capabilities);
				return capabilities;
			}
		}
	}

	public DevicePtzHwid Hwid
	{
		get
		{
			return new DevicePtzHwid(ptzSettings.capabilities.hwid);
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ptzSettings.capabilities.hwid = value.Text;
		}
	}

	public DevicePtzSettings(MainDefinition mainDefinition, deviceptzsettingsType ptzSettings)
	{
		this.mainDefinition = mainDefinition;
		this.ptzSettings = ptzSettings;
	}
}
