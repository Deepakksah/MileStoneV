using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ConnectionFailureException : VmoException
{
	public ConnectionFailureException()
	{
	}

	public ConnectionFailureException(string server)
		: base(string.Format(CultureInfo.InvariantCulture, Resources.ConnectionFailureException, server))
	{
	}

	public ConnectionFailureException(string server, Exception innerException)
		: base(string.Format(CultureInfo.InvariantCulture, Resources.ConnectionFailureException, server), innerException)
	{
	}

	protected ConnectionFailureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
