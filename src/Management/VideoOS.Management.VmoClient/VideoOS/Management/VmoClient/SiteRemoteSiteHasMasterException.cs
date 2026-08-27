using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class SiteRemoteSiteHasMasterException : VmoException
{
	public SiteRemoteSiteHasMasterException()
	{
	}

	public SiteRemoteSiteHasMasterException(string message)
		: base(message)
	{
	}

	public SiteRemoteSiteHasMasterException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SiteRemoteSiteHasMasterException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
