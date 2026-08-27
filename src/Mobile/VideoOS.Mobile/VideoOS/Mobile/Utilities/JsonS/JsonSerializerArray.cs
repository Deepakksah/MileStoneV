using System;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonSerializerArray : JsonSerializerEnumerable
{
	public JsonSerializerArray(string name, Type arrayType)
		: base(name, arrayType)
	{
		if (!arrayType.IsArray)
		{
			throw new ArgumentException("The underlining type is not an Array !");
		}
		SetElementType(arrayType.GetElementType());
	}
}
