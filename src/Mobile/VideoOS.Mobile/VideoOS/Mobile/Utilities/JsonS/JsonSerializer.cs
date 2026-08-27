using System;
using System.IO;
using System.Text;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonSerializer : IJsonSerializer
{
	private readonly JsonSerializerObject _serializer;

	public JsonSerializer(Type type)
	{
		_serializer = new JsonSerializerObject(null, type);
	}

	public void Serialize(Stream stream, object o)
	{
		_serializer.Serialize(stream, o);
	}

	public void Serialize(StringBuilder builder, object o)
	{
		_serializer.Serialize(builder, o);
	}

	public object Deserialize(Stream stream)
	{
		return _serializer.Deserialize(stream);
	}
}
