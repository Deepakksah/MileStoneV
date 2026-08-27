using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonSerializerObject : JsonSerializerBase
{
	private class Element
	{
		public MemberInfo MemberInfo { get; set; }

		public string SerilizationName { get; set; }

		public IJsonSerializer Serializer { get; set; }
	}

	private readonly Dictionary<string, Element> _elements = new Dictionary<string, Element>();

	public JsonSerializerObject(string name, Type type)
		: base(name, type)
	{
		Initialize(type);
	}

	private void Initialize(Type type)
	{
		List<MemberInfo> list = new List<MemberInfo>();
		list.AddRange(type.GetProperties());
		list.AddRange(type.GetFields());
		foreach (MemberInfo item in list)
		{
			string name = item.Name;
			if (JsonHelper.IsSerializable(item, ref name))
			{
				_elements.Add(name, new Element
				{
					MemberInfo = item,
					Serializer = SerializerHelper.GetSerializer(item, name),
					SerilizationName = name
				});
			}
		}
	}

	public override void SerializeData(StringBuilder builder, object o)
	{
		builder.Append("{");
		bool flag = true;
		foreach (Element value2 in _elements.Values)
		{
			try
			{
				object value = JsonHelper.GetValue(value2.MemberInfo, o);
				if (value == null)
				{
					continue;
				}
				if (!flag)
				{
					builder.Append(",");
				}
				if (typeof(IDictionary).IsAssignableFrom(value.GetType()))
				{
					IDictionary obj = (IDictionary)value;
					builder.Append("\"");
					builder.Append(value2.MemberInfo.Name);
					builder.Append("\":");
					builder.Append("{");
					List<string> list = new List<string>();
					foreach (DictionaryEntry item in obj)
					{
						string text = "\"" + item.Key?.ToString() + "\"";
						string text2 = ((!(item.Value is bool flag2)) ? ("\"" + item.Value?.ToString() + "\"") : flag2.ToString().ToLower());
						list.Add(text + ":" + text2);
					}
					builder.Append(string.Join(",", list));
					builder.Append("}");
				}
				else
				{
					value2.Serializer.Serialize(builder, value);
				}
				flag = false;
			}
			catch (TargetException)
			{
			}
		}
		builder.Append("}");
	}
}
