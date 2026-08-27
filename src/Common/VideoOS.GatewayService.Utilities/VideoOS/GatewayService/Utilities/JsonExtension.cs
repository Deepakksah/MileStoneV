using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace VideoOS.GatewayService.Utilities;

public static class JsonExtension
{
	public static void SetValue(this JObject baseObject, string item, JToken value)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		string[] source = item.Split('.');
		foreach (string item2 in source.Reverse().Skip(1).Reverse())
		{
			JToken obj = baseObject[item2];
			JObject val = (JObject)(object)((obj is JObject) ? obj : null);
			if (val == null)
			{
				if (baseObject[item2] != null)
				{
					throw new InvalidOperationException($"Cannot set subitem {item2} to a JObject because it already contains the element {baseObject[item2]} of type {((object)baseObject[item2]).GetType()}");
				}
				val = (JObject)(object)(baseObject[item2] = (JToken)new JObject());
			}
			baseObject = val;
		}
		string text = source.Last();
		baseObject[text] = value;
	}
}
