using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.Serializers;

public class JsonMessageSerializer<T> : IMessageSerializer<T>
{
	private readonly JsonSerializer _serializer = new JsonSerializer();

	public T Deserialize(Stream stream)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		using StreamReader streamReader = new StreamReader(stream);
		JsonTextReader val = new JsonTextReader((TextReader)streamReader);
		try
		{
			return _serializer.Deserialize<T>((JsonReader)(object)val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void Serialize(T message, Stream stream)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		using StreamWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), 1024, leaveOpen: true);
		JsonTextWriter val = new JsonTextWriter((TextWriter)streamWriter);
		try
		{
			((JsonWriter)val).CloseOutput = false;
			_serializer.Serialize((JsonWriter)(object)val, (object)message, typeof(T));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
