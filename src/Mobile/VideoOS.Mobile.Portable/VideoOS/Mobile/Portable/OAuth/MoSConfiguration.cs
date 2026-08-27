using System;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.OAuth;

public class MoSConfiguration
{
	private static string ConfigurationRelativePath = "JsonConfiguration";

	public string IdpRelativePath { get; set; }

	public static MoSConfiguration LoadMosConfiguration(Uri uri)
	{
		return JsonConvert.DeserializeObject<MoSConfiguration>(HttpHelper.PerformHTTPGet(new Uri(uri, ConfigurationRelativePath).ToString()));
	}
}
