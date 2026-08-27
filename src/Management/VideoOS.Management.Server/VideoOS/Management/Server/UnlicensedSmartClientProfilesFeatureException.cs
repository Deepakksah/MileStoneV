using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedSmartClientProfilesFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64023;

	public UnlicensedSmartClientProfilesFeatureException()
		: this(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64023, OemResources.Instance.SmartClient.ClientName))
	{
	}

	protected UnlicensedSmartClientProfilesFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "OemSmartClientName", OemResources.Instance.SmartClient.ClientName));
	}

	public UnlicensedSmartClientProfilesFeatureException(string message)
		: base(message)
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "OemSmartClientName", OemResources.Instance.SmartClient.ClientName));
	}

	public UnlicensedSmartClientProfilesFeatureException(string message, Exception ex)
		: base(message, ex)
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "OemSmartClientName", OemResources.Instance.SmartClient.ClientName));
	}
}
