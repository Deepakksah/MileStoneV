using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class SystemSettingNotSupportedException : VmoServiceException
{
	protected override int ErrorCodeDefault => 65005;

	public SystemSettingNotSupportedException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	protected SystemSettingNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public SystemSettingNotSupportedException(string message, Exception ex)
		: base(message, ex)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65005, name);
	}
}
