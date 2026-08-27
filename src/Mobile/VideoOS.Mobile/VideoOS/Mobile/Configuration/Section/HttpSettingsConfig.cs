using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.Configuration.Section;

[DataContract]
public class HttpSettingsConfig : IConfigSection
{
	public const string NameRequestTimeout = "RequestTimeout";

	public const string NameMaximumContentRequestSize = "MaximumContentRequestSize";

	[DataMember]
	public TimeSpan RequestTimeout { get; private set; }

	[DataMember]
	public long MaximumContentRequestSize { get; private set; }

	public string SectionName => "HttpSettings";

	public HttpSettingsConfig()
	{
		RequestTimeout = TimeSpan.FromSeconds(5.0);
		MaximumContentRequestSize = 52428800L;
	}

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			RequestTimeout = TimeSpan.FromSeconds(int.Parse(section["RequestTimeout"]));
			MaximumContentRequestSize = long.Parse(section["MaximumContentRequestSize"]);
		}
		catch (Exception)
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
			serverSection["RequestTimeout"] = RequestTimeout.Seconds.ToString();
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}
}
