using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VideoOS.IdentityServer.Common;

public abstract class PatchDocumentBase
{
	public string GetPatchDocument(JObject model)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		JsonPatchDocument val = new JsonPatchDocument();
		string[] array = (from x in model.Properties()
			select x.Name).ToArray();
		foreach (string text in array)
		{
			JProperty val2 = model.Property(text);
			if (val2 != null && (int)val2.Value.Type != 10)
			{
				val.Replace("/" + val2.Name, (object)val2.Value);
			}
		}
		return JsonConvert.SerializeObject((object)val);
	}
}
