using System;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.CertificateRegistration.Common;

public abstract class ServerEndpointRegistrarBase : WindowsServiceConfiguratorBase, IApplyCertificate, IServerCleanup
{
	public ServerEndpointRegistrarBase(string serviceName)
		: base(serviceName, new TimeSpan(0, 3, 0), new ServiceControllerFactory())
	{
	}

	public ServerEndpointRegistrarBase()
		: base(null, new TimeSpan(0, 3, 0), new ServiceControllerFactory())
	{
	}

	public Status PreApplyCertificate()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PreExecute(), "PreApplyCertificate");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, null, "PreApplyCertificate");
		}
	}

	public Status ApplyCertificate(string thumbprint)
	{
		try
		{
			if (!string.IsNullOrEmpty(thumbprint))
			{
				base.Logger?.LogInfo(FormattableString.Invariant($"Applying Certificate {thumbprint}"));
			}
			else
			{
				base.Logger?.LogInfo("Applying No Certificate = Unencrypted");
			}
			return LoggedIfSuccessOrWarning(Execute(thumbprint), "ApplyCertificate");
		}
		catch (Exception exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, null, "ApplyCertificate");
		}
	}

	protected virtual Status Execute(string thumbprint)
	{
		bool encryptionEnabled = !string.IsNullOrEmpty(thumbprint);
		return ModifyConfiguration(encryptionEnabled, thumbprint);
	}

	protected abstract Status ModifyConfiguration(bool encryptionEnabled, string thumbprint);

	public Status PostApplyCertificate()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PostExecute(), "PostApplyCertificate");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, null, "PostApplyCertificate");
		}
	}

	public Status CleanUp()
	{
		try
		{
			return LoggedIfSuccessOrWarning(ExcecuteCleanup(), "CleanUp");
		}
		catch (Exception exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorEncryption, null, "CleanUp");
		}
	}

	protected virtual Status ExcecuteCleanup()
	{
		return Status.SuccessStatus();
	}
}
