using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonSerializerEnumerable : JsonSerializerBase
{
	private IJsonSerializer _elementSerializer;

	private Type _elementType;

	public JsonSerializerEnumerable(string name, Type enumerableType)
		: base(name, enumerableType)
	{
	}

	protected void SetElementType(Type elementType)
	{
		_elementType = elementType;
		_elementSerializer = SerializerHelper.GetSerializer(_elementType, null);
	}

	public override void SerializeData(StringBuilder builder, object o)
	{
		builder.Append("[");
		bool flag = true;
		if (o is IEnumerable enumerable)
		{
			foreach (object item in enumerable)
			{
				try
				{
					if (!flag)
					{
						builder.Append(",");
					}
					_elementSerializer.Serialize(builder, item);
					flag = false;
				}
				catch (TargetException)
				{
				}
			}
		}
		builder.Append("]");
	}
}
