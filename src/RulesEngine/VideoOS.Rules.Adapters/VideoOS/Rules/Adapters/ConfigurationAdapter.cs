using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.Adapters;

public class ConfigurationAdapter : IRuleConfiguration
{
	private List<(RuleModification modification, Guid eventTypeId, string xmlDefinition)> _eventTypesChanges;

	private List<(RuleModification modification, Guid deviceGroupId, Guid parentId)> _deviceGroupChanges;

	private List<(RuleModification modification, Guid deviceGroupId, Guid deviceId)> _deviceGroupMemberChanges;

	private List<(RuleModification modification, Guid serverGroupId, Guid parentId)> _serverGroupChanges;

	private List<(RuleModification modification, Guid serverGroupId, Guid serverId)> _serverGroupMembershipChanges;

	private List<(RuleModification modification, Guid recorderId)> _recorderChanges;

	private List<(RuleModification modification, Guid VariableId, string name, string value, bool reset)> _variableManagerChanges;

	private List<(RuleModification modification, Guid ruleId, string name, string xmlDefinition, bool enabled, bool doClearEvent)> _ruleChanges;

	private List<(RuleModification modification, Guid timeProfileId, string timeProfileType, string xmlDefinition)> _timeProfileChanges;

	public bool InitialConfiguration { get; private set; }

	public IEnumerable<(RuleModification modification, Guid eventTypeDeleteId, string xmlDefinition)> EventTypesChanges => _eventTypesChanges;

	public IEnumerable<(RuleModification modification, Guid deviceGroupId, Guid parentId)> DeviceGroupChanges => _deviceGroupChanges;

	public IEnumerable<(RuleModification modification, Guid deviceGroupId, Guid deviceId)> DeviceGroupMemberChanges => _deviceGroupMemberChanges;

	public IEnumerable<(RuleModification modification, Guid serverGroupId, Guid parentId)> ServerGroupChanges => _serverGroupChanges;

	public IEnumerable<(RuleModification modification, Guid serverGroupId, Guid serverId)> ServerGroupMembershipChanges => _serverGroupMembershipChanges;

	public IEnumerable<(RuleModification modification, Guid recorderId)> RecorderChanges => _recorderChanges;

	public IEnumerable<(RuleModification modification, Guid VariableId, string name, string value, bool reset)> RuleVariablesChanges => _variableManagerChanges;

	public IEnumerable<(RuleModification modification, Guid ruleId, string name, string xmlDefinition, bool enabled, bool doClearEvent)> RuleChanges => _ruleChanges;

	public IEnumerable<(RuleModification modification, Guid timeProfileId, string timeProfileType, string xmlDefinition)> TimeProfileChanges => _timeProfileChanges;

	public bool HasAnyChanges
	{
		get
		{
			List<(RuleModification modification, Guid eventTypeId, string xmlDefinition)> eventTypesChanges = _eventTypesChanges;
			if (eventTypesChanges == null || eventTypesChanges.Count <= 0)
			{
				List<(RuleModification modification, Guid deviceGroupId, Guid parentId)> deviceGroupChanges = _deviceGroupChanges;
				if (deviceGroupChanges == null || deviceGroupChanges.Count <= 0)
				{
					List<(RuleModification modification, Guid deviceGroupId, Guid deviceId)> deviceGroupMemberChanges = _deviceGroupMemberChanges;
					if (deviceGroupMemberChanges == null || deviceGroupMemberChanges.Count <= 0)
					{
						List<(RuleModification modification, Guid serverGroupId, Guid parentId)> serverGroupChanges = _serverGroupChanges;
						if (serverGroupChanges == null || serverGroupChanges.Count <= 0)
						{
							List<(RuleModification modification, Guid serverGroupId, Guid serverId)> serverGroupMembershipChanges = _serverGroupMembershipChanges;
							if (serverGroupMembershipChanges == null || serverGroupMembershipChanges.Count <= 0)
							{
								List<(RuleModification modification, Guid recorderId)> recorderChanges = _recorderChanges;
								if (recorderChanges == null || recorderChanges.Count <= 0)
								{
									List<(RuleModification modification, Guid VariableId, string name, string value, bool reset)> variableManagerChanges = _variableManagerChanges;
									if (variableManagerChanges == null || variableManagerChanges.Count <= 0)
									{
										List<(RuleModification modification, Guid ruleId, string name, string xmlDefinition, bool enabled, bool doClearEvent)> ruleChanges = _ruleChanges;
										if (ruleChanges == null || ruleChanges.Count <= 0)
										{
											List<(RuleModification modification, Guid timeProfileId, string timeProfileType, string xmlDefinition)> timeProfileChanges = _timeProfileChanges;
											if (timeProfileChanges == null)
											{
												return false;
											}
											return timeProfileChanges.Count > 0;
										}
									}
								}
							}
						}
					}
				}
			}
			return true;
		}
	}

