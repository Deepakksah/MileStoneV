using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionMailServer : IConfigurationItem
{
	public string SmtpSender
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SmtpSender");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SmtpSender", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string SmtpServer
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SmtpServer");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SmtpServer", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int SmtpPort
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("SmtpPort");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("SmtpPort", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool SmtpUseSsl
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("SmtpUseSsl");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("SmtpUseSsl", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool SmtpLogOn
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("SmtpLogOn");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("SmtpLogOn", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string SmtpUsername
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SmtpUsername");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SmtpUsername", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string SmtpPassword
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SmtpPassword");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SmtpPassword", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ToolOptionMailServer(ServerId serverId, ConfigurationItem toolOptionMailServer)
		: base(serverId, toolOptionMailServer)
	{
	}

	public ToolOptionMailServer(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionMailServer(FQID fqid)
		: base(fqid)
	{
	}
}
