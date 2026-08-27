using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UserNamePasswordException : VmoException
{
	public UserNamePasswordException()
		: base(string.Format(CultureInfo.InvariantCulture, Resources.UsernamePasswordException))
	{
	}

	public UserNamePasswordException(Exception innerException)
		: base(string.Format(CultureInfo.InvariantCulture, Resources.UsernamePasswordException), innerException)
	{
	}
}