	public ILogger Logger { get; }

	public ConfigurationAdapter(ConfigurationDataSet configurationDataSet)
	{
		if (configurationDataSet == null)
		{
			throw new ArgumentNullException("configurationDataSet");
		}
		InitialConfiguration = true;
		_eventTypesChanges = new List<(RuleModification, Guid, string)>();
		foreach (ConfigurationDataSet.EventTypesRow row in configurationDataSet.EventTypes.Rows)
		{
			_eventTypesChanges.Add((RuleModification.Add, default(Guid), row.Definition));
		}
		_deviceGroupChanges = new List<(RuleModification, Guid, Guid)>();
		foreach (ConfigurationDataSet.DeviceGroupsRow row2 in configurationDataSet.DeviceGroups.Rows)
		{
			_deviceGroupChanges.Add((RuleModification.Add, new Guid(row2.IDDeviceGroup), row2.IsIDParentDeviceGroupNull() ? Guid.Empty : new Guid(row2.IDParentDeviceGroup)));
		}
		_deviceGroupMemberChanges = new List<(RuleModification, Guid, Guid)>();
		foreach (ConfigurationDataSet.DeviceGroupMembersRow row3 in configurationDataSet.DeviceGroupMembers.Rows)
		{
			_deviceGroupMemberChanges.Add((RuleModification.Add, new Guid(row3.IDDeviceGroup), new Guid(row3.IDDevice)));
		}
		_serverGroupChanges = new List<(RuleModification, Guid, Guid)>();
		foreach (ConfigurationDataSet.ServerGroupRow row4 in configurationDataSet.ServerGroup.Rows)
		{
			_serverGroupChanges.Add((RuleModification.Add, new Guid(row4.IDServerGroup), string.IsNullOrWhiteSpace(row4.IDParentGroup) ? Guid.Empty : new Guid(row4.IDParentGroup)));
		}
		_serverGroupMembershipChanges = new List<(RuleModification, Guid, Guid)>();
		foreach (ConfigurationDataSet.ServerGroupMembershipRow row5 in configurationDataSet.ServerGroupMembership.Rows)
		{
			_serverGroupMembershipChanges.Add((RuleModification.Add, new Guid(row5.IDServerGroup), new Guid(row5.IDObject)));
		}
		_recorderChanges = new List<(RuleModification, Guid)>();
		foreach (ConfigurationDataSet.RecordersRow recorder in configurationDataSet.Recorders)
		{
			_recorderChanges.Add((RuleModification.Add, new Guid(recorder.IDRecorder)));
		}
		_timeProfileChanges = new List<(RuleModification, Guid, string, string)>();
		foreach (ConfigurationDataSet.TimeProfileRow row6 in configurationDataSet.TimeProfile.Rows)
		{
			_timeProfileChanges.Add((RuleModification.Add, new Guid(row6.pkid), row6.type, row6.xml_data));
		}
		_variableManagerChanges = new List<(RuleModification, Guid, string, string, bool)>();
		foreach (ConfigurationDataSet.RuleVariablesRow row7 in configurationDataSet.RuleVariables.Rows)
		{
			_variableManagerChanges.Add((RuleModification.Add, new Guid(row7.IDVariable), row7.Name, row7.Value, false));
		}
		_ruleChanges = new List<(RuleModification, Guid, string, string, bool, bool)>();
		_ruleChanges = new List<(RuleModification, Guid, string, string, bool, bool)>();
		foreach (ConfigurationDataSet.RulesRow row8 in configurationDataSet.Rules.Rows)
		{
			_ruleChanges.Add((RuleModification.Add, new Guid(row8.IDRule), row8.Name, row8.Definition, row8.Enabled, false));
		}
	}

