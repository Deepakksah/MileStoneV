using System;
using System.Collections.Specialized;
using System.Globalization;

namespace VideoOS.Mobile.Configuration.Section;

public class LoginSettingsConfig : IConfigSection
{
	public const string NameSingleServer = "SingleServer";

	public bool SingleServer { get; private set; }

	public string SectionName => "LoginSettings";

	public LoginSettingsConfig()
	{
		SingleServer = false;
	}

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			SingleServer = string.Compare(section["SingleServer"], "True", StringComparison.OrdinalIgnoreCase) == 0;
		}
		catch
		{
			result = false;
		}
		return result;
	}

	public bool ConvToCollection(NameValueCollection serverSection)
	{
		bool result = true;
		try
		{
			serverSection["SingleServer"] = SingleServer.ToString(CultureInfo.InvariantCulture);
		}
		catch
		{
			result = false;
		}
		return result;
	}
}
