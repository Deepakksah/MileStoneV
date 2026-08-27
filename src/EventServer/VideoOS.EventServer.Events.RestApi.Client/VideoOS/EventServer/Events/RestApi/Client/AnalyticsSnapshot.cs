using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsSnapshot
{
	[JsonProperty("timeOffset", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int TimeOffset { get; set; }

	[JsonProperty("width", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int Width { get; set; }

	[JsonProperty("height", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int Height { get; set; }

	[JsonProperty("hasOverlay", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool HasOverlay { get; set; }

	[JsonProperty("sizeInBytes", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int SizeInBytes { get; set; }

	[JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public byte[] Image { get; set; }

	[JsonProperty("path", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Path { get; set; }

	[JsonProperty("source", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Source { get; set; }
}
