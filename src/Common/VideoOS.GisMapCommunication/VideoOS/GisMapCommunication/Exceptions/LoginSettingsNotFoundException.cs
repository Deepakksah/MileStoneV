using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Exceptions;

[Serializable]
public class LoginSettingsNotFoundException : Exception
{
	public ServerId ServerId { get; private set; }

	public LoginSettingsNotFoundException(ServerId serverId)
	{
		ServerId = serverId;
	}

	public LoginSettingsNotFoundException()
	{
	}

	public LoginSettingsNotFoundException(string message)
		: base(message)
	{
	}

	public LoginSettingsNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected LoginSettingsNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentException("Argument is null", "info");
		}
		string text = info.GetString("ServerId");
		if (text != null)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServerId));
			using StringReader textReader = new StringReader(text);
			ServerId = (ServerId)xmlSerializer.Deserialize(textReader);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentException("Argument is null", "info");
		}
		base.GetObjectData(info, context);
		if (ServerId != null)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServerId));
			using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			xmlSerializer.Serialize(stringWriter, ServerId);
			info.AddValue("ServerId", stringWriter.ToString());
			return;
		}
		info.AddValue("ServerId", null);
	}
}
