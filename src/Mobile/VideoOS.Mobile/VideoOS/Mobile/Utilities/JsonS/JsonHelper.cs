using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using VideoOS.Mobile.Portable.Utilities.Json;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonHelper
{
	public const string JsonObjectStart = "{";

	public const string JsonObjectEnd = "}";

	public const string JsonNameStart = "\"";

	public const string JsonNameEnd = "\":";

	public const string JsonMemberSeparator = ",";

	public const string JsonArrayStart = "[";

	public const string JsonArrayEnd = "]";

	public const string JsonStringStart = "\"";

	public const string JsonStringEnd = "\"";

	public static readonly BuildinTypesSerilization[] BuildinTypes = new BuildinTypesSerilization[17]
	{
		new BuildinTypesSerilization
		{
			Type = typeof(string),
			Serializеr = SerializeString,
			Deserializеr = (string x) => x
		},
		new BuildinTypesSerilization
		{
			Type = typeof(sbyte),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToSByte(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(short),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToInt16(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(int),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToInt32(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(long),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToInt64(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(byte),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToByte(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(ushort),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToUInt16(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(uint),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToUInt32(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(ulong),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToUInt64(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(float),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToSingle(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(double),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToDouble(x, CultureInfo.InvariantCulture)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(sbyte),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToSByte(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(decimal),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToDecimal(x, CultureInfo.InvariantCulture)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(bool),
			Serializеr = (object x) => x.ToString().ToLower(),
			Deserializеr = (string x) => Convert.ToBoolean(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(char),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToChar(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(DateTime),
			Serializеr = (object x) => x.ToString(),
			Deserializеr = (string x) => Convert.ToDateTime(x)
		},
		new BuildinTypesSerilization
		{
			Type = typeof(Guid),
			Serializеr = (object x) => SerializeString(x.ToString()),
			Deserializеr = (string x) => new Guid(x)
		}
	};

	public static string SerializeString(object item)
	{
		return new StringBuilder().Append("\"").Append(item.ToString()).Append("\"")
			.ToString();
	}

	public static BuildinTypesSerilization GetSerialization(MemberInfo info)
	{
		return GetSerialization(GetRealType(info));
	}

	public static BuildinTypesSerilization GetSerialization(Type type)
	{
		BuildinTypesSerilization result = null;
		BuildinTypesSerilization[] buildinTypes = BuildinTypes;
		foreach (BuildinTypesSerilization buildinTypesSerilization in buildinTypes)
		{
			if (type == buildinTypesSerilization.Type)
			{
				result = buildinTypesSerilization;
				break;
			}
		}
		return result;
	}

	public static Type GetRealType(MemberInfo info)
	{
		Type result = null;
		if (info.MemberType == MemberTypes.Property)
		{
			result = (info as PropertyInfo).PropertyType;
		}
		else if (info.MemberType == MemberTypes.Field)
		{
			result = (info as FieldInfo).FieldType;
		}
		return result;
	}

	public static object GetValue(MemberInfo info, object o)
	{
		object result = null;
		switch (info.MemberType)
		{
		case MemberTypes.Property:
			result = (info as PropertyInfo).GetValue(o, null);
			break;
		case MemberTypes.Field:
			result = (info as FieldInfo).GetValue(o);
			break;
		}
		return result;
	}

	public static bool IsSerializable(MemberInfo info, ref string name)
	{
		bool result = false;
		try
		{
			if (info.MemberType == MemberTypes.Field || (info.MemberType == MemberTypes.Property && (info as PropertyInfo).CanRead && (info as PropertyInfo).CanWrite))
			{
				result = true;
				object[] customAttributes = info.GetCustomAttributes(typeof(JsonSerializerAttribute), inherit: true);
				foreach (object obj in customAttributes)
				{
					if (obj is JsonElementAttribute)
					{
						name = (obj as JsonElementAttribute).Name;
						break;
					}
					if (obj is JsonIgnoreAttribute)
					{
						result = false;
						break;
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return result;
	}
}
