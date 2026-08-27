using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class InvalidMipLicenseRequestPartException : VmoServiceException
{
	protected override int ErrorCodeDefault => 65007;

	public InvalidMipLicenseRequestPartException(string message)
		: base(FormatMessage(message))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Message", message));
	}

	protected InvalidMipLicenseRequestPartException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public InvalidMipLicenseRequestPartException(string message, Exception ex)
		: base(FormatMessage(message), ex)
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Message", message));
	}

	private static string FormatMessage(string message)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65007, message);
	}
}
