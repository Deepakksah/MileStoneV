using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Installer.Interfaces.ModelContainer;
using VideoOS.Installer.Logging;
using VideoOS.ServerConfigurator.CertificateConfigurator;
using VideoOS.ServerConfigurator.CertificateUtilities;
using VideoOS.ServerConfigurator.ConfigurationProvider;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Installer.Manager.CertificateCommon;

public class CertificateModelContainer : ModelContainerCustomSerialized<ServerConfiguratorDataModel>
{
	private Lazy<ICommonConfigurationProvider> lazyConfigurationProvider = new Lazy<ICommonConfigurationProvider>(() => new CommonConfigurationProvider(new ServerConfiguratorLogWrapper(), new FileService(), new DirectoryService(), new ServerConfiguratorApplicationResources()));

	private ICommonConfigurationProvider ConfigurationProvider => lazyConfigurationProvider.Value;

	public static IEnumerable<Type> ImplementedInterfaces => new List<Type>
	{
		typeof(ICertificateCmdModel),
		typeof(ICertificateQueryModel)
	};

	protected override IEnumerable<Type> Interfaces => ImplementedInterfaces;

	public CertificateModelContainer()
	{
		base.Content = CreateModel();
	}

	protected override void Persist()
	{
		SetApplicationLanguage();
		ConfigurationProvider.PersistCertificateConfiguration(base.Content);
	}

	protected override ServerConfiguratorDataModel CreateModel()
	{
		return new ServerConfiguratorDataModel(new ServerConfiguratorLogWrapper(), new CertificateProvider(), new DnsService());
	}

	protected override void SerializeModel(XmlWriter xmlWriter)
	{
		ConfigurationProvider.PersistEntireModel(base.Content, xmlWriter);
	}

	protected override void DeserializeModel(XmlReader xmlReader)
	{
		ConfigurationProvider.LoadEntireModel(base.Content, xmlReader);
	}

	public void Load()
	{
		ConfigurationProvider.LoadCertificateConfiguration(base.Content);
	}

	private void SetApplicationLanguage()
	{
		if (InstallerManager.Instance.Parameters.TryGetValue("LANGUAGE", out var value))
		{
			try
			{
				base.Content.SetApplicationUICulture(new CultureInfo(value));
			}
			catch (CultureNotFoundException)
			{
				LogManager.Write(FormattableString.Invariant($"Ignoring invalid application UI culture: {value}"), TraceEventType.Warning);
			}
		}
	}
}
