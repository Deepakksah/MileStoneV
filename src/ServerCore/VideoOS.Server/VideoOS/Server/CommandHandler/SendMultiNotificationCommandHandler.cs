using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.CommandHandler;

public sealed class SendMultiNotificationCommandHandler : ICommandHandler
{
	private Server _server;

	public SendMultiNotificationCommandHandler(Server server)
	{
		_server = server;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is SendMultiNotificationCommand cmd))
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
		ProcessCommand(cmd, callContextInfo, responseSink);
	}

	private void ProcessCommand(SendMultiNotificationCommand cmd, ICallContextInfo callContextInfo, IResponseSink responseSink)
	{
		try
		{
			HashSet<Guid> hashSet = new HashSet<Guid>(cmd.SourceIds);
			Guid[] sourceGroupIds = cmd.SourceGroupIds;
			foreach (Guid groupId in sourceGroupIds)
			{
				Guid[] array = _server.RuleEngine.GroupConfiguration.FindAllDescendantMembersByGroup(groupId);
				Guid[] array2 = array;
				foreach (Guid item in array2)
				{
					hashSet.Add(item);
				}
			}
			ConfigurationDataSet configurationDataSet = _server.Data.LoadDevicesByDeviceIds(hashSet.ToArray());
			List<Guid> list = new List<Guid>();
			List<Guid> list2 = new List<Guid>();
			foreach (ConfigurationDataSet.DevicesRow row in configurationDataSet.Devices.Rows)
			{
				Guid item2 = new Guid(row.IDDevice);
				if (RegisteredDeviceType.IsCamera(row.DeviceType))
				{
					list.Add(item2);
				}
				else
				{
					list2.Add(item2);
				}
			}
			if (list.Count == 0)
			{
				list.Add(Guid.Empty);
			}
			foreach (Guid item3 in list)
			{
				SendNotificationCommand command = new SendNotificationCommand(cmd.IDNotificationProfile, cmd.RuleId, cmd.EventTypeId, cmd.TriggerId, cmd.RecorderId, item3, list2.ToArray(), cmd.Time);
				CommandProcessor.ProcessAsync(command, callContextInfo, ProcessCommandCallback);
			}
			responseSink.AddResponse(new SuccessResponse());
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogWarning("Failed to execute SendMultiNotification command. Error: {ExceptionMessage}", ex2.Message);
			ServerLogManager.Instance.AddLogEntry(LogEntryId.SendNotificationError, ex2.ToString());
			responseSink.AddResponse(new FailResponse());
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	private void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		Server.Instance.Logger.LogDebug("Command processed. Command={Command}; Response={Response}", command.GetType(), response.GetType());
	}
}
