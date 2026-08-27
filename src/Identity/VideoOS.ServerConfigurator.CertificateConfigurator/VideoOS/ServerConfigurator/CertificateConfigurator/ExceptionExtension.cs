using System;
using VideoOS.Utilities.Validation;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public static class ExceptionExtension
{
	public static bool OriginatesFrom(this Exception exception, Type exceptionType)
	{
		exceptionType.NullCheck("", "OriginatesFrom", "C:\\actions-runner\\_work\\XPCO\\XPCO\\ServerConfigurator\\Binaries\\VideoOS.ServerConfigurator.CertificateConfigurator\\ManagementServerHelper.cs", 152);
		while (exception.InnerException != null)
		{
			exception = exception.InnerException;
		}
		return exceptionType.IsAssignableFrom(exception.GetType());
	}
}
