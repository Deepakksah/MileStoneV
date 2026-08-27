using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientSettings : IConfigurationItem
{
	public string Related
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Related");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Related", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Shortcut
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Shortcut");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Shortcut", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool MulticastEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("MulticastEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("MulticastEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ShortcutDefined
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Shortcut") != null;
		}
		set
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Shortcut");
			if (!value || propertyValue == null)
			{
				base.Wrapper.SetPropertyValue("Shortcut", value ? "0" : "");
				base.Wrapper.IsDirty = true;
			}
		}
	}

	public ClientSettings(ServerId serverId, ConfigurationItem clientSettings)
		: base(serverId, clientSettings)
	{
	}

	public ClientSettings(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientSettings(FQID fqid)
		: base(fqid)
	{
	}
}
