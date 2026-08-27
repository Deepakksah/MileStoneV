using System;
using Server.UACParts;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.Integration;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.Server.CertificateRegistration;

public abstract class ManagementServerEndpointRegistrarBase : ServerEndpointRegistrarBase
{
	private readonly IHttpSysSettingsFactory _httpSysSettingsFactory;

	protected IFileService FileService { get; private set; }

	protected IisHelper IisHelper
	{
		get
		{
			IisHelper obj = IisHelper1 ?? new IisHelper(new ILoggerWrapper(base.Logger));
			IisHelper result = obj;
			IisHelper1 = obj;
			return result;
		}
	}

	public IisHelper IisHelper1 { get; set; }

	public event EventHandler<IisProgressEventArgs> IisProgress;

	protected ManagementServerEndpointRegistrarBase(IHttpSysSettingsFactory httpSysSettingsFactory, IServerResources oemResources, IFileService fileService)
		: base(oemResources?.ServiceName)
	{
		_httpSysSettingsFactory = httpSysSettingsFactory;
		FileService = fileService ?? throw new ArgumentNullException("fileService");
	}

	protected override Status ExcecuteCleanup()
	{
		ServerConfig serverConfig = new ServerConfig(new FileService(), null);
		ushort recorderCommunicationApiPort = serverConfig.RecorderCommunicationApiPort;
		ushort serverPort = serverConfig.ServerPort;
		RemoveServerHttpSysConfigurationAndReservation(recorderCommunicationApiPort);
		RemoveServerHttpSysConfigurationAndReservation(serverPort);
		return base.ExcecuteCleanup();
	}

	protected void SslHelperStatus(object sender, SslStatusEventArgs e)
	{
		this.IisProgress?.Invoke(this, new IisProgressEventArgs(e));
	}

	protected static void IisEnableSsl(string websiteId, string thumbprint, string applicationName)
	{
		SslHelper.SetCertificate(websiteId, thumbprint);
		SslHelper.EnableSsl(websiteId, applicationName);
	}

	protected void IisDisableSsl(string websiteId, string applicationName)
	{
		this.IisProgress?.Invoke(this, new IisProgressEventArgs(IisProgressStep.DisableSsl));
		SslHelper.DisableSsl(websiteId, applicationName);
	}

	public void RemoveServerHttpSysConfigurationAndReservation(ushort port)
	{
		base.Logger?.LogInfo(FormattableString.Invariant($"Removing HTTP.SYS configuration for server service..."));
		try
		{
			_httpSysSettingsFactory.Create(WellKnownAppIds.ServerApiService, port).DeleteSettings();
			base.Logger?.LogInfo(FormattableString.Invariant($"HTTP.SYS configuration for server service removed..."));
		}
		catch (Exception ex)
		{
			base.Logger?.LogError(FormattableString.Invariant($"Failed to delete HTTP sys  settings for server service during uninstallation {Environment.NewLine}{ex}"));
		}
	}
}
