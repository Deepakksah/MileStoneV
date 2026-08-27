using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Configuration;

[Serializable]
public class ConfigurationItemNotFoundException : Exception
{
	private const string NoInstanceFoundWithIdText = "No {0} instance found with the id {1}";

	public ConfigurationItemNotFoundException()
	{
	}

	public ConfigurationItemNotFoundException(string message)
		: base(message)
	{
	}

	public ConfigurationItemNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ConfigurationItemNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public static ConfigurationItemNotFoundException Create(MemberInfo configurationItemType)
	{
		return new ConfigurationItemNotFoundException(string.Format(CultureInfo.InvariantCulture, "No {0} instance found", GetTypeName(configurationItemType)));
	}

	public static ConfigurationItemNotFoundException CreateWithId(MemberInfo configurationItemType, Guid configurationItemId)
	{
		return new ConfigurationItemNotFoundException(string.Format(CultureInfo.InvariantCulture, "No {0} instance found with the id {1}", GetTypeName(configurationItemType), configurationItemId));
	}

	public static ConfigurationItemNotFoundException CreateWithId(MemberInfo configurationItemType, int configurationItemId)
	{
		return new ConfigurationItemNotFoundException(string.Format(CultureInfo.InvariantCulture, "No {0} instance found with the id {1}", GetTypeName(configurationItemType), configurationItemId));
	}

	public static ConfigurationItemNotFoundException CreateWithId(MemberInfo configurationItemType, string configurationItemId)
	{
		return new ConfigurationItemNotFoundException(string.Format(CultureInfo.InvariantCulture, "No {0} instance found with the id {1}", GetTypeName(configurationItemType), configurationItemId));
	}

	public static ConfigurationItemNotFoundException CreateWithKey(MemberInfo configurationItemType, string key)
	{
		return new ConfigurationItemNotFoundException(string.Format(CultureInfo.InvariantCulture, "No {0} instance found with the key {1}", GetTypeName(configurationItemType), key));
	}

	private static string GetTypeName(MemberInfo configurationItemType)
	{
		return configurationItemType?.Name ?? "<Unknown>";
	}
}
