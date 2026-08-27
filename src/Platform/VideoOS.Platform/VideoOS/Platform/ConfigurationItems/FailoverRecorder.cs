using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class FailoverRecorder : IConfigurationItem
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

	public string HostName => base.Wrapper.GetPropertyValue("HostName");

	public int PortNumber
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PortNumber");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string WebServerUri => base.Wrapper.GetPropertyValue("WebServerUri");

	public string ActiveWebServerUri => base.Wrapper.GetPropertyValue("ActiveWebServerUri");

	public bool PublicAccessEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PublicAccessEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PublicAccessEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string PublicWebserverHostName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PublicWebserverHostName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PublicWebserverHostName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int PublicWebserverPort
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PublicWebserverPort");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PublicWebserverPort", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string MulticastServerAddress
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MulticastServerAddress");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MulticastServerAddress", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int UdpPort
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("UdpPort");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("UdpPort", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int Position
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Position");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string DatabasePath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DatabasePath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DatabasePath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public FailoverRecorder(ServerId serverId, ConfigurationItem failoverRecorder)
		: base(serverId, failoverRecorder)
	{
	}

	public FailoverRecorder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public FailoverRecorder(FQID fqid)
		: base(fqid)
	{
	}
}
