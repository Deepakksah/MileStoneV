using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common.Exceptions;

[Serializable]
public class FileExtractException : LocalizedException
{
	public FileExtractException()
	{
	}

	public FileExtractException(string message)
		: base(message)
	{
	}

	public FileExtractException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected FileExtractException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public FileExtractException(string message, Func<string> lookupDisplayMessage, Exception innerException)
		: base(message, lookupDisplayMessage, innerException)
	{
	}
}
