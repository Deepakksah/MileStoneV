using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
public class ManufacturerInfoException : Exception
{
	public string DisplayMessage { get; set; }

	public ManufacturerInfoException()
	{
	}

	public ManufacturerInfoException(string message)
		: base(message)
	{
		DisplayMessage = message;
	}

	public ManufacturerInfoException(string message, Exception innerException)
		: base(message, innerException)
	{
		DisplayMessage = message;
	}

	protected ManufacturerInfoException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}
}
