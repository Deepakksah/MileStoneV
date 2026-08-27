using System;
using VideoOS.Platform.JsonHandling;

namespace VideoOS.Platform.Proxy.RestApi;

public class WellKnownInformationResponse
{
	public JsonObject JsonObject { get; set; }

	public DateTime ValidUntilDateTime { get; set; }

	public WellKnownInformationResponse(JsonObject jsonObject)
	{
		JsonObject = jsonObject;
		ValidUntilDateTime = DateTime.UtcNow + TimeSpan.FromSeconds(60.0);
	}
}
