using System.Collections.Generic;
using VideoOS.Platform.JsonHandling;

namespace VideoOS.Platform.Proxy.RestApi;

public class ErrorRest
{
	public int httpCode { get; set; }

	public DetailRest[] details { get; set; }

	public ErrorRest()
	{
	}

	public ErrorRest(JsonObject jsonObject)
	{
		httpCode = jsonObject.GetInteger("httpCode").Value;
		JsonObject child = jsonObject.GetChild("details");
		List<DetailRest> list = new List<DetailRest>();
		if (child != null)
		{
			foreach (JsonObject child2 in child.GetChildren())
			{
				list.Add(new DetailRest(child2));
			}
		}
		details = list.ToArray();
	}
}
