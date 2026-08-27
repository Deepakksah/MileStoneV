using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class ExecuteRuleActionsCommandHandler : ICommandHandler
{
	private Server server;

	private IServerEnvironment environment;

	public ExecuteRuleActionsCommandHandler(IServerEnvironment environment, Server server)
	{
		this.server = server;
		this.environment = environment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ExecuteRuleActionsCommand executeRuleActionsCommand))
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
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ExecuteRuleActionsCommand serverCommand;
		Hashtable hashtable = BuildRecorderCommands(executeRuleActionsCommand, out serverCommand);
		LocalProcessExecuteRuleActionsCommand(serverCommand);
		foreach (DictionaryEntry item in hashtable)
		{
			Guid guid = (Guid)item.Key;
			ExecuteRuleActionsCommand executeRuleActionsCommand2 = (ExecuteRuleActionsCommand)item.Value;
			if (guid == executeRuleActionsCommand.SourceId)
			{
				continue;
			}
			try
			{
				server.RecorderCommManager.ExecuteRuleActionsAsync(guid, executeRuleActionsCommand2.RuleActionExecutionRequests, executeRuleActionsCommand2.SenderUtcTime, callContextInfo);
			}
			catch (RecorderNotFoundException ex)
			{
				string text = "Unknown";
				if (executeRuleActionsCommand2.RuleActionExecutionRequests.Length == 1)
				{
					try
					{
						Guid ruleId = executeRuleActionsCommand2.RuleActionExecutionRequests[0].RuleId;
						text = ConfigurationDataManager.FindRuleNameByRuleId(ruleId);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch
					{
					}
				}
				ServerLogManager.Instance.AddLogEntry(LogEntryId.FailedToDistributeRuleActions, text, ex.Message);
			}
		}
		responseSink.AddResponse(new SuccessResponse());
	}

	private Hashtable BuildRecorderCommands(ExecuteRuleActionsCommand cmd, out ExecuteRuleActionsCommand serverCommand)
	{
		serverCommand = null;
		List<RuleActionExecutionRequest> list = new List<RuleActionExecutionRequest>();
		Hashtable hashtable = new Hashtable();
		RuleActionExecutionRequest[] ruleActionExecutionRequests = cmd.RuleActionExecutionRequests;
		foreach (RuleActionExecutionRequest ruleActionExecutionRequest in ruleActionExecutionRequests)
		{
			TriggerEvent triggerEvent = ruleActionExecutionRequest.TriggerEvent.Clone();
			triggerEvent.IsRemoteEvent = true;
			foreach (ActionTarget expandedTarget in ruleActionExecutionRequest.ExpandedTargets)
			{
				switch (expandedTarget.TargetType)
				{
				case ActionTargetType.Device:
					foreach (Guid targetItem in expandedTarget.TargetItems)
					{
						Guid guid;
						try
						{
							guid = ConfigurationDataManager.QueryRecorderIdByDeviceId(environment.DataComponentFactory, targetItem);
						}
						catch (DatabaseException ex)
						{
							Server.Instance.Logger.LogWarning("Could not find recorder from the device id when dispatching RuleActionExecutionRequests. DeviceId: {DeviceId}; Rule id: {RuleId}; Error: {ExceptionMessage}", targetItem, ruleActionExecutionRequest.RuleId, ex.Message);
							continue;
						}
						if (guid == Guid.Empty)
						{
							Server.Instance.Logger.LogWarning("Could not find recorder from the device id when dispatching RuleActionExecutionRequests. DeviceId: {DeviceId}; Rule id: {RuleId}", targetItem, ruleActionExecutionRequest.RuleId);
						}
						else
						{
							if (guid == cmd.SourceId)
							{
								continue;
							}
							ArrayList arrayList = hashtable[guid] as ArrayList;
							if (arrayList == null)
							{
								arrayList = (ArrayList)(hashtable[guid] = new ArrayList());
							}
							RuleActionExecutionRequest ruleActionExecutionRequest3 = null;
							foreach (RuleActionExecutionRequest item in arrayList)
							{
								if (item.ID == ruleActionExecutionRequest.ID)
								{
									ruleActionExecutionRequest3 = item;
									break;
								}
							}
							if (ruleActionExecutionRequest3 == null)
							{
								ruleActionExecutionRequest3 = new RuleActionExecutionRequest(ruleActionExecutionRequest.RuleId, ruleActionExecutionRequest.RuleActionInstanceId, triggerEvent, new ActionTargetCollection());
								ruleActionExecutionRequest3.ID = ruleActionExecutionRequest.ID;
								arrayList.Add(ruleActionExecutionRequest3);
							}
							ActionTarget actionTarget2 = null;
							foreach (ActionTarget expandedTarget2 in ruleActionExecutionRequest3.ExpandedTargets)
							{
								if (expandedTarget2.TargetType == expandedTarget.TargetType)
								{
									actionTarget2 = expandedTarget2;
									break;
								}
							}
							if (actionTarget2 == null)
							{
								actionTarget2 = new ActionTarget(expandedTarget.TargetType);
								ruleActionExecutionRequest3.ExpandedTargets.Add(actionTarget2);
							}
							actionTarget2.TargetItems.Add(targetItem);
						}
					}
					break;
				case ActionTargetType.Unspecified:
					Server.Instance.Logger.LogDebug("Targets with ActionTargetType='Unspecified' is not dispatched to other recorders by the server.");
					foreach (Guid targetItem2 in expandedTarget.TargetItems)
					{
						RuleActionExecutionRequest ruleActionExecutionRequest2 = null;
						foreach (RuleActionExecutionRequest item2 in list)
						{
							if (item2.ID == ruleActionExecutionRequest.ID)
							{
								ruleActionExecutionRequest2 = item2;
								break;
							}
						}
						if (ruleActionExecutionRequest2 == null)
						{
							ruleActionExecutionRequest2 = new RuleActionExecutionRequest(ruleActionExecutionRequest.RuleId, ruleActionExecutionRequest.RuleActionInstanceId, triggerEvent, new ActionTargetCollection());
							ruleActionExecutionRequest2.ID = ruleActionExecutionRequest.ID;
							list.Add(ruleActionExecutionRequest2);
						}
						list.Add(ruleActionExecutionRequest2);
						ActionTarget actionTarget = null;
						foreach (ActionTarget expandedTarget3 in ruleActionExecutionRequest2.ExpandedTargets)
						{
							if (expandedTarget3.TargetType == expandedTarget.TargetType)
							{
								actionTarget = expandedTarget3;
								break;
							}
						}
						if (actionTarget == null)
						{
							actionTarget = new ActionTarget(expandedTarget.TargetType);
							ruleActionExecutionRequest2.ExpandedTargets.Add(actionTarget);
						}
						actionTarget.TargetItems.Add(targetItem2);
					}
					break;
				default:
					Server.Instance.Logger.LogWarning("Unexpected ActionTargetType specified for ActionTarget received by server. Type: {TargetType}", expandedTarget.TargetType);
					break;
				}
			}
		}
		serverCommand = new ExecuteRuleActionsCommand(cmd.SourceId, list.ToArray());
		Hashtable hashtable2 = new Hashtable();
		foreach (DictionaryEntry item3 in hashtable)
		{
			Guid guid2 = (Guid)item3.Key;
			ArrayList arrayList3 = (ArrayList)item3.Value;
			if (!(guid2 == cmd.SourceId))
			{
				ExecuteRuleActionsCommand executeRuleActionsCommand = new ExecuteRuleActionsCommand(cmd.SourceId, (RuleActionExecutionRequest[])arrayList3.ToArray(typeof(RuleActionExecutionRequest)));
				executeRuleActionsCommand.SenderUtcTime = cmd.SenderUtcTime;
				hashtable2[guid2] = executeRuleActionsCommand;
			}
		}
		return hashtable2;
	}

	private void LocalProcessExecuteRuleActionsCommand(ExecuteRuleActionsCommand cmd)
	{
		TimeSpan value = CalculateTimestampCorrectionTimeSpan(cmd);
		RuleActionExecutionRequest[] ruleActionExecutionRequests = cmd.RuleActionExecutionRequests;
		foreach (RuleActionExecutionRequest ruleActionExecutionRequest in ruleActionExecutionRequests)
		{
			ruleActionExecutionRequest.TriggerEvent.IsRemoteEvent = true;
			ruleActionExecutionRequest.TriggerEvent.Time = ruleActionExecutionRequest.TriggerEvent.Time.Add(value);
			server.RuleEngine.AddRuleActionExecutionRequest(ruleActionExecutionRequest);
		}
	}

	private static TimeSpan CalculateTimestampCorrectionTimeSpan(ExecuteRuleActionsCommand cmd)
	{
		TimeSpan timeSpan = DateTime.UtcNow.Subtract(cmd.SenderUtcTime);
		TimeSpan result = TimeSpan.Zero;
		double maxExecuteRuleActionsCommandDelay = ServerConfig.Instance.RulesConfiguration.MaxExecuteRuleActionsCommandDelay;
		if (timeSpan.TotalMilliseconds < 0.0 || timeSpan.TotalMilliseconds > maxExecuteRuleActionsCommandDelay)
		{
			result = TimeSpan.FromTicks(timeSpan.Ticks);
		}
		return result;
	}
}
