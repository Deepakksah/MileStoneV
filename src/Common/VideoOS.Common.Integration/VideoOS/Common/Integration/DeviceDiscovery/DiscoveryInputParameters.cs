using Newtonsoft.Json;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class DiscoveryInputParameters
{
	public static DiscoveryInputParameters Empty => new DiscoveryInputParameters();

	[JsonProperty("HttpsPort")]
	public uint? HttpsPort { get; set; }

	[JsonProperty("DetectFactoryDefault")]
	public bool? DetectFactoryDefault { get; set; }

	[JsonProperty("CacheHttpsSettings")]
	public bool? CacheHttpsSettings { get; set; }

	[JsonProperty("CheckRecorderRegistration")]
	public bool? CheckRecorderRegistration { get; set; }

	[JsonProperty("Ver")]
	public uint? Version { get; set; }

	public string ToJson()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return JsonConvert.SerializeObject((object)this, new JsonSerializerSettings
		{
			NullValueHandling = (NullValueHandling)1
		});
	}

	public static DiscoveryInputParameters FromJson(string json)
	{
		return JsonConvert.DeserializeObject<DiscoveryInputParameters>(json);
	}
}
