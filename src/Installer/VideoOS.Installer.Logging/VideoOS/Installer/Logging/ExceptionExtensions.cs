using System;
using System.Globalization;

namespace VideoOS.Installer.Logging;

public static class ExceptionExtensions
{
	public static string TypeAndMessage(this Exception ex)
	{
		if (ex == null)
		{
			throw new ArgumentNullException("ex");
		}
		if (ex.InnerException == null)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", ex.GetType().Name, ex.Message);
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}: {1} ---> {2}", ex.GetType().Name, ex.Message, ex.InnerException.TypeAndMessage());
	}
}
