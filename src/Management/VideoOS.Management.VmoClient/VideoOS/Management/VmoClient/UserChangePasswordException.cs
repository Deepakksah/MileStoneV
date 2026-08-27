using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UserChangePasswordException : VmoException
{
	public UserChangePasswordException()
		: base(string.Format(CultureInfo.InvariantCulture, Resources.UserChangePasswordException))
	{
	}

	protected UserChangePasswordException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UserChangePasswordException(string message)
		: base(message)
	{
	}

	public UserChangePasswordException(string message, Exception ex)
		: base(message, ex)
	{
	}

	public UserChangePasswordException(Exception innerException)
		: this(string.Format(CultureInfo.InvariantCulture, Resources.UserChangePasswordException), innerException)
	{
	}
}
