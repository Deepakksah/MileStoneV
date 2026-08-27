using VideoOS.Platform.JsonHandling;

namespace VideoOS.Platform.Proxy.RestApi;

public class DetailRest
{
	public string property { get; set; }

	public string errorText { get; set; }

	public DetailRest()
	{
	}

	public DetailRest(JsonObject jsonObject)
	{
		property = jsonObject.GetString("property");
		errorText = jsonObject.GetString("errorText");
	}
}
