using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Configuration.Section;

[DataContract]
public class AddressConfig : IConfigSection
{
	public const string NameScheme = "Scheme";

	public const string NameIp = "Ip";

	public const string NamePort = "Port";

	public const string NameEnabled = "Enabled";

	public const string NameTrue = "True";

	[DataMember]
	public string Scheme { get; set; }

	[DataMember]
	public string IP { get; set; }

	[DataMember]
	public int Port { get; set; }

	[DataMember]
	public bool Enabled { get; private set; }

	public string SectionName => "Address";

	public Uri Uri => new Uri($"{Scheme}://{IP}:{Port}");

	public AddressConfig()
	{
		Scheme = Uri.UriSchemeHttp;
		IP = "";
		Port = 0;
	}

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			string text = section["Scheme"];
			if (text != null)
			{
				if (text.Equals(Uri.UriSchemeHttp, StringComparison.CurrentCultureIgnoreCase) || text.Equals(Uri.UriSchemeHttps, StringComparison.CurrentCultureIgnoreCase))
				{
					Scheme = text;
				}
			}
			else
			{
				result = false;
			}
		}
		catch
		{
			result = false;
		}
		try
		{
			IP = section["Ip"].Sanitized(".+");
		}
		catch
		{
			result = false;
		}
		try
		{
			Port = section["Port"].Sanitized().ToInt32();
		}
		catch
		{
			result = false;
		}
		try
		{
			Enabled = section["Enabled"].Sanitized().IsTrue();
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
			serverSection["Scheme"] = Scheme;
		}
		catch
		{
			result = false;
		}
		try
		{
			serverSection["Ip"] = IP;
		}
		catch
		{
			result = false;
		}
		try
		{
			serverSection["Port"] = Port.ToString(CultureInfo.InvariantCulture);
		}
		catch
		{
			result = false;
		}
		try
		{
			serverSection["Enabled"] = Enabled.ToString(CultureInfo.InvariantCulture);
		}
		catch
		{
			result = false;
		}
		return result;
	}
}
