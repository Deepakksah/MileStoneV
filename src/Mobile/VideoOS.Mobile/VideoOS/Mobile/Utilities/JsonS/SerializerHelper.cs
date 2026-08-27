using System;
using System.Reflection;

namespace VideoOS.Mobile.Utilities.JsonS;

public class SerializerHelper
{
	public static IJsonSerializer GetSerializer(MemberInfo info, string name)
	{
		return GetSerializer(JsonHelper.GetRealType(info), name);
	}

	public static IJsonSerializer GetSerializer(Type realType, string name)
	{
		if (realType.IsArray)
		{
			return new JsonSerializerArray(name, realType);
		}
		if (realType.IsEnum)
		{
			return new JsonSerializerEnum(name, realType);
		}
		if (realType.Name == "IEnumerable`1" && realType.Namespace == "System.Collections.Generic")
		{
			return new JsonSerializerEnumerableT(name, realType);
		}
		if (realType.Name == "List`1" && realType.Namespace == "System.Collections.Generic")
		{
			return new JsonSerializerList(name, realType);
		}
		BuildinTypesSerilization serialization = JsonHelper.GetSerialization(realType);
		if (serialization != null)
		{
			return new JsonSerializerBuildIn(name, serialization);
		}
		return new JsonSerializerObject(name, realType);
	}
}
