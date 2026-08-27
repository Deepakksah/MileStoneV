using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Proxy.ConfigApi;

public class ConfigurationApiUtil
{
	public const char OuterListSeparator = ';';

	public const char InnerListSeparator = ',';

	private static readonly char[] AllSeparators = new char[2] { ';', ',' };

	private static readonly IStringSplitter _stringSplitter = new StringSplitter();

	public static Configuration GetConfiguration(ServerId serverId)
	{
		if (serverId.UserContext != null)
		{
			return serverId.UserContext.Configuration;
		}
		return Configuration.Instance;
	}

	public static string ToSeparatorString(ICollection<string> parts)
	{
		return ToSeparatorString(parts, ';');
	}

	public static Collection<string> SeparatorStringToCollection(string input)
	{
		return new Collection<string>(SeparatorStringToList(input, ';'));
	}

	public static string ToSeparatorString(IEnumerable<string> parts, char separator)
	{
		if (parts == null)
		{
			return string.Empty;
		}
		return _stringSplitter.JoinString(parts, new char[1] { separator });
	}

	public static IList<string> SeparatorStringToList(string input, char separator)
	{
		if (input == null)
		{
			return new List<string>();
		}
		return _stringSplitter.SplitString(input, new char[1] { separator }).Cast<string>().ToList();
	}

	public static string Unescape(string inputString)
	{
		if (inputString == null)
		{
			return string.Empty;
		}
		return _stringSplitter.Unescape(inputString, AllSeparators);
	}

	public static string Escape(string inputString)
	{
		if (inputString == null)
		{
			return string.Empty;
		}
		return _stringSplitter.Escape(inputString, AllSeparators);
	}

	public static Exception ExceptionHandler(Exception ex)
	{
		if (ex is ArgumentException)
		{
			return new ArgumentMIPException(ex.Message, ex);
		}
		if (ex is FaultException)
		{
			if (ex is FaultException<ArgumentExceptionFault>)
			{
				return new ArgumentMIPException(ex.Message, ex);
			}
			if (ex is FaultException<ArgumentNullExceptionFault>)
			{
				return new ArgumentNullMIPException(ex.Message, ex);
			}
			if (ex is FaultException<LicenseExceptionFault>)
			{
				return new LicenseMIPException(ex.Message, ex);
			}
			if (ex is FaultException<NotSupportedExceptionFault>)
			{
				return new NotSupportedMIPException(ex.Message, ex);
			}
			if (ex is FaultException<PathNotFoundExceptionFault>)
			{
				return new PathNotFoundMIPException(ex.Message, ex);
			}
			if (ex is FaultException<ServerExceptionFault>)
			{
				return new ServerFaultMIPException(ex.Message, ex);
			}
			if (ex is FaultException<UnauthorizedAccessFault>)
			{
				return new NotAuthorizedMIPException(ex.Message, ex);
			}
		}
		if (ex is CommunicationException)
		{
			return new CommunicationMIPException(ex.Message, ex);
		}
		if (ex is ValidateResultException)
		{
			return ex;
		}
		if (ex is RestApiMIPException)
		{
			if (ex.Message.StartsWith("PATCH Unexpected statuscode BadRequest") && ex.Message.Contains("Property cannot be changed: LastModified"))
			{
				return new ArgumentMIPException("Object update failed due to concurrent modification. Please refresh the object and try again.", ex);
			}
			return new ArgumentMIPException(ex.Message, ex);
		}
		return ex;
	}
}
