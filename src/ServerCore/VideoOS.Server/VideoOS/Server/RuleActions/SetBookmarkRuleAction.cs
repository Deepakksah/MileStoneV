using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents;
using VideoOS.Server.Properties;

namespace VideoOS.Server.RuleActions;

public sealed class SetBookmarkRuleAction : RuleActionBase
{
	private struct TextReplaceInfo
	{
		public string Text;

		public BookmarkEntryAlias.AliasType[] ReplaceAliases;
	}

	private Server server;

	private Guid ruleID;

	private TextReplaceInfo headerInfo;

	private TextReplaceInfo descriptionInfo;

	private TimeSpan timeBefore;

	private TimeSpan timeAfter;

	private const string ITEMNOTFOUND = "(?)";

	public override bool HandlesAllActionTargets => true;

	public SetBookmarkRuleAction(IRuleEngine ruleEngine, Server server, Guid ruleId, string header, string description, TimeSpan timeBefore, TimeSpan timeAfter, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		ruleID = ruleId;
		this.server = server;
		headerInfo = PrepareTextReplace(header);
		descriptionInfo = PrepareTextReplace(description);
		this.timeBefore = timeBefore;
		this.timeAfter = timeAfter;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (expandedActionTargets == null)
		{
			throw new ArgumentNullException("expandedActionTargets");
		}
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		List<Guid> list = new List<Guid>();
		ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
		foreach (ActionTarget expandedActionTarget in expandedActionTargets)
		{
			if (expandedActionTarget.TargetType != ActionTargetType.Device)
			{
				continue;
			}
			foreach (Guid targetItem in expandedActionTarget.TargetItems)
			{
				list.Add(targetItem);
			}
		}
		if (list.Count > 0)
		{
			string header = DoTextReplace(headerInfo, triggerEvent);
			string description = DoTextReplace(descriptionInfo, triggerEvent);
			AddBookmarkCommand command = new AddBookmarkCommand(list.ToArray(), header, description, triggerEvent.Time, timeBefore, timeAfter);
			CommandProcessor.ProcessAsync(command, state, new NullResponseSink());
		}
	}

	private static TextReplaceInfo PrepareTextReplace(string text)
	{
		ArrayList arrayList = new ArrayList();
		int num = 0;
		if (text.IndexOf("$RuleName$", StringComparison.Ordinal) != -1)
		{
			text = text.Replace("$RuleName$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(BookmarkEntryAlias.AliasType.RuleName);
			num++;
		}
		if (text.IndexOf("$TriggerTime$", StringComparison.Ordinal) != -1)
		{
			text = text.Replace("$TriggerTime$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(BookmarkEntryAlias.AliasType.TriggerTime);
			num++;
		}
		if (text.IndexOf("$EventName$", StringComparison.Ordinal) != -1)
		{
			text = text.Replace("$EventName$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(BookmarkEntryAlias.AliasType.EventName);
			num++;
		}
		if (text.IndexOf("$DeviceName$", StringComparison.Ordinal) != -1)
		{
			text = text.Replace("$DeviceName$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(BookmarkEntryAlias.AliasType.DeviceName);
			num++;
		}
		if (text.IndexOf("$RecorderName$", StringComparison.Ordinal) != -1)
		{
			text = text.Replace("$RecorderName$", "{" + num.ToString(CultureInfo.InvariantCulture) + "}");
			arrayList.Add(BookmarkEntryAlias.AliasType.RecorderName);
			num++;
		}
		TextReplaceInfo result = new TextReplaceInfo
		{
			Text = text,
			ReplaceAliases = null
		};
		if (arrayList.Count > 0)
		{
			result.ReplaceAliases = (BookmarkEntryAlias.AliasType[])arrayList.ToArray(typeof(BookmarkEntryAlias.AliasType));
		}
		return result;
	}

	private string DoTextReplace(TextReplaceInfo textReplaceInfo, TriggerEvent triggerEvent)
	{
		string text = textReplaceInfo.Text;
		if (textReplaceInfo.ReplaceAliases != null)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < textReplaceInfo.ReplaceAliases.Length; i++)
			{
				string value = "(?)";
				switch (textReplaceInfo.ReplaceAliases[i])
				{
				case BookmarkEntryAlias.AliasType.RecorderName:
					arrayList.Add(Environment.MachineName);
					break;
				case BookmarkEntryAlias.AliasType.RuleName:
				{
					string value2 = "(?)";
					try
					{
						value2 = ConfigurationDataManager.FindRuleNameByRuleId(ruleID);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch
					{
					}
					arrayList.Add(value2);
					break;
				}
				case BookmarkEntryAlias.AliasType.TriggerTime:
					arrayList.Add(triggerEvent.Time.ToString(CultureInfo.InvariantCulture));
					break;
				case BookmarkEntryAlias.AliasType.EventName:
					arrayList.Add(server.ConfigurationLookup.FindEventDisplayNameFromEventTypeID(triggerEvent.EventTypeId, Resources.EventNameUnknown));
					break;
				case BookmarkEntryAlias.AliasType.DeviceName:
				{
					SourceType sourceType = server.ConfigurationLookup.FindEventSourceTypeFromEventTypeID(triggerEvent.EventTypeId);
					if (sourceType == SourceType.Device)
					{
						string text2 = server.ConfigurationLookup.FindDeviceDisplayName(triggerEvent.SourceId);
						if (text2.Length > 0)
						{
							value = text2;
						}
					}
					arrayList.Add(value);
					break;
				}
				}
			}
			text = string.Format(CultureInfo.InvariantCulture, text, arrayList.ToArray());
		}
		return text;
	}
}
