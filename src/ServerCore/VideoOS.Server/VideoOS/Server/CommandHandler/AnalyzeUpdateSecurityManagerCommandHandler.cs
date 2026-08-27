using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Security;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Telemetry;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server.CommandHandler;

public sealed class AnalyzeUpdateSecurityManagerCommandHandler : ICommandHandler
{
	private Server _server;

	public AnalyzeUpdateSecurityManagerCommandHandler(Server server)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		_server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is AnalyzeUpdateSecurityManagerCommand analyzeUpdateSecurityManagerCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		if (analyzeUpdateSecurityManagerCommand.SecurityManagerDto.DataSet == null)
		{
			responseSink.AddResponse(new SuccessResponse());
			return;
		}
		SecurityDataSet changes = analyzeUpdateSecurityManagerCommand.SecurityManagerDto.DataSet.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified);
		Dictionary<Guid, SecurityDataSet> recorderChanges = new Dictionary<Guid, SecurityDataSet>();
		if (analyzeUpdateSecurityManagerCommand.DoAuditLog)
		{
			bool result;
			using (ISystemSettingsComponent systemSettingsComponent = _server.DataComponentFactory.CreateSystemSettingsComponent())
			{
				if (!bool.TryParse(systemSettingsComponent.QuerySystemSettingByName(SettingList.DisablePermissionAuditLogs), out result))
				{
					result = false;
				}
			}
			if (!result)
			{
				AuditLogChanges(_server.AuditAndSystemLogger, changes, callContextInfo);
			}
		}
		try
		{
			AnalyzeChangesByRecorder(changes, recorderChanges);
			AnalyzeChangesForAllRecorders(changes, recorderChanges);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogError("Failed to analyze security update diffgram. Exception: {Exception}", ex2);
			responseSink.AddResponse(new ExceptionResponse(ex2));
			return;
		}
		try
		{
			SendUpdatesToRecorders(recorderChanges, callContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		responseSink.AddResponse(new SuccessResponse());
	}

	private static void AuditLogChanges(ILocalizedLogger auditAndSystemLogger, SecurityDataSet allChanges, ICallContextInfo callContextInfo)
	{
		if (allChanges == null)
		{
			return;
		}
		try
		{
			ServerInfoCollector serverInfoCollector = new ServerInfoCollector();
			using VmoCache vmoCache = new VmoCache(ServerConfig.Instance);
			using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
			DataSetAuditLogAnalyzer dataSetAuditLogAnalyzer = new DataSetAuditLogAnalyzer(vmoCache, configurationDataComponent, auditAndSystemLogger);
			List<(DateTime, LogEntryAudit)> list = dataSetAuditLogAnalyzer.AnalyzeAndCreateAuditLogs(allChanges, serverInfoCollector.ManagementServerName(), callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
			DateTime dateTime = list.Select(((DateTime, LogEntryAudit) i) => i.Item1).Max();
			TimeSpan timeSpan = dateTime - DateTime.UtcNow;
			foreach (var item3 in list)
			{
				item3.Item2.TimeStampUtc = item3.Item1 - timeSpan;
				LogEntryAudit item = item3.Item2;
				DateTime item2 = item3.Item1;
				item.TimeStampLocal = item2.ToLocalTime() - timeSpan;
				ServerLogManager.Instance.ServerLogEntryAuditLogger.LogEntryAuditLog(item3.Item2);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	private static void AnalyzeChangesByRecorder(SecurityDataSet allChanges, Dictionary<Guid, SecurityDataSet> recorderChanges)
	{
		Dictionary<string, Guid> dictionary = new Dictionary<string, Guid>();
		if (allChanges.SecurityPermissionsDevice.Count <= 0)
		{
			return;
		}
		foreach (SecurityDataSet.SecurityPermissionsDeviceRow row in allChanges.SecurityPermissionsDevice.Rows)
		{
			try
			{
				string deviceId = row.GetDeviceId();
				if (!dictionary.TryGetValue(deviceId, out var value))
				{
					value = ConfigurationDataManager.QueryRecorderIdByDeviceId(new Guid(deviceId));
					dictionary.Add(deviceId, value);
				}
				if (value != Guid.Empty)
				{
					GetDiffgramDatasetForRecorder(recorderChanges, value).SecurityPermissionsDevice.ImportRow(row);
				}
			}
			catch (DatabaseException)
			{
			}
		}
	}

	private void AnalyzeChangesForAllRecorders(SecurityDataSet allChanges, Dictionary<Guid, SecurityDataSet> recorderChanges)
	{
		if (allChanges.SecurityGroupAccessEntry.Count == 0 && allChanges.SecurityPermissions.Count == 0 && allChanges.TimeProfileDefaultRoleRelation.Count == 0 && allChanges.SecurityRoles.Count == 0)
		{
			return;
		}
		RecorderConfigData[] array = RecorderConfigData.QueryRecorders(_server.DataComponentFactory);
		if (allChanges.SecurityGroupAccessEntry.Count > 0)
		{
			foreach (SecurityDataSet.SecurityGroupAccessEntryRow row5 in allChanges.SecurityGroupAccessEntry.Rows)
			{
				RecorderConfigData[] array2 = array;
				foreach (RecorderConfigData recorderConfigData in array2)
				{
					GetDiffgramDatasetForRecorder(recorderChanges, recorderConfigData.Id).SecurityGroupAccessEntry.ImportRow(row5);
				}
			}
		}
		if (allChanges.SecurityPermissions.Count > 0)
		{
			foreach (SecurityDataSet.SecurityPermissionsRow row6 in allChanges.SecurityPermissions.Rows)
			{
				RecorderConfigData[] array3 = array;
				foreach (RecorderConfigData recorderConfigData2 in array3)
				{
					GetDiffgramDatasetForRecorder(recorderChanges, recorderConfigData2.Id).SecurityPermissions.ImportRow(row6);
				}
			}
		}
		if (allChanges.TimeProfileDefaultRoleRelation.Count > 0)
		{
			foreach (SecurityDataSet.TimeProfileDefaultRoleRelationRow row7 in allChanges.TimeProfileDefaultRoleRelation.Rows)
			{
				RecorderConfigData[] array4 = array;
				foreach (RecorderConfigData recorderConfigData3 in array4)
				{
					GetDiffgramDatasetForRecorder(recorderChanges, recorderConfigData3.Id).TimeProfileDefaultRoleRelation.ImportRow(row7);
				}
			}
		}
		if (allChanges.SecurityRoles.Count <= 0)
		{
			return;
		}
		foreach (SecurityDataSet.SecurityRolesRow row8 in allChanges.SecurityRoles.Rows)
		{
			RecorderConfigData[] array5 = array;
			foreach (RecorderConfigData recorderConfigData4 in array5)
			{
				GetDiffgramDatasetForRecorder(recorderChanges, recorderConfigData4.Id).SecurityRoles.ImportRow(row8);
			}
		}
	}

	private static SecurityDataSet GetDiffgramDatasetForRecorder(Dictionary<Guid, SecurityDataSet> recordersAffected, Guid recorderId)
	{
		if (!recordersAffected.TryGetValue(recorderId, out var value))
		{
			value = new SecurityDataSet();
			recordersAffected.Add(recorderId, value);
		}
		return value;
	}

	private void SendUpdatesToRecorders(Dictionary<Guid, SecurityDataSet> recorderChanges, ICallContextInfo callContextInfo)
	{
		foreach (KeyValuePair<Guid, SecurityDataSet> recorderChange in recorderChanges)
		{
			Guid key = recorderChange.Key;
			if (!_server.ConfigurationLookup.IsRecorderEnabled(key))
			{
				continue;
			}
			Guid[] array = _server.RecorderCommManager.QueryRecorderServiceIds(key);
			foreach (Guid serviceId in array)
			{
				try
				{
					_server.RecorderCommManager.UpdateSecurityConfigurationAsync(serviceId, new SecurityConfigurationDto(recorderChange.Value), callContextInfo);
				}
				catch (RecorderNotFoundException)
				{
				}
				catch (NotSupportedByLegacyRecorderException)
				{
					ServerLogManager.Instance.AddLogEntry(LogEntryId.LegacyRecorderNoSecurityUpdates, _server.ConfigurationLookup.FindRecorderDisplayName(key));
				}
			}
		}
	}
}
