using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class TooManyItemsException : GeminiServiceException
{
	protected override int ErrorCodeDefault => 60162;

	public TooManyItemsException()
	{
	}

	public TooManyItemsException(string message)
		: base(message)
	{
	}

	public TooManyItemsException(string message, Exception ex)
		: base(message, ex)
	{
	}

	public TooManyItemsException(int maxItems)
		: base(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60162, maxItems.ToString(CultureInfo.InvariantCulture)))
	{
		base.EventId = GeminiEventId.TooManyItemsException;
	}

	protected TooManyItemsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