	public ConfigurationAdapter(ConfigureRulesInfo rulesInfo, ILogger logger)
	{
		Logger = logger;
		if (rulesInfo == null)
		{
			throw new ArgumentNullException("rulesInfo");
		}
		ConfigureRuleEvents(rulesInfo);
		ConfigureRuleGroups(rulesInfo);
		try
		{
			UpdateRuleVariablesConfiguration(rulesInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("RuleEngine: Unable to update rule variable configuration. Error: {Exception}", ex2);
		}
		_ruleChanges = new List<(RuleModification, Guid, string, string, bool, bool)>();
		foreach (ConfigureRulesInfo.RuleConfigInfo ruleChange in rulesInfo.RuleChanges)
		{
			try
			{
				switch (ruleChange.Action)
				{
				case ConfigureRulesInfo.RuleAction.Add:
					_ruleChanges.Add((RuleModification.Add, new Guid(ruleChange.RuleRow.IDRule), ruleChange.RuleRow.Name, ruleChange.RuleRow.Definition, ruleChange.RuleRow.Enabled, false));
					break;
				case ConfigureRulesInfo.RuleAction.Update:
				{
					bool item = ruleChange.RuleRow.Enabled != ruleChange.RuleRow.GetEnabled(DataRowVersion.Original) || ruleChange.RuleRow.Priority != ruleChange.RuleRow.GetPriority(DataRowVersion.Original) || ruleChange.RuleRow.Definition != ruleChange.RuleRow.GetDefinition(DataRowVersion.Original);
					_ruleChanges.Add((RuleModification.Update, new Guid(ruleChange.RuleRow.IDRule), ruleChange.RuleRow.Name, ruleChange.RuleRow.Definition, ruleChange.RuleRow.Enabled, item));
					break;
				}
				case ConfigureRulesInfo.RuleAction.Remove:
					_ruleChanges.Add((RuleModification.Remove, new Guid(ruleChange.RuleRow.IDRule), null, null, false, false));
					break;
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				Logger.LogError("ConfigureRules: Error updating rule, Rule: {RuleId} ({RuleName}), exception: {Exception}", ruleChange.RuleRow.IDRule, ruleChange.RuleRow.Name, ex4);
			}
		}
		UpdateTimeProfileConfiguration(rulesInfo);
	}

	private void ConfigureRuleEvents(ConfigureRulesInfo rulesInfo)
	{
		if (rulesInfo.EventTypesChanges == null)
		{
			return;
		}
		_eventTypesChanges = new List<(RuleModification, Guid, string)>();
		foreach (ConfigurationDataSet.EventTypesRow eventTypesChange in rulesInfo.EventTypesChanges)
		{
			switch (eventTypesChange.RowState)
			{
			case DataRowState.Added:
				_eventTypesChanges.Add((RuleModification.Add, default(Guid), eventTypesChange.Definition));
				break;
			case DataRowState.Modified:
				try
				{
					string g2 = (string)eventTypesChange[ConfigurationDataSet.ReadOnlyInstance.EventTypes.IDEventTypeColumn.ColumnName, DataRowVersion.Original];
					Guid item2 = new Guid(g2);
					_eventTypesChanges.Add((RuleModification.Update, item2, eventTypesChange.Definition));
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex4)
				{
					Logger.LogError("Failed to register modified event type definition in RuleEngine. Error: {Exception}", ex4);
				}
				break;
			case DataRowState.Deleted:
				try
				{
					string g = (string)eventTypesChange[ConfigurationDataSet.ReadOnlyInstance.EventTypes.IDEventTypeColumn.ColumnName, DataRowVersion.Original];
					Guid item = new Guid(g);
					_eventTypesChanges.Add((RuleModification.Remove, item, null));
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					Logger.LogError("Failed to unregister deleted event type definition in RuleEngine. Error: {Exception}", ex2);
				}
				break;
			}
		}
	}

	private void ConfigureRuleGroups(ConfigureRulesInfo rulesInfo)
	{
		try
		{
			if (rulesInfo.DeviceGroupChanges != null)
			{
				_deviceGroupChanges = new List<(RuleModification, Guid, Guid)>();
				foreach (ConfigurationDataSet.DeviceGroupsRow deviceGroupChange in rulesInfo.DeviceGroupChanges)
				{
					switch (deviceGroupChange.RowState)
					{
					case DataRowState.Added:
						_deviceGroupChanges.Add((RuleModification.Add, new Guid(deviceGroupChange.IDDeviceGroup), deviceGroupChange.IsIDParentDeviceGroupNull() ? Guid.Empty : new Guid(deviceGroupChange.IDParentDeviceGroup)));
						break;
					case DataRowState.Deleted:
						_deviceGroupChanges.Add((RuleModification.Remove, new Guid(deviceGroupChange.GetIDDeviceGroup(DataRowVersion.Original)), default(Guid)));
						break;
					}
				}
			}
			if (rulesInfo.DeviceGroupMemberChanges != null)
			{
				_deviceGroupMemberChanges = new List<(RuleModification, Guid, Guid)>();
				foreach (ConfigurationDataSet.DeviceGroupMembersRow row in rulesInfo.DeviceGroupMemberChanges.Rows)
				{
					switch (row.RowState)
					{
					case DataRowState.Added:
						_deviceGroupMemberChanges.Add((RuleModification.Add, new Guid(row.IDDeviceGroup), new Guid(row.IDDevice)));
						break;
					case DataRowState.Deleted:
						_deviceGroupMemberChanges.Add((RuleModification.Remove, new Guid(row.GetIDDeviceGroup(DataRowVersion.Original)), new Guid(row.GetIDDevice(DataRowVersion.Original))));
						break;
					}
				}
			}
			if (rulesInfo.ServerGroupChanges != null)
			{
				_serverGroupChanges = new List<(RuleModification, Guid, Guid)>();
				foreach (ConfigurationDataSet.ServerGroupRow serverGroupChange in rulesInfo.ServerGroupChanges)
				{
					switch (serverGroupChange.RowState)
					{
					case DataRowState.Added:
						_serverGroupChanges.Add((RuleModification.Add, new Guid(serverGroupChange.IDServerGroup), string.IsNullOrWhiteSpace(serverGroupChange.IDParentGroup) ? Guid.Empty : new Guid(serverGroupChange.IDParentGroup)));
						break;
					case DataRowState.Deleted:
						_serverGroupChanges.Add((RuleModification.Remove, new Guid(serverGroupChange.GetIDServerGroup(DataRowVersion.Original)), default(Guid)));
						break;
					}
				}
			}
			if (rulesInfo.ServerGroupMembershipChanges != null)
			{
				_serverGroupMembershipChanges = new List<(RuleModification, Guid, Guid)>();
				foreach (ConfigurationDataSet.ServerGroupMembershipRow row2 in rulesInfo.ServerGroupMembershipChanges.Rows)
				{
					switch (row2.RowState)
					{
					case DataRowState.Added:
						_serverGroupMembershipChanges.Add((RuleModification.Add, new Guid(row2.IDServerGroup), new Guid(row2.IDObject)));
						break;
					case DataRowState.Deleted:
						_serverGroupMembershipChanges.Add((RuleModification.Remove, new Guid(row2.GetIDServerGroup(DataRowVersion.Original)), new Guid(row2.GetIDObject(DataRowVersion.Original))));
						break;
					}
				}
			}
			if (rulesInfo.RecorderChanges == null)
			{
				return;
			}
			_recorderChanges = new List<(RuleModification, Guid)>();
			foreach (ConfigurationDataSet.RecordersRow row3 in rulesInfo.RecorderChanges.Rows)
			{
				switch (row3.RowState)
				{
				case DataRowState.Added:
					_recorderChanges.Add((RuleModification.Add, new Guid(row3.IDRecorder)));
					break;
				case DataRowState.Deleted:
					_recorderChanges.Add((RuleModification.Remove, new Guid(row3.GetIdRecorder(DataRowVersion.Original))));
					break;
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("RuleEngine: Unable to update group configuration. Error: {Exception}", ex2);
		}
	}

	public void UpdateRuleVariablesConfiguration(ConfigureRulesInfo rulesInfo)
	{
		if (rulesInfo == null)
		{
			throw new ArgumentNullException("rulesInfo");
		}
		if (rulesInfo.RuleVariablesChanges == null)
		{
			return;
		}
		_variableManagerChanges = new List<(RuleModification, Guid, string, string, bool)>();
		foreach (ConfigurationDataSet.RuleVariablesRow ruleVariablesChange in rulesInfo.RuleVariablesChanges)
		{
			switch (ruleVariablesChange.RowState)
			{
			case DataRowState.Added:
				_variableManagerChanges.Add((RuleModification.Add, new Guid(ruleVariablesChange.IDVariable), ruleVariablesChange.Name, ruleVariablesChange.Value, false));
				break;
			case DataRowState.Modified:
				_variableManagerChanges.Add((RuleModification.Update, new Guid(ruleVariablesChange.IDVariable), ruleVariablesChange.Name, ruleVariablesChange.Value, ruleVariablesChange.Reset));
				break;
			case DataRowState.Deleted:
			{
				Guid item = new Guid((string)ruleVariablesChange[ConfigurationDataSet.ReadOnlyInstance.RuleVariables.IDVariableColumn.ColumnName, DataRowVersion.Original]);
				_variableManagerChanges.Add((RuleModification.Remove, item, null, null, false));
				break;
			}
			}
		}
	}

	public void UpdateTimeProfileConfiguration(ConfigureRulesInfo rulesInfo)
	{
		if (rulesInfo == null)
		{
			throw new ArgumentNullException("rulesInfo");
		}
		if (rulesInfo.TimeProfileChanges == null)
		{
			return;
		}
		_timeProfileChanges = new List<(RuleModification, Guid, string, string)>();
		foreach (ConfigurationDataSet.TimeProfileRow row in rulesInfo.TimeProfileChanges.Rows)
		{
			switch (row.RowState)
			{
			case DataRowState.Added:
			case DataRowState.Modified:
				_timeProfileChanges.Add((RuleModification.Add, new Guid(row.pkid), row.type, row.xml_data));
				break;
			case DataRowState.Deleted:
				_timeProfileChanges.Add((RuleModification.Remove, new Guid(row.GetPkid(DataRowVersion.Original)), null, null));
				break;
			}
		}
	}
}
