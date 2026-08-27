using System;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonSerializerEnumerableT : JsonSerializerEnumerable
{
	public JsonSerializerEnumerableT(string name, Type listType)
		: base(name, listType)
	{
		Type[] genericArguments = listType.GetGenericArguments();
		if (genericArguments.Length == 0)
		{
			throw new ArgumentException("The underlining type is not an Generic !");
		}
		SetElementType(genericArguments[0]);
	}
}
