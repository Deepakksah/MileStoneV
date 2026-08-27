using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UserNotExistsException : VmoException
{
	public UserNotExistsException()
		: base(string.Format(CultureInfo.InvariantCulture, Resources.UserDoesNotExist))
	{
	}

	public UserNotExistsException(Exception innerException)
		: base(string.Format(CultureInfo.InvariantCulture, Resources.UserDoesNotExist), innerException)
	{
	}
}
