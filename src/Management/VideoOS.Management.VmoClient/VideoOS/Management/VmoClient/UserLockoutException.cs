using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UserLockoutException : VmoException
{
	public UserLockoutException()
		: base(string.Format(CultureInfo.InvariantCulture, Resources.UserLockoutException))
	{
	}

	protected UserLockoutException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UserLockoutException(string message)
		: base(message)
	{
	}

	public UserLockoutException(string message, Exception ex)
		: base(message, ex)
	{
	}

	public UserLockoutException(Exception innerException)
		: this(string.Format(CultureInfo.InvariantCulture, Resources.UserLockoutException), innerException)
	{
	}
}
