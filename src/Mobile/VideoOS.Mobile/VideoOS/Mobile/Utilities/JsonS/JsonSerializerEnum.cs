using System;
using System.Text;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonSerializerEnum : JsonSerializerBase
{
	public JsonSerializerEnum(string name, Type type)
		: base(name, type)
	{
	}

	public override object Deserialize(string chars)
	{
		return Enum.Parse(GetType(), chars);
	}

	public override void SerializeData(StringBuilder builder, object o)
	{
		builder.Append("\"").Append(o.ToString()).Append("\"");
	}
}
