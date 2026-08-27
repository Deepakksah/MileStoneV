using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RecordingStorageInformationException : VmoException
{
	public RecordingStorageInformationException()
	{
	}

	public RecordingStorageInformationException(string message)
		: base(message)
	{
	}

	public RecordingStorageInformationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingStorageInformationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
