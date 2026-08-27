using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class FeatureNotInstalledException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 63016;

	public FeatureNotInstalledException()
	{
	}

	public FeatureNotInstalledException(string message)
		: base(message)
	{
	}

	public FeatureNotInstalledException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FeatureNotInstalledException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public static FeatureNotInstalledException CreateException(string featureName)
	{
		return new FeatureNotInstalledException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63016, featureName))
		{
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "FeatureName", featureName)
			} }
		};
	}
}
