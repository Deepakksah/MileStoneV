using System;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public abstract class AccessControlTypeToStringConverter
{
	private const string StateType = "StateType";

	private const string CommandType = "CommandType";

	private const string EventType = "EventType";

	private const string ServerType = "ServerType";

	private const string UnitType = "UnitType";

	public static string TypeToString(Type accessControlType)
	{
		if (accessControlType == typeof(AccessControlStateType))
		{
			return "StateType";
		}
		if (accessControlType == typeof(AccessControlCommandType))
		{
			return "CommandType";
		}
		if (accessControlType == typeof(AccessControlEventType))
		{
			return "EventType";
		}
		if (accessControlType == typeof(AccessControlServerType))
		{
			return "ServerType";
		}
		if (accessControlType == typeof(AccessControlUnit))
		{
			return "UnitType";
		}
		return string.Empty;
	}

	public static Type TypeFromString(string accessControlTypeString)
	{
		return accessControlTypeString switch
		{
			"StateType" => typeof(AccessControlStateType), 
			"CommandType" => typeof(AccessControlCommandType), 
			"EventType" => typeof(AccessControlEventType), 
			"ServerType" => typeof(AccessControlServerType), 
			"UnitType" => typeof(AccessControlUnit), 
			_ => null, 
		};
	}
}
