using Newtonsoft.Json;

namespace VideoOS.Management.Server.RestApi;

public class PathDataModel
{
	[JsonProperty(PropertyName = "type")]
	public string Type { get; set; }

	[JsonProperty(PropertyName = "id")]
	public string Id { get; set; }

	public PathDataModel()
	{
		Type = "";
		Id = "";
	}

	public override string ToString()
	{
		return "type:" + Type + ", id:" + Id;
	}
}
