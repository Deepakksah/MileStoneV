using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Installer.Manager.CertificateCommon;

public static class CertificateModelInitializer
{
	private class ConsolidatedServerCertificateGroup : ServerCertificateGroup
	{
		public IEnumerable<Func<LegacyEncryption>> LegacyEncryptionProviders { get; }

		public ConsolidatedServerCertificateGroup(ServerCertificateGroup group, IEnumerable<Func<LegacyEncryption>> legacyEncryptionProviders)
			: base(group?.Id ?? Guid.Empty, group?.TitleKey, group?.SubtitleKey, group?.EnableEncryptionParameterKey, group?.CertificateThumbprintParameterKey, group?.PresenterImplementation, null, group?.FilterStrings)
		{
			LegacyEncryptionProviders = legacyEncryptionProviders;
		}
	}

	public static void Initialize(IEnumerable<CertificatePageContent> content, bool isQuietMode, bool isUpgrade, bool? definedSecureAddress, Dictionary<string, string> parameters)
	{
		ICertificateQueryModel model = InstallerManager.Instance.ModelRepository.GetModel<ICertificateQueryModel>();
		ICertificateCmdModel model2 = InstallerManager.Instance.ModelRepository.GetModel<ICertificateCmdModel>();
		Initialize(content, isQuietMode, isUpgrade, definedSecureAddress, model, model2, parameters, new ServerConfiguratorLogWrapper());
	}

