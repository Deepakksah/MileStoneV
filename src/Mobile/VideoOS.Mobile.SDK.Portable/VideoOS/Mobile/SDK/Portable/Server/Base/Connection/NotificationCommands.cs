using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class NotificationCommands : INotificationsAsync, INotificationsSync, IErrorResult
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public NotificationCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult RegisterForNotifications(NotificationParams notificationParams, OnResponse<RegisterForNotificationsResponse> onSuccess = null, OnResponse<RegisterForNotificationsResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.RegisterForNotifications);
		Connection connection = _connection as Connection;
		if (notificationParams.CheckNotificationParams(4u) == ErrorCodes.Ok)
		{
			notificationParams.DeviceToken = connection.GetEncryptedText(notificationParams.DeviceToken);
		}
		if (notificationParams.CheckNotificationParams(128u) == ErrorCodes.Ok)
		{
			notificationParams.OptionalParamX = connection.GetEncryptedText(notificationParams.OptionalParamX);
		}
		if (notificationParams.CheckNotificationParams(256u) == ErrorCodes.Ok)
		{
			notificationParams.OptionalParamY = connection.GetEncryptedText(notificationParams.OptionalParamY);
		}
		if (notificationParams.CheckNotificationParams(512u) == ErrorCodes.Ok)
		{
			notificationParams.OptionalParamZ = connection.GetEncryptedText(notificationParams.OptionalParamZ);
		}
		if (notificationParams.CheckNotificationParams(1024u) == ErrorCodes.Ok)
		{
			notificationParams.DeviceName = connection.GetEncryptedText(notificationParams.DeviceName);
		}
		notificationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse responce) => new RegisterForNotificationsResponse(responce));
	}

	public IAsyncResult SendNotification(NotificationParams notificationParams, OnResponse<SendNotificationResponse> onSuccess = null, OnResponse<SendNotificationResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.SendNotification);
		notificationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse responce) => new SendNotificationResponse(responce));
	}

	public IAsyncResult SendHeartbeat(HeartbeatContent content, OnResponse<SendHeartbeatResponse> onSuccess = null, OnResponse<SendHeartbeatResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.SendHeartbeat);
		command.Items = new List<SerializableItem> { SerializableHeartbeat.GetItem(content) };
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse responce) => new SendHeartbeatResponse(responce));
	}

	public RegisterForNotificationsResponse RegisterForNotifications(NotificationParams notificationParams, TimeSpan timeout)
	{
		IAsyncResult iResult = RegisterForNotifications(notificationParams);
		return new RegisterForNotificationsResponse(_connection.ProcessResult(iResult, timeout));
	}

	public SendNotificationResponse SendNotification(NotificationParams notificationParams, TimeSpan timeout)
	{
		IAsyncResult iResult = SendNotification(notificationParams);
		return new SendNotificationResponse(_connection.ProcessResult(iResult, timeout));
	}

	public SendHeartbeatResponse SendHeartbeat(HeartbeatContent content, TimeSpan timeout)
	{
		IAsyncResult iResult = SendHeartbeat(content);
		return new SendHeartbeatResponse(_connection.ProcessResult(iResult, timeout));
	}
}
