using System;
using System.IO;
using System.Text;

namespace VideoOS.Mobile.Utilities.JsonS;

public abstract class JsonSerializerBase : IJsonSerializer
{
	private readonly string _name;

	private readonly Type _type;

	protected JsonSerializerBase(string name, Type type)
	{
		_name = name;
		_type = type;
	}

	public virtual object Deserialize(Stream stream)
	{
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		string chars = Encoding.UTF8.GetString(array);
		return Deserialize(chars);
	}

	public virtual object Deserialize(string chars)
	{
		throw new NotImplementedException();
	}

	private string DeserializeName(string chars, out string name)
	{
		string result = chars;
		name = null;
		if (_name != null)
		{
			int num = chars.IndexOf("\"", StringComparison.InvariantCulture) + "\"".Length;
			int num2 = chars.IndexOf("\":", StringComparison.InvariantCulture);
			int startIndex = num2 + "\":".Length;
			name = chars.Substring(num, num2 - num);
			result = chars.Substring(startIndex);
		}
		return result;
	}

	public virtual void Serialize(Stream stream, object o)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Serialize(stringBuilder, o);
		byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
		stream.Write(bytes, 0, bytes.Length);
	}

	public virtual void Serialize(StringBuilder builder, object o)
	{
		SerializeName(builder);
		SerializeData(builder, o);
	}

	private void SerializeName(StringBuilder builder)
	{
		if (_name != null)
		{
			builder.Append("\"");
			builder.Append(_name);
			builder.Append("\":");
		}
	}

	public abstract void SerializeData(StringBuilder builder, object o);
}
