using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class SystemSettingInvalidValueException : VmoServiceException
{
	protected override int ErrorCodeDefault => 65006;

	public SystemSettingInvalidValueException(string name, string value)
		: base(FormatMessage(name, value))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Value", value));
		base.MessageParameters.Add(1, new MessageParameter(1, "Name", name));
	}

	protected SystemSettingInvalidValueException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public SystemSettingInvalidValueException(string message)
		: base(message)
	{
	}

	public SystemSettingInvalidValueException(string message, Exception ex)
		: base(message, ex)
	{
	}

	private static string FormatMessage(string name, string value)
	{
		if (value.Length > 50)
		{
			value = value.Substring(0, 50) + "...";
		}
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65006, value, name);
	}
}
