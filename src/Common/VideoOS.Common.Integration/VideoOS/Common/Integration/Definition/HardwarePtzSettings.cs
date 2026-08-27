using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class HardwarePtzSettings
{
	private MainDefinition mainDefinition;

	private hardwareptzsettingsType ptzSettings;

	private CapabilitiesCollection capabilities;

	public MainDefinition MainDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return mainDefinition;
		}
	}

	public hardwareptzsettingsType Base
	{
		[DebuggerStepThrough]
		get
		{
			return ptzSettings;
		}
	}

	public bool Enabled
	{
		[DebuggerStepThrough]
		get
		{
			return ptzSettings.enabled;
		}
		[DebuggerStepThrough]
		set
		{
			ptzSettings.enabled = value;
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

	public HardwarePtzHwid Hwid
	{
		get
		{
			return new HardwarePtzHwid(ptzSettings.capabilities.hwid);
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

	public HardwarePtzSettings(MainDefinition mainDefinition, hardwareptzsettingsType ptzSettings)
	{
		this.mainDefinition = mainDefinition;
		this.ptzSettings = ptzSettings;
	}
}
