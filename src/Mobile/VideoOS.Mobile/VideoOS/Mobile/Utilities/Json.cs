using Newtonsoft.Json;

namespace VideoOS.Mobile.Utilities;

public class Json
{
	public static string SerializeObject(object obj)
	{
		return JsonConvert.SerializeObject(obj);
	}
}
