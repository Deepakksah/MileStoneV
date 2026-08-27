using System;

namespace VideoOS.Installer.SQLExpressAdapter;

public class SqlExpressAdapterException : Exception
{
	public SqlExpressAdapterException(Exception exception)
		: base(exception.Message, exception.InnerException)
	{
	}

	public SqlExpressAdapterException(string message)
		: base(message)
	{
	}
}
