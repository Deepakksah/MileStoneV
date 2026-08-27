using System;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class DeviceDefinition : IDeviceHwid
{
	private MainDefinition mainDefinition;

	private deviceType device;

	private DevicePtzSettings ptzSettings2;

	private StreamDefinitionCollection streams;

	private StreamCombinationsCollection streamCombinations;

	private MetadataItemDefinitionCollection metadataItems;

	private CapabilitiesCollection capabilities;

	private SettingsCollection settings;

	private EventDefinitionCollection events;

	public MainDefinition MainDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return mainDefinition;
		}
	}

	public deviceType Base
	{
		[DebuggerStepThrough]
		get
		{
			return device;
		}
	}

	private DevicePtzSettings ptzSettings
	{
		get
		{
			return ptzSettings2;
		}
		set
		{
			ptzSettings2 = value;
		}
	}

	public DeviceHwid Hwid
	{
		get
		{
			return new DeviceHwid(device.capabilities.hwid);
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			device.capabilities.hwid = value.Text;
		}
	}

	public DevicePtzSettings PtzSettings
	{
		get
		{
			if (device.ptzsettings == null)
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
				ptzSettings = new DevicePtzSettings(mainDefinition, device.ptzsettings);
				return ptzSettings;
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
				capabilities = new CapabilitiesCollection(mainDefinition, device.capabilities);
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
						if (string.Equals(settingsType2.hwid, Hwid.Text, StringComparison.Ordinal))
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

	public StreamDefinitionCollection Streams
	{
		get
		{
			if (streams != null)
			{
				return streams;
			}
			lock (this)
			{
				if (streams != null)
				{
					return streams;
				}
				StreamDefinitionCollection streamDefinitionCollection = new StreamDefinitionCollection();
				if (device.streams != null)
				{
					streamType[] array = device.streams;
					foreach (streamType streamType2 in array)
					{
						streamDefinitionCollection.Add(new StreamDefinition(this, streamType2));
					}
				}
				streams = streamDefinitionCollection;
				return streams;
			}
		}
	}

	public StreamCombinationsCollection StreamCombinations
	{
		get
		{
			if (streamCombinations != null)
			{
				return streamCombinations;
			}
			lock (this)
			{
				if (streamCombinations != null)
				{
					return streamCombinations;
				}
				StreamCombinationsCollection streamCombinationsCollection = new StreamCombinationsCollection();
				if (device.supportedstreamtypes != null)
				{
					supportedstreamtypeType[] supportedstreamtypes = device.supportedstreamtypes;
					foreach (supportedstreamtypeType supportedStreams in supportedstreamtypes)
					{
						streamCombinationsCollection.Add(new StreamCombinations(this, supportedStreams));
					}
				}
				streamCombinations = streamCombinationsCollection;
				return streamCombinations;
			}
		}
	}

	public MetadataItemDefinitionCollection MetadataItems
	{
		get
		{
			if (metadataItems != null)
			{
				return metadataItems;
			}
			lock (this)
			{
				if (metadataItems != null)
				{
					return metadataItems;
				}
				MetadataItemDefinitionCollection metadataItemDefinitionCollection = new MetadataItemDefinitionCollection();
				if (device.metadataitems != null)
				{
					metadataitemType[] metadataitems = device.metadataitems;
					foreach (metadataitemType metadataItemType in metadataitems)
					{
						metadataItemDefinitionCollection.Add(new MetadataItemDefinition(this, metadataItemType));
					}
				}
				metadataItems = metadataItemDefinitionCollection;
				return metadataItems;
			}
		}
	}

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
				if (device.supportedevents != null)
				{
					supportedeventType[] supportedevents = device.supportedevents;
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

	public DeviceDefinition(MainDefinition mainDefinition, deviceType device)
	{
		this.mainDefinition = mainDefinition;
		this.device = device;
	}

	public bool IsMultipleStreamsSupported()
	{
		foreach (StreamCombinations streamCombination in StreamCombinations)
		{
			if (streamCombination.Base.allowedconcurrent != null)
			{
				int num = 0;
				streamcountType[] allowedconcurrent = streamCombination.Base.allowedconcurrent;
				foreach (streamcountType streamcountType2 in allowedconcurrent)
				{
					num += Convert.ToInt32(streamcountType2.value, CultureInfo.InvariantCulture);
				}
				if (num > 1)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsHwidForThisDevice(Hwid hwid)
	{
		DeviceHwid hwid2 = Hwid;
		DeviceHwid deviceHwid = hwid as DeviceHwid;
		if (deviceHwid != null)
		{
			if (deviceHwid.Type == hwid2.Type)
			{
				return deviceHwid.Index == hwid2.Index;
			}
			return false;
		}
		DevicePtzHwid devicePtzHwid = hwid as DevicePtzHwid;
		if (devicePtzHwid != null)
		{
			if (devicePtzHwid.Type == hwid2.Type)
			{
				return devicePtzHwid.Index == hwid2.Index;
			}
			return false;
		}
		StreamHwid streamHwid = hwid as StreamHwid;
		if (streamHwid != null)
		{
			if (streamHwid.Type == hwid2.Type)
			{
				return streamHwid.DeviceIndex == hwid2.Index;
			}
			return false;
		}
		return false;
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
		foreach (StreamDefinition stream in Streams)
		{
			if (string.Equals(stream.Hwid.Text, text, StringComparison.OrdinalIgnoreCase))
			{
				return stream.Capabilities;
			}
		}
		return null;
	}
}
