using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ConfigurationApiExceptionUtilities
{
	public static FaultException HandleException(Exception ex)
	{
		if (ex is ArgumentNullException ex2)
		{
			return CreateArgumentNullException(ex2);
		}
		if (ex is ArgumentException ex3)
		{
			return CreateArgumentException(ex3);
		}
		if (ex is LicenseException ex4)
		{
			return CreateLicenseException(ex4);
		}
		if (ex is NotSupportedException ex5)
		{
			return CreateNotSupportedException(ex5);
		}
		if (ex is PropertyInvalidException ex6)
		{
			return CreateArgumentException(ex6);
		}
		if (ex is PathNotFoundException ex7)
		{
			return CreatePathNotFoundException(ex7);
		}
		if (ex is UnauthorizedAccessException ex8)
		{
			return CreateUnauthorizedAccessException(ex8);
		}
		return CreateServerException(ex);
	}

	private static FaultException<ArgumentNullExceptionFault> CreateArgumentNullException(ArgumentNullException ex)
	{
		ArgumentNullExceptionFault argumentNullExceptionFault = new ArgumentNullExceptionFault(ex, "ExceptionArgumentNull");
		return new FaultException<ArgumentNullExceptionFault>(code: new FaultCode(ex.GetType().Name), detail: argumentNullExceptionFault, reason: argumentNullExceptionFault.Message);
	}

	private static FaultException<ArgumentExceptionFault> CreateArgumentException(ArgumentException ex)
	{
		ArgumentExceptionFault argumentExceptionFault = new ArgumentExceptionFault(ex, "ExceptionArgument");
		return new FaultException<ArgumentExceptionFault>(code: new FaultCode(ex.GetType().Name), detail: argumentExceptionFault, reason: argumentExceptionFault.Message);
	}

	private static FaultException<ArgumentExceptionFault> CreateArgumentException(PropertyInvalidException ex)
	{
		string text = RemoveVmoMessagePrefix(ex.Message);
		ArgumentException ex2 = (string.IsNullOrEmpty(text) ? new ArgumentException(ex.PropertyName) : ((!string.IsNullOrEmpty(ex.PropertyName) && !text.Contains(ex.PropertyName)) ? new ArgumentException(text, ex.PropertyName) : new ArgumentException(text)));
		ArgumentExceptionFault argumentExceptionFault = new ArgumentExceptionFault(ex2, "ExceptionArgument");
		return new FaultException<ArgumentExceptionFault>(code: new FaultCode(ex.GetType().Name), detail: argumentExceptionFault, reason: argumentExceptionFault.Message);
	}

	public static string RemoveVmoMessagePrefix(string message)
	{
		if (message != null && Regex.IsMatch(message, "^VMO\\d+:\\s", RegexOptions.Compiled))
		{
			message = message.Substring(message.IndexOf(':') + 2);
		}
		return message;
	}

	private static FaultException<LicenseExceptionFault> CreateLicenseException(LicenseException ex)
	{
		LicenseExceptionFault licenseExceptionFault = new LicenseExceptionFault(ex, "ExceptionLicense");
		return new FaultException<LicenseExceptionFault>(code: new FaultCode(ex.GetType().Name), detail: licenseExceptionFault, reason: licenseExceptionFault.Message);
	}

	private static FaultException<NotSupportedExceptionFault> CreateNotSupportedException(NotSupportedException ex)
	{
		NotSupportedExceptionFault notSupportedExceptionFault = new NotSupportedExceptionFault(ex, "ExceptionNotSupported");
		return new FaultException<NotSupportedExceptionFault>(code: new FaultCode(ex.GetType().Name), detail: notSupportedExceptionFault, reason: notSupportedExceptionFault.Message);
	}

	private static FaultException<PathNotFoundExceptionFault> CreatePathNotFoundException(PathNotFoundException ex)
	{
		PathNotFoundExceptionFault pathNotFoundExceptionFault = new PathNotFoundExceptionFault(ex, "ExceptionPathNotFound");
		return new FaultException<PathNotFoundExceptionFault>(code: new FaultCode(ex.GetType().Name), detail: pathNotFoundExceptionFault, reason: pathNotFoundExceptionFault.Message);
	}

	private static FaultException<UnauthorizedAccessFault> CreateUnauthorizedAccessException(UnauthorizedAccessException ex)
	{
		UnauthorizedAccessFault unauthorizedAccessFault = new UnauthorizedAccessFault(ex, "ExceptionUnauthorizedAccess");
		return new FaultException<UnauthorizedAccessFault>(code: new FaultCode(ex.GetType().Name), detail: unauthorizedAccessFault, reason: unauthorizedAccessFault.Message);
	}

	public static FaultException<ServerExceptionFault> CreateServerException(Exception ex)
	{
		if (ex == null)
		{
			throw new ArgumentNullException("ex");
		}
		ServerExceptionFault serverExceptionFault = new ServerExceptionFault(ex, "ExceptionServer");
		return new FaultException<ServerExceptionFault>(code: new FaultCode(ex.GetType().Name), detail: serverExceptionFault, reason: serverExceptionFault.Message);
	}
}
