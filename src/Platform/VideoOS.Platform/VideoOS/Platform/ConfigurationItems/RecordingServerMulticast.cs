using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RecordingServerMulticast : IConfigurationItem
{
	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string IPAddressStart
	{
		get
		{
			return base.Wrapper.GetPropertyValue("IPAddressStart");
		}
		set
		{
			base.Wrapper.SetPropertyValue("IPAddressStart", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string IPAddressEnd
	{
		get
		{
			return base.Wrapper.GetPropertyValue("IPAddressEnd");
		}
		set
		{
			base.Wrapper.SetPropertyValue("IPAddressEnd", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int PortRangeStart
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PortRangeStart");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PortRangeStart", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int PortRangeEnd
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PortRangeEnd");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PortRangeEnd", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string MTU
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MTU");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MTU", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string TTL
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TTL");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TTL", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public RecordingServerMulticast(ServerId serverId, ConfigurationItem recordingServerMulticast)
		: base(serverId, recordingServerMulticast)
	{
	}

	public RecordingServerMulticast(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RecordingServerMulticast(FQID fqid)
		: base(fqid)
	{
	}
}
