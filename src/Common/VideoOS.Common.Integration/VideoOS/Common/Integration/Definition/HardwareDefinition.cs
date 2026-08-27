using System;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class HardwareDefinition
{
	private MainDefinition mainDefinition;

	private hardwareType hardware;

	private HardwarePtzSettings ptzSettings;

	private DeviceDefinitionCollection deviceDefinitions;

	private CapabilitiesCollection capabilities;

	private EventDefinitionCollection events;

	private SettingsCollection settings;

	public MainDefinition MainDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return mainDefinition;
		}
	}

	public hardwareType Base
	{
		[DebuggerStepThrough]
		get
		{
			return hardware;
		}
	}

	public HardwareHwid Hwid
	{
		[DebuggerStepThrough]
		get
		{
			return new HardwareHwid(hardware.capabilities.hwid);
		}
	}

	public HardwarePtzSettings PtzSettings
	{
		get
		{
			if (hardware.ptzsettings == null)
			{
				return null;
			}
			if (ptzSettings != null)
			{
				return ptzSettings;
			}
			lock (this)
			{
				if (ptzSettings != null)
				{
					return ptzSettings;
				}
				ptzSettings = new HardwarePtzSettings(mainDefinition, hardware.ptzsettings);
				return ptzSettings;
			}
		}
	}

	public int MaxEvents => int.Parse(Base.events.maxcount, CultureInfo.InvariantCulture);

	public EventDefinitionCollection Events
	{
		get
		{
			if (events != null)
			{
				return events;
			}
			lock (this)
			{
				if (events != null)
				{
					return events;
				}
				EventDefinitionCollection eventDefinitionCollection = new EventDefinitionCollection();
				if (hardware.supportedevents != null)
				{
					supportedeventType[] supportedevents = hardware.supportedevents;
					foreach (supportedeventType supportedeventType2 in supportedevents)
					{
						eventType eventType2 = mainDefinition.HardwareDefinition.Base.events.FindByReferenceId(supportedeventType2.eventreferenceid);
						if (eventType2 != null)
						{
							eventDefinitionCollection.Add(new EventDefinition(mainDefinition, eventType2));
						}
					}
				}
				events = eventDefinitionCollection;
				return events;
			}
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
				capabilities = new CapabilitiesCollection(mainDefinition, hardware.capabilities);
				return capabilities;
			}
		}
	}

	public SettingsCollection Settings
	{
		get
		{
			if (settings != null)
			{
				return settings;
			}
			lock (this)
			{
				if (settings != null)
				{
					return settings;
				}
				if (mainDefinition.Base.properties != null)
				{
					settingsType[] properties = mainDefinition.Base.properties;
					foreach (settingsType settingsType2 in properties)
					{
						if (settingsType2.hwid == "hardware")
						{
							settings = new SettingsCollection(mainDefinition, settingsType2);
							break;
						}
					}
				}
				if (settings == null)
				{
					settings = new SettingsCollection(mainDefinition, null);
				}
				return settings;
			}
		}
	}

	public DeviceDefinitionCollection DeviceDefinitions
	{
		get
		{
			if (deviceDefinitions != null)
			{
				return deviceDefinitions;
			}
			lock (this)
			{
				if (deviceDefinitions != null)
				{
					return deviceDefinitions;
				}
				DeviceDefinitionCollection deviceDefinitionCollection = new DeviceDefinitionCollection();
				if (Base.devices != null)
				{
					deviceType[] devices = Base.devices;
					foreach (deviceType device in devices)
					{
						deviceDefinitionCollection.Add(new DeviceDefinition(mainDefinition, device));
					}
				}
				deviceDefinitions = deviceDefinitionCollection;
				return deviceDefinitions;
			}
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return mainDefinition.Base.driver.name;
		}
	}

	public string Version
	{
		[DebuggerStepThrough]
		get
		{
			return mainDefinition.Base.driver.version;
		}
	}

	public string VersionId
	{
		[DebuggerStepThrough]
		get
		{
			return mainDefinition.Base.driver.versionid;
		}
	}

	public string ProductId => mainDefinition.Base.FindHardwareSettingByTypeReferenceId(TypeReferenceIds.ProductId)?.value;

	public bool SupportEvents
	{
		get
		{
			if (Events.Count > 0)
			{
				return true;
			}
			foreach (DeviceDefinition deviceDefinition in DeviceDefinitions)
			{
				if (deviceDefinition.Events.Count > 0)
				{
					return true;
				}
			}
			return false;
		}
	}

	public HardwareDefinition(MainDefinition mainDefinition)
	{
		if (mainDefinition == null)
		{
			throw new ArgumentNullException("mainDefinition");
		}
		this.mainDefinition = mainDefinition;
		hardware = mainDefinition.Base.hardware;
	}

	public bool IsPtzEnabledVideoServer()
	{
		if (PtzSettings != null)
		{
			return PtzSettings.Enabled;
		}
		return false;
	}

	public bool MaxEventsSpecified()
	{
		if (Base.events != null)
		{
			return Base.events.maxcount != null;
		}
		return false;
	}

	public DeviceDefinition GetDeviceDefinition(DeviceHwid hwid)
	{
		if (DeviceDefinitions.Contains(hwid))
		{
			return DeviceDefinitions[hwid];
		}
		return null;
	}

	public CapabilitiesCollection GetCapabilities(Hwid hwid)
	{
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		string text = hwid.Text;
		if (string.Equals(Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
		{
			return Capabilities;
		}
		if (PtzSettings != null && string.Equals(PtzSettings.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
		{
			return PtzSettings.Capabilities;
		}
		foreach (EventDefinition @event in Events)
		{
			if (string.Equals(@event.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
			{
				return @event.Capabilities;
			}
		}
		return null;
	}

	public string ProductIdDisplayNameResolved()
	{
		enumValueType[] supportedProductIds = mainDefinition.Base.GetSupportedProductIds();
		foreach (enumValueType enumValueType2 in supportedProductIds)
		{
			if (string.Equals(ProductId, enumValueType2.name, StringComparison.Ordinal))
			{
				return enumValueType2.displayname.Resolve();
			}
		}
		return ProductId;
	}
}
