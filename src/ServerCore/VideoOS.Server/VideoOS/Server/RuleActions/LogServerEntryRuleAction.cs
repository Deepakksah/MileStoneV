using System;
using System.Collections;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents;
using VideoOS.Server.Properties;

namespace VideoOS.Server.RuleActions;

public sealed class LogServerEntryRuleAction : RuleActionBase
{
	private Server _server;

	private string _text;

	private LogEntryAlias.AliasType[] _replaceAliases;

	private Guid _ruleID;

	private const string ITEMNOTFOUND = "(?)";

	public LogServerEntryRuleAction(IRuleEngine ruleEngine, Server server, Guid ruleId, string text)
		: base(ruleEngine)
	{
		_ruleID = ruleId;
		_server = server;
		ArrayList arrayList = new ArrayList();
		int num = 0;
		if (text.IndexOf("$RuleName$", StringComparison.OrdinalIgnoreCase) != -1)
		{
			text = text.Replace("$RuleName$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(LogEntryAlias.AliasType.RuleName);
			num++;
		}
		if (text.IndexOf("$TriggerTime$", StringComparison.OrdinalIgnoreCase) != -1)
		{
			text = text.Replace("$TriggerTime$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(LogEntryAlias.AliasType.TriggerTime);
			num++;
		}
		if (text.IndexOf("$EventName$", StringComparison.OrdinalIgnoreCase) != -1)
		{
			text = text.Replace("$EventName$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(LogEntryAlias.AliasType.EventName);
			num++;
		}
		if (text.IndexOf("$DeviceName$", StringComparison.OrdinalIgnoreCase) != -1)
		{
			text = text.Replace("$DeviceName$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(LogEntryAlias.AliasType.DeviceName);
			num++;
		}
		if (text.IndexOf("$RecorderName$", StringComparison.OrdinalIgnoreCase) != -1)
		{
			text = text.Replace("$RecorderName$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(LogEntryAlias.AliasType.RecorderName);
			num++;
		}
		if (arrayList.Count > 0)
		{
			_replaceAliases = (LogEntryAlias.AliasType[])arrayList.ToArray(typeof(LogEntryAlias.AliasType));
		}
		_text = text;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		if (!ServerLogManager.IsLogEnabled(LogType.RuleLog))
		{
			return;
		}
		SourceType sourceType = _server.ConfigurationLookup.FindEventSourceTypeFromEventTypeID(triggerEvent.EventTypeId);
		Subtype subtype = _server.ConfigurationLookup.FindEventSourceSubtypeFromEventTypeID(triggerEvent.EventTypeId);
		string sourceName = _server.ConfigurationLookup.FindSourceDisplayName(triggerEvent.SourceId, sourceType);
		string text = string.Empty;
		try
		{
			text = ConfigurationDataManager.FindRuleNameByRuleId(_ruleID);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		Guid generatorId = Guid.Empty;
		EventGeneratorType eventGeneratorType = EventGeneratorType.Unknown;
		string generatorName = string.Empty;
		if (triggerEvent.Definition != null)
		{
			if (!string.IsNullOrEmpty(triggerEvent.Definition.GeneratorID))
			{
				try
				{
					generatorId = new Guid(triggerEvent.Definition.GeneratorID);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
			if (!string.IsNullOrEmpty(triggerEvent.Definition.GeneratorType))
			{
				try
				{
					eventGeneratorType = (EventGeneratorType)Enum.Parse(typeof(EventGeneratorType), triggerEvent.Definition.GeneratorType);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
			generatorName = triggerEvent.Definition.GeneratorName;
		}
		string text2 = _server.ConfigurationLookup.FindEventDisplayNameFromEventTypeID(triggerEvent.EventTypeId, Resources.EventNameUnknown);
		string text3 = _text;
		if (_replaceAliases != null)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < _replaceAliases.Length; i++)
			{
				string value = "(?)";
				switch (_replaceAliases[i])
				{
				case LogEntryAlias.AliasType.RecorderName:
					if (sourceType == SourceType.Recorder || sourceType == SourceType.System)
					{
						value = _server.ConfigurationLookup.FindSourceDisplayName(triggerEvent.SourceId, SourceType.Recorder);
					}
					arrayList.Add(value);
					break;
				case LogEntryAlias.AliasType.RuleName:
					arrayList.Add(text);
					break;
				case LogEntryAlias.AliasType.TriggerTime:
					arrayList.Add(triggerEvent.Time.ToString(CultureInfo.CurrentCulture));
					break;
				case LogEntryAlias.AliasType.EventName:
					arrayList.Add(text2);
					break;
				case LogEntryAlias.AliasType.DeviceName:
					if (sourceType == SourceType.Device || (eventGeneratorType == EventGeneratorType.SystemDashboard && subtype == Subtype.Device))
					{
						string text4 = _server.ConfigurationLookup.FindDeviceDisplayName(triggerEvent.SourceId);
						if (text4.Length > 0)
						{
							value = text4;
						}
					}
					arrayList.Add(value);
					break;
				}
			}
			text3 = string.Format(CultureInfo.InvariantCulture, text3, arrayList.ToArray());
		}
		LogEntryRule logEntry = new LogEntryRule(LogEntryId.GenericRule, Environment.MachineName, triggerEvent.SourceId, sourceType, sourceName, triggerEvent.EventTypeId, text2, text, generatorId, eventGeneratorType, generatorName, text3);
		ServerLogManager.Instance.AddLogEntry(logEntry);
	}
}