	public static void Initialize(IEnumerable<CertificatePageContent> content, bool isQuietMode, bool isUpgrade, bool? definedSecureAddress, ICertificateQueryModel queryModel, ICertificateCmdModel commandModel, Dictionary<string, string> parameters, ILogger logger)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		if (queryModel == null)
		{
			throw new ArgumentNullException("queryModel");
		}
		if (commandModel == null)
		{
			throw new ArgumentNullException("commandModel");
		}
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		IEnumerable<ClientEndpoint> enumerable = content.SelectMany((CertificatePageContent pageContent) => pageContent.ClientEndpoints);
		IEnumerable<ServerEndpoint> enumerable2 = content.SelectMany((CertificatePageContent pageContent) => pageContent.ServerEndpoints);
		IEnumerable<VideoOS.Installer.Interfaces.Content.SystemConfigurationElement> enumerable3 = content.SelectMany((CertificatePageContent pageContent) => pageContent.SystemConfigurationElements);
		IEnumerable<ServerCertificateGroup> serverGroups = new ServerCertificateGroup[1]
		{
			new ServerToServerCommunicationCertificateGroup()
		}.Concat(content.SelectMany((CertificatePageContent pageContent) => pageContent.ServerCertificateGroups));
		ValidateServerGroups(serverGroups);
		IEnumerable<ConsolidatedServerCertificateGroup> serverGroups2 = DistinctServerGroupCollection(serverGroups);
		ValidateClientEndpoints(enumerable, logger);
		ValidateServerEndpoints(enumerable2, serverGroups2);
		ValidateSystemConfiguration(enumerable3);
		IEnumerable<Guid> newServerGroupIds = AddCertificateGroups(isQuietMode, serverGroups2, queryModel, commandModel);
		IEnumerable<ServerEndpointConfiguration> serverEndpointsToBeInstalled = enumerable2.Select((ServerEndpoint x) => new ServerEndpointConfiguration(x.Id, x.DisplayNameKey, x.CertificateGroupId, x.ApplyCertificateImplementation, x.RegisterThisServerImplementationType, x.SetSqlServerImplementationType));
		commandModel.SetServerEndpointsToBeInstalled(serverEndpointsToBeInstalled);
		SetAddresses(commandModel, parameters);
		IEnumerable<ClientEndpointConfiguration> clientEndpointsToBeInstalled = enumerable.Select((ClientEndpoint x) => new ClientEndpointConfiguration(x.Id, x.DisplayNameKey, x.ServerGroupDependency, x.RegisterImplementationType, x.SetServerAddressImplementationType));
		commandModel.SetClientEndpointsToBeInstalled(clientEndpointsToBeInstalled);
		IEnumerable<VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement> configurationElementsToBeInstalled = enumerable3.Select((VideoOS.Installer.Interfaces.Content.SystemConfigurationElement sc) => new VideoOS.ServerConfigurator.Interfaces.SystemConfigurationElement(sc.Id, sc.DisplayNameKey, ConvertFeature(sc.SystemFeature), ConvertConfigurationPurpose(sc.ConfigurationPurpose), sc.ConfigurationImplementationType));
		commandModel.SetConfigurationElementsToBeInstalled(configurationElementsToBeInstalled);
		SetEncryptionSettings(isUpgrade, serverGroups2, newServerGroupIds, commandModel, parameters);
		SetLockEncryptionSettingServerToServer(serverGroups2, commandModel, definedSecureAddress);
	}

	private static void SetAddresses(ICertificateCmdModel commandModel, Dictionary<string, string> parameters)
	{
		Uri uri = GetUri("MANAGEMENTAPPNAME", parameters);
		if (uri != null)
		{
			commandModel.SetManagementServer(uri);
		}
		Uri uri2 = GetUri("MANAGEMENTAPPNAMESECURE", parameters);
		if (uri2 != null)
		{
			commandModel.SetManagementServer(uri2);
		}
		Uri uri3 = GetUri("MANAGEMENTSERVERURI", parameters);
		if (uri == null && uri2 == null && uri3 != null)
		{
			commandModel.SetManagementServer(uri3);
		}
		if (uri3 != null)
		{
			commandModel.SetPreferredSecureState(uri3.Scheme == Uri.UriSchemeHttps);
		}
	}

	private static Uri GetUri(string uriParameter, Dictionary<string, string> parameters)
	{
		if (parameters.TryGetValue(uriParameter, out var value) && Uri.TryCreate(value, UriKind.Absolute, out var result))
		{
			return result;
		}
		return null;
	}

	private static VideoOS.ServerConfigurator.Interfaces.SystemFeature ConvertFeature(VideoOS.Installer.Interfaces.Content.SystemFeature feature)
	{
		return feature switch
		{
			VideoOS.Installer.Interfaces.Content.SystemFeature.None => VideoOS.ServerConfigurator.Interfaces.SystemFeature.None, 
			VideoOS.Installer.Interfaces.Content.SystemFeature.IsManagementServer => VideoOS.ServerConfigurator.Interfaces.SystemFeature.IsManagementServer, 
			_ => throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Unexpected value of {0}: The value was {1}", "feature", feature))), 
		};
	}

	private static VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose ConvertConfigurationPurpose(VideoOS.Installer.Interfaces.Content.ConfigurationPurpose configPurpose)
	{
		return configPurpose switch
		{
			VideoOS.Installer.Interfaces.Content.ConfigurationPurpose.None => VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose.None, 
			VideoOS.Installer.Interfaces.Content.ConfigurationPurpose.ChangeHostName => VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose.ChangeHostName, 
			_ => throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Unexpected value of {0}: The value was {1}", "configPurpose", configPurpose))), 
		};
	}

	private static IEnumerable<Guid> AddCertificateGroups(bool isQuietMode, IEnumerable<ServerCertificateGroup> serverGroups, ICertificateQueryModel queryModel, ICertificateCmdModel commandModel)
	{
		List<Guid> list = new List<Guid>();
		foreach (ServerCertificateGroup serverGroup in serverGroups)
		{
			if (!queryModel.CertificateGroups.Any((CertificateGroup y) => y.Id == serverGroup.Id))
			{
				list.Add(serverGroup.Id);
				bool useEncryption = !isQuietMode;
				CertificateGroup certificateGroup = new CertificateGroup(serverGroup.Id, serverGroup.TitleKey, serverGroup.SubtitleKey, useEncryption, null, serverGroup.PresenterImplementation, serverGroup.FilterStrings);
				commandModel.AddCertificateGroup(certificateGroup);
			}
			InstallerManager.Instance.DoNotSaveParameterInInstallerInfo(new string[2] { serverGroup.EnableEncryptionParameterKey, serverGroup.CertificateThumbprintParameterKey });
		}
		return list;
	}

	private static void SetEncryptionSettings(bool isUpgrade, IEnumerable<ConsolidatedServerCertificateGroup> serverGroups, IEnumerable<Guid> newServerGroupIds, ICertificateCmdModel commandModel, Dictionary<string, string> parameters)
	{
		foreach (ConsolidatedServerCertificateGroup serverGroup in serverGroups)
		{
			bool? flag = EnableEncryptionValue(serverGroup.EnableEncryptionParameterKey, parameters);
			if (flag.HasValue)
			{
				bool value = flag.Value;
				string thumbprint = null;
				if (value)
				{
					thumbprint = CertificateThumbprintValue(serverGroup.CertificateThumbprintParameterKey, parameters);
				}
				commandModel.SetUseEncryption(value, serverGroup.Id);
				commandModel.SetCertificate(thumbprint, serverGroup.Id);
			}
			else if (isUpgrade && newServerGroupIds.Contains(serverGroup.Id))
			{
				LegacyEncryption legacyEncryption = serverGroup.LegacyEncryptionProviders.Select((Func<LegacyEncryption> x) => x()).FirstOrDefault((LegacyEncryption y) => y.HasValue);
				if (legacyEncryption != null)
				{
					commandModel.SetUseEncryption(legacyEncryption.IsEncrypted, serverGroup.Id);
					commandModel.SetCertificate(legacyEncryption.Thumbprint, serverGroup.Id);
				}
			}
		}
	}

	private static void SetLockEncryptionSettingServerToServer(IEnumerable<ConsolidatedServerCertificateGroup> serverGroups, ICertificateCmdModel commandModel, bool? isSecure)
	{
		ConsolidatedServerCertificateGroup consolidatedServerCertificateGroup = serverGroups.FirstOrDefault((ConsolidatedServerCertificateGroup y) => y.Id == ServerCertificateGroup.ServerToServerCommunicationGroupId);
		if (consolidatedServerCertificateGroup != null && isSecure.HasValue)
		{
			commandModel.SetLockEncryption(isSecure.Value, isSecure.HasValue, consolidatedServerCertificateGroup.Id);
		}
	}

	private static void ValidateServerGroups(IEnumerable<ServerCertificateGroup> serverGroups)
	{
		foreach (IGrouping<Guid, ServerCertificateGroup> item in from x in serverGroups
			group x by x.Id)
		{
			ValidateServerGroupItem(item, (ServerCertificateGroup sg) => sg.CertificateThumbprintParameterKey, "Group has different CertificateThumbprintParameterKey.", "Has CertificateThumbprintParameterKey");
			ValidateServerGroupItem(item, (ServerCertificateGroup sg) => sg.EnableEncryptionParameterKey, "Group has different EnableEncryptionParameterKey.", "Has EnableEncryptionParameterKey");
			ValidateServerGroupItem(item, (ServerCertificateGroup sg) => sg.PresenterImplementation, "Group has different PresenterImplementation.", "Has PresenterImplementation");
			ValidateServerGroupItem(item, (ServerCertificateGroup sg) => sg.TitleKey, "Group has different PresenterImplementation.", "Has PresenterImplementation");
		}
	}

	private static IEnumerable<ConsolidatedServerCertificateGroup> DistinctServerGroupCollection(IEnumerable<ServerCertificateGroup> serverGroups)
	{
		List<ConsolidatedServerCertificateGroup> list = new List<ConsolidatedServerCertificateGroup>();
		foreach (IGrouping<Guid, ServerCertificateGroup> item in from x in serverGroups
			group x by x.Id)
		{
			IEnumerable<Func<LegacyEncryption>> legacyEncryptionProviders = from y in item
				where y.LegacyEncryptionProvider != null
				select y.LegacyEncryptionProvider;
			ServerCertificateGroup serverCertificateGroup = item.First();
			list.Add(new ConsolidatedServerCertificateGroup(serverCertificateGroup, legacyEncryptionProviders));
		}
		return list;
	}

	private static void ValidateServerGroupItem<T>(IEnumerable<ServerCertificateGroup> serverGroupDuplicates, Func<ServerCertificateGroup, T> getItem, string leadingText, string itemString)
	{
		IEnumerable<IGrouping<T, ServerCertificateGroup>> source = from sgd in serverGroupDuplicates
			group sgd by getItem(sgd);
		if (source.Count() > 1)
		{
			ServerCertificateGroup certificateGroup = source.First().First();
			ServerCertificateGroup certificateGroup2 = source.Last().First();
			throw new ArgumentException(FormattableString.Invariant($"{leadingText}{ServerGroupDefinitionString(certificateGroup, itemString, getItem)}{ServerGroupDefinitionString(certificateGroup2, itemString, getItem)}"));
		}
	}

	private static FormattableString ServerGroupDefinitionString<T>(ServerCertificateGroup certificateGroup, string itemString, Func<ServerCertificateGroup, T> getItem)
	{
		return $"{Environment.NewLine}Server group {certificateGroup.TitleKey} ({certificateGroup.Id}) {itemString} {getItem(certificateGroup)}";
	}

	private static bool? EnableEncryptionValue(string EnableEncryptionParameterKey, Dictionary<string, string> parameters)
	{
		return parameters.GetValueAsNullableBoolean(EnableEncryptionParameterKey);
	}

	private static string CertificateThumbprintValue(string CertificateThumbprintParameterKey, Dictionary<string, string> parameters)
	{
		if (!parameters.TryGetValue(CertificateThumbprintParameterKey, out var value))
		{
			return null;
		}
		return value;
	}

	private static void ValidateClientEndpoints(IEnumerable<ClientEndpoint> clientEndPoints, ILogger logger)
	{
		IGrouping<Guid, ClientEndpoint> grouping = (from se in clientEndPoints
			group se by se.Id into g
			where g.Skip(1).Any()
			select g).FirstOrDefault();
		if (grouping != null)
		{
			Guid key = grouping.Key;
			string text = string.Join(", ", grouping.Select((ClientEndpoint g) => g.DisplayNameKey));
			throw new ArgumentException(FormattableString.Invariant($"Multiple client endpoints are using the same id '{key}': {text}"));
		}
		ValidateClientItem(clientEndPoints.Where((ClientEndpoint x) => x.ServerGroupDependency != Guid.Empty), (ClientEndpoint ce) => ce.ServerGroupDependency, "Client endpoints depends on multiple groups.", "depends on server group", logger, doThrow: true);
	}

	private static void ValidateClientItem<T>(IEnumerable<ClientEndpoint> clientEndPoints, Func<ClientEndpoint, T> getItem, string leadingText, string itemString, ILogger logger, bool doThrow)
	{
		IEnumerable<IGrouping<T, ClientEndpoint>> source = from ce in clientEndPoints
			group ce by getItem(ce);
		if (source.Count() > 1)
		{
			ClientEndpoint client = source.First().First();
			ClientEndpoint client2 = source.Last().First();
			string text = FormattableString.Invariant($"{leadingText}{ClientDefinitionString(client, itemString, getItem)}{ClientDefinitionString(client2, itemString, getItem)}");
			logger.LogWarning(text);
			if (doThrow)
			{
				throw new ArgumentException(text);
			}
		}
	}

	private static FormattableString ClientDefinitionString<T>(ClientEndpoint client, string itemString, Func<ClientEndpoint, T> getItem)
	{
		return $"{Environment.NewLine}Client endpoint {client.DisplayNameKey} ({client.Id}) {itemString} {getItem(client)}";
	}

	private static void ValidateServerEndpoints(IEnumerable<ServerEndpoint> serverEndPoints, IEnumerable<ServerCertificateGroup> serverGroups)
	{
		IGrouping<Guid, ServerEndpoint> grouping = (from se in serverEndPoints
			group se by se.Id into g
			where g.Skip(1).Any()
			select g).FirstOrDefault();
		if (grouping != null)
		{
			Guid key = grouping.Key;
			string text = string.Join(", ", grouping.Select((ServerEndpoint g) => g.DisplayNameKey));
			throw new ArgumentException(FormattableString.Invariant($"Multiple server endpoints are using the same id '{key}': {text}"));
		}
		HashSet<Guid> groupIds = new HashSet<Guid>(serverGroups.Select((ServerCertificateGroup g) => g.Id));
		IGrouping<Guid, ServerEndpoint> grouping2 = (from se in serverEndPoints
			where se.CertificateGroupId != Guid.Empty
			group se by se.CertificateGroupId into g
			where !groupIds.Contains(g.Key)
			select g).FirstOrDefault();
		if (grouping2 != null)
		{
			Guid key2 = grouping2.Key;
			string text2 = string.Join(", ", grouping2.Select((ServerEndpoint g) => FormattableString.Invariant($"{g.DisplayNameKey} ({g.Id})")));
			throw new ArgumentException(FormattableString.Invariant($"Unknown server certificate group '{key2}' for server endpoints: {text2}"));
		}
	}

	private static void ValidateSystemConfiguration(IEnumerable<VideoOS.Installer.Interfaces.Content.SystemConfigurationElement> systemConfigurations)
	{
		IEnumerable<VideoOS.Installer.Interfaces.Content.SystemConfigurationElement> source = systemConfigurations.Where((VideoOS.Installer.Interfaces.Content.SystemConfigurationElement sc) => sc.ConfigurationPurpose == VideoOS.Installer.Interfaces.Content.ConfigurationPurpose.None);
		if (source.Count() > 0)
		{
			string text = string.Join(", ", source.Select((VideoOS.Installer.Interfaces.Content.SystemConfigurationElement sc) => sc.DisplayNameKey));
			throw new ArgumentException(FormattableString.Invariant($"These system configurations have an invalid configuration purpose: {text}"));
		}
	}
}
