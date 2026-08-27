using System;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services;

public class ErrorUtil
{
	public static string GetErrorName(int errorNumber)
	{
		Enum obj = null;
		if (Enum.IsDefined(typeof(GeneralError), errorNumber))
		{
			obj = (GeneralError)errorNumber;
		}
		else if (Enum.IsDefined(typeof(SessionError), errorNumber))
		{
			obj = (SessionError)errorNumber;
		}
		else if (Enum.IsDefined(typeof(SecurityError), errorNumber))
		{
			obj = (SecurityError)errorNumber;
		}
		else if (Enum.IsDefined(typeof(MessageWatchError), errorNumber))
		{
			obj = (MessageWatchError)errorNumber;
		}
		else if (Enum.IsDefined(typeof(MessageError), errorNumber))
		{
			obj = (MessageError)errorNumber;
		}
		else if (Enum.IsDefined(typeof(AppDataError), errorNumber))
		{
			obj = (AppDataError)errorNumber;
		}
		else if (Enum.IsDefined(typeof(DataTransformationError), errorNumber))
		{
			obj = (DataTransformationError)errorNumber;
		}
		else if (Enum.IsDefined(typeof(ServiceRegistrationError), errorNumber))
		{
			obj = (ServiceRegistrationError)errorNumber;
		}
		if (obj == null)
		{
			return string.Empty;
		}
		return obj.GetType().Name + "." + obj.ToString();
	}

	public static string GetErrorDescription(int errorNumber, string errorDescription)
	{
		string errorName = GetErrorName(errorNumber);
		if (string.IsNullOrEmpty(errorName))
		{
			return errorDescription;
		}
		return "[" + errorName + "]: " + errorDescription;
	}
}
