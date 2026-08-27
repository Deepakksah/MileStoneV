using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

public class CommonConfigurationProvider : ICommonConfigurationProvider
{
	private readonly IFileService _fileService;

	private readonly IDirectoryService _directoryService;

	private string ConfigurationDataFilePath { get; set; }

	private ILogger Logger { get; set; }

	public CommonConfigurationProvider(ILogger logger, IFileService fileService, IDirectoryService directoryService, IApplicationResources applicationResources)
		: this(logger, fileService, directoryService, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), applicationResources?.CompanyName, "ConfigurationData.xml"))
	{
	}

	public CommonConfigurationProvider(ILogger logger, IFileService fileService, IDirectoryService directoryService, string path)
	{
		Logger = logger;
		_fileService = fileService;
		_directoryService = directoryService;
		ConfigurationDataFilePath = path;
	}

	public void LoadCertificateConfiguration(ICertificateCmdModel certificateCmdModel)
	{
		LoadModel(certificateCmdModel, null, includeVolatile: false);
	}

	public void LoadEntireModel(ICertificateCmdModel certificateCmdModel, XmlReader xmlReader)
	{
		LoadModel(certificateCmdModel, xmlReader, includeVolatile: true);
	}

	public void LoadModel(ICertificateCmdModel certificateCmdModel, XmlReader xmlReader, bool includeVolatile)
	{
		ConfigurationData2 configurationData = ReadOrCreateConfigurationData(xmlReader);
		ConfigurationToDataModel(configurationData, certificateCmdModel, includeVolatile);
	}

	private ConfigurationData2 ReadOrCreateConfigurationData(XmlReader xmlReader)
	{
		Stream stream = null;
		Logger.LogInfo(FormattableString.Invariant(FormattableStringFactory.Create("{0}", "ReadOrCreateConfigurationData")));
		ConfigurationData2 configurationData = null;
		if (xmlReader != null)
		{
			configurationData = ReadConfigurationData(xmlReader);
		}
		else
		{
			try
			{
				configurationData = ReadOrUpgradeConfigurationData();
			}
			catch (InvalidOperationException ex)
			{
				if (ex.InnerException != null)
				{
					Logger.LogError(FormattableString.Invariant($"LoadCertificateConfiguration threw InvalidOperationException with inner exception. {Environment.NewLine} {ex.InnerException}"));
				}
				else
				{
					Logger.LogError(FormattableString.Invariant($"LoadCertificateConfiguration failed.{Environment.NewLine} {ex}"));
				}
				throw;
			}
			catch (IOException ex2)
			{
				Logger.LogError(FormattableString.Invariant($"LoadCertificateConfiguration failed to open file. {Environment.NewLine} {ex2}"));
				throw;
			}
			catch (SecurityException ex3)
			{
				Logger.LogError(FormattableString.Invariant($"LoadCertificateConfiguration could not access file. {Environment.NewLine} {ex3}"));
				throw;
			}
			finally
			{
				stream?.Dispose();
			}
		}
		if (configurationData == null)
		{
			Logger.LogInfo(FormattableString.Invariant($"No configuration existed, empty configuration created"));
		}
		return configurationData ?? new ConfigurationData2();
	}

	private ConfigurationData2 ReadConfigurationData(XmlReader xmlReader)
	{
		ConfigurationData2 result = null;
		DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ConfigurationData2));
		Logger.LogInfo(FormattableString.Invariant($"LoadCertificateConfiguration started..."));
		if (xmlReader != null)
		{
			result = (ConfigurationData2)dataContractSerializer.ReadObject(xmlReader);
		}
		return result;
	}

	private ConfigurationData2 ReadOrUpgradeConfigurationData()
	{
		Logger.LogInfo(FormattableString.Invariant($"ReadOrUpgradeConfigurationData started..."));
		int num = ReadVersionData();
		if (_fileService.Exists(ConfigurationDataFilePath))
		{
			using (Stream input = _fileService.Open(ConfigurationDataFilePath, FileMode.Open, FileAccess.Read))
			{
				using XmlReader xmlReader = XmlReader.Create(input);
				Logger.LogInfo(FormattableString.Invariant($"Reading configuration version {num}. Current configuration version is {new ConfigurationVersion().Version}."));
				if (num >= 2)
				{
					ConfigurationData2 configurationData = (ConfigurationData2)new DataContractSerializer(typeof(ConfigurationData2)).ReadObject(xmlReader);
					if (num < new ConfigurationVersion().Version)
					{
						configurationData.ConfigurationVersion = new ConfigurationVersion();
					}
					return configurationData;
				}
				if (num != 1)
				{
					throw new InvalidOperationException(FormattableString.Invariant($"ReadOrUpgradeConfigurationData the version of the data is invalid ({num})."));
				}
				return new ConfigurationData2((ConfigurationData)new XmlSerializer(typeof(ConfigurationData)).Deserialize(xmlReader));
			}
		}
		return null;
	}

	private int ReadVersionData()
	{
		ConfigurationDataBase configurationDataBase = null;
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationDataBase));
		Logger.LogInfo(FormattableString.Invariant($"ReadVersionData started..."));
		if (_fileService.Exists(ConfigurationDataFilePath))
		{
			using Stream input = _fileService.Open(ConfigurationDataFilePath, FileMode.Open, FileAccess.Read);
			using XmlReader xmlReader = XmlReader.Create(input);
			configurationDataBase = (ConfigurationDataBase)xmlSerializer.Deserialize(xmlReader);
		}
		return configurationDataBase?.ConfigurationVersion.Version ?? 0;
	}

	private void ConfigurationToDataModel(ConfigurationData2 configurationData, ICertificateCmdModel certificateCmdModel, bool includeVolatile)
	{
		ConfigurationDataUtilities.PopulateToDataApplicationSettings(configurationData.ApplicationSettings, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelCertificateGroups(configurationData.CertificateGroupsData, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelServerEndpoints(configurationData.ServerEndpointConfigurations, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelClientEndpoints(configurationData.ClientEndpointConfigurations, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelSystemConfigurationElements(configurationData.ConfigurationElements, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelManagementServer(configurationData.ManagementServerAddress, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelAuthorizationServer(configurationData.AuthorizationServerAddress, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelPersistedHostName(configurationData.HostName, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelOverrideLocalManagementServer(configurationData.OverrideLocalManagementServer, certificateCmdModel);
		ConfigurationDataUtilities.PopulateToDataModelPreferredSecureState(configurationData.PreferredSecureState, certificateCmdModel);
		if (includeVolatile)
		{
			ConfigurationDataUtilities.PopulateToDataModelVolatileItems(configurationData, certificateCmdModel);
		}
		Logger.LogInfo(FormattableString.Invariant($"Datamodel updated with configuration"));
	}

	public void PersistCertificateConfiguration(ICertificateQueryModel certificateQueryModel)
	{
		PersistCertificateConfiguration(certificateQueryModel, null, includeVolatile: false);
	}

	public void PersistEntireModel(ICertificateQueryModel certificateQueryModel, XmlWriter xmlWriter)
	{
		PersistCertificateConfiguration(certificateQueryModel, xmlWriter, includeVolatile: true);
	}

	public void PersistCertificateConfiguration(ICertificateQueryModel certificateQueryModel, XmlWriter xmlWriter, bool includeVolatile)
	{
		ConfigurationData2 configurationData = ReadOrCreateConfigurationData(null);
		DataModelToConfiguration(certificateQueryModel, configurationData, includeVolatile);
		WriteConfigurationData(xmlWriter, configurationData);
	}

	public void PersistCertificateConfiguration(IServerCertificateQueryModel serverCertificateQueryModel)
	{
		ConfigurationData2 configurationData = ReadOrCreateConfigurationData(null);
		DataModelToConfiguration(serverCertificateQueryModel, configurationData);
		WriteConfigurationData(null, configurationData);
	}

	public void PersistCertificateConfiguration(IClientRegistrationQueryModel clientRegistrationQueryModel)
	{
		ConfigurationData2 configurationData = ReadOrCreateConfigurationData(null);
		DataModelToConfiguration(clientRegistrationQueryModel, configurationData);
		WriteConfigurationData(null, configurationData);
	}

	public void PersistCertificateConfiguration(IApplicationSettingsQueryModel applicationSettingsQueryModel)
	{
		ConfigurationData2 configurationData = ReadOrCreateConfigurationData(null);
		DataModelToConfiguration(applicationSettingsQueryModel, configurationData);
		WriteConfigurationData(null, configurationData);
	}

	private void WriteConfigurationData(XmlWriter xmlWriter, ConfigurationData2 configurationData)
	{
		Logger.LogInfo(FormattableString.Invariant(FormattableStringFactory.Create("{0}", "WriteConfigurationData")));
		DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ConfigurationData2));
		if (xmlWriter != null)
		{
			dataContractSerializer.WriteObject(xmlWriter, configurationData);
			return;
		}
		XmlWriterSettings settings = new XmlWriterSettings
		{
			Indent = true,
			Encoding = Encoding.Unicode
		};
		Stream stream = null;
		try
		{
			EnsureFileFolderExists(ConfigurationDataFilePath);
			stream = _fileService.Open(ConfigurationDataFilePath, FileMode.Create, FileAccess.Write);
			using XmlWriter writer = XmlWriter.Create(stream, settings);
			dataContractSerializer.WriteObject(writer, configurationData);
		}
		catch (InvalidOperationException ex)
		{
			if (ex.InnerException != null)
			{
				Logger.LogError(FormattableString.Invariant($"PersistCertificateConfiguration threw InvalidOperationException with inner exception. {Environment.NewLine} {ex.InnerException}"));
			}
			else
			{
				Logger.LogError(FormattableString.Invariant($"PersistCertificateConfiguration failed.{Environment.NewLine} {ex}"));
			}
			throw;
		}
		catch (IOException ex2)
		{
			Logger.LogError(FormattableString.Invariant($"PersistCertificateConfiguration failed to create file.{Environment.NewLine} {ex2}"));
			throw;
		}
		catch (SecurityException ex3)
		{
			Logger.LogError(FormattableString.Invariant($"PersistCertificateConfiguration could not access file.{Environment.NewLine} {ex3}"));
			throw;
		}
		finally
		{
			stream?.Dispose();
		}
	}

	private void DataModelToConfiguration(ICertificateQueryModel certificateQueryModel, ConfigurationData2 configurationData, bool IncludeVolatile)
	{
		DataModelToConfiguration((INonvolatileClientRegistrationQueryModel)certificateQueryModel, configurationData);
		DataModelToConfiguration((INonvolatileServerCertificateQueryModel)certificateQueryModel, configurationData);
		ConfigurationDataUtilities.PopulateToConfigurationApplicationSettings(configurationData.ApplicationSettings, certificateQueryModel);
		if (IncludeVolatile)
		{
			ConfigurationDataUtilities.PopulateToConfigurationServerEndpoints(configurationData.VolatileServersCertificateConfigData, certificateQueryModel.ServerEndpointsToBeInstalled);
			ConfigurationDataUtilities.PopulateToConfigurationClientEndpoints(configurationData.VolatileClientsRegistrationConfigData, certificateQueryModel.ClientEndpointsToBeInstalled);
			ConfigurationDataUtilities.PopulateToConfigurationConfigurationElements(configurationData.VolatileSystemConfigurationElements, certificateQueryModel.ConfigurationElementsToBeInstalled);
		}
		Logger.LogDebug(FormattableString.Invariant($"Datamodel converted to configuration"));
	}

	private void DataModelToConfiguration(IApplicationSettingsQueryModel applicationSettingsQueryModel, ConfigurationData2 configurationData)
	{
		ConfigurationDataUtilities.PopulateToConfigurationApplicationSettings(configurationData.ApplicationSettings, applicationSettingsQueryModel);
		Logger.LogDebug(FormattableString.Invariant($"Server application settings datamodel converted to configuration"));
	}

	private void DataModelToConfiguration(INonvolatileServerCertificateQueryModel serverCertificateQueryModel, ConfigurationData2 configurationData)
	{
		ConfigurationDataUtilities.PopulateToConfigurationCertificateGroups(configurationData.CertificateGroupsData, serverCertificateQueryModel.CertificateGroups);
		ConfigurationDataUtilities.PopulateToConfigurationServerEndpoints(configurationData.ServerEndpointConfigurations, serverCertificateQueryModel.ServerEndpointsInstalled);
		Logger.LogDebug(FormattableString.Invariant($"Server certificate datamodel converted to configuration"));
	}

	private void DataModelToConfiguration(INonvolatileClientRegistrationQueryModel clientRegistrationQueryModel, ConfigurationData2 configurationData)
	{
		ConfigurationDataUtilities.PopulateToConfigurationClientEndpoints(configurationData.ClientEndpointConfigurations, clientRegistrationQueryModel.ClientEndpointsInstalled);
		ConfigurationDataUtilities.PopulateToConfigurationConfigurationElements(configurationData.ConfigurationElements, clientRegistrationQueryModel.ConfigurationElementsInstalled);
		ConfigurationDataUtilities.PopulateToConfigurationManagementServer(configurationData, clientRegistrationQueryModel);
		ConfigurationDataUtilities.PopulateToConfigurationAuthorizationServer(configurationData, clientRegistrationQueryModel);
		ConfigurationDataUtilities.PopulateToConfigurationHostName(configurationData, clientRegistrationQueryModel);
		ConfigurationDataUtilities.PopulateToConfigurationOverrideLocalManagementServer(configurationData, clientRegistrationQueryModel);
		ConfigurationDataUtilities.PopulateToConfigurationPreferredSecureState(configurationData, clientRegistrationQueryModel);
		Logger.LogDebug(FormattableString.Invariant($"Client registration datamodel converted to configuration"));
	}

	private void EnsureFileFolderExists(string configurationDataFilePath)
	{
		_directoryService.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(configurationDataFilePath)));
	}
}
