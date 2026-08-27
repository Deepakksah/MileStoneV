using System;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonSerializerList : JsonSerializerEnumerableT
{
	public JsonSerializerList(string name, Type listType)
		: base(name, listType)
	{
	}
}
