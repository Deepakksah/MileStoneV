using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.Properties;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class ChangeHardwarePasswordTaskCommandHandler : ICommandHandler
{
	[Serializable]
	public class UpdatePasswordException : Exception
	{
		public UpdatePasswordException()
		{
		}

		public UpdatePasswordException(string message)
			: base(message)
		{
		}

		public UpdatePasswordException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected UpdatePasswordException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	private readonly IServerEnvironment _serverEnvironment;

	private IRecorderCommManager RecorderCommManager => _serverEnvironment.RecorderCommManager;

	private IMessageManager MessageManager => _serverEnvironment.MessageManager;

	private IDataComponentFactory DataComponentFactory => _serverEnvironment.DataComponentFactory;

	private ILogger Logger => _serverEnvironment.Server.Logger;

	private IDataComponentFactory ComponentFactory => _serverEnvironment.DataComponentFactory;

	public ChangeHardwarePasswordTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ChangeHardwarePasswordTaskCommand cmd))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		TaskResponseBase response = ProcessCommand(cmd, responseSink, callContextInfo);
		responseSink.AddResponse(response);
	}

	private TaskResponseBase ProcessCommand(ChangeHardwarePasswordTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		if (!RecorderCommManager.IsRecorderNotDisconnected(cmd.RecordingServerId))
		{
			Logger.LogError("Change hardware password task - Recorder not available. Recorder Id: {ServerId}", cmd.RecordingServerId);
			return CreateTaskFailedResponse(66010, VmoResources.VMO66010, cmd.HardwareName);
		}
		if (RecorderCommManager.IsRecorderIncompatible(cmd.RecordingServerId))
		{
			Logger.LogError("Change hardware password task - Recorder is running unsupported version. Recorder Id: {ServerId}", cmd.RecordingServerId);
			return CreateTaskFailedResponse(66011, VmoResources.VMO66011, cmd.HardwareName);
		}
		int percentComplete = 10;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
		ChangeHardwarePasswordCommand command = new ChangeHardwarePasswordCommand(cmd.HardwareId, cmd.Password, cmd.UserName);
		try
		{
			using IMessageManagerResponseCollector messageManagerResponseCollector = MessageManager.SendCommand(cmd.RecordingServerId, command, callContextInfo);
			ResponseBase nextResponse;
			try
			{
				nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(60.0));
			}
			catch (TimeoutException)
			{
				Logger.LogError("Change hardware password task - timeout while waiting for a response.");
				return CreateTaskFailedResponse(66012, VmoResources.VMO66012, cmd.HardwareName);
			}
			percentComplete = 50;
			responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
			if (nextResponse is SuccessResponse)
			{
				return HandleSuccessResponse(cmd, responseSink, callContextInfo, utcNow);
			}
			return HandleErrorResponse(cmd, responseSink, utcNow, nextResponse);
		}
		catch (RecorderNotFoundException ex2)
		{
			Logger.LogError("Recorder not found while changing hardware password: {ExceptionMessage}", ex2.Message);
			return CreateTaskFailedResponse(66010, VmoResources.VMO66010, cmd.HardwareName);
		}
		catch (NotSupportedByLegacyRecorderException ex3)
		{
			Logger.LogError("Legacy Recorder used while changing hardware password: {ExceptionMessage}", ex3.Message);
			return CreateTaskFailedResponse(66011, VmoResources.VMO66011, cmd.HardwareName);
		}
		catch (UpdatePasswordException ex4)
		{
			Logger.LogError("Failed to change the hardware password: {ExceptionMessage}", ex4.Message);
			return CreateTaskFailedResponse(66009, VmoResources.VMO66009, cmd.HardwareName);
		}
	}

	private TaskResponseBase HandleErrorResponse(ChangeHardwarePasswordTaskCommand cmd, IResponseSink responseSink, DateTime startTime, ResponseBase response)
	{
		int percentComplete = 100;
		if (response is ExceptionResponse exceptionResponse)
		{
			Logger.LogError("Change hardware password command for Hardware Id {HardwareId} failed with exception. Exception: {Exception}", cmd.HardwareId, exceptionResponse.Exception.Message);
			responseSink.AddResponse(new TaskProgressResponse(startTime, percentComplete));
			return CreateTaskFailedResponse(66004, VmoResources.VMO66004, cmd.HardwareName);
		}
		if (response is ErrorResponse errorResponse)
		{
			TaskFailedResponse failedResponse = GetFailedResponse(cmd, errorResponse);
			Logger.LogError("Change hardware password command for Hardware Id: {HardwareId} failed. Error: {ErrorMessage}", cmd.HardwareId, errorResponse.Message);
			responseSink.AddResponse(new TaskProgressResponse(startTime, percentComplete));
			return failedResponse;
		}
		Logger.LogWarning("Change hardware password command failed for unknown reason.");
		responseSink.AddResponse(new TaskProgressResponse(startTime, percentComplete));
		return CreateTaskFailedResponse(66013, VmoResources.VMO66013, cmd.HardwareName);
	}

	private TaskResponseBase HandleSuccessResponse(ChangeHardwarePasswordTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo, DateTime startTime)
	{
		int percentComplete = 75;
		IEnumerable<HardwareConfigData> enumerable = GetAllDeviceInstances(cmd.HardwareId).ToList();
		if (enumerable != null && enumerable.Count() > 0)
		{
			responseSink.AddResponse(new TaskProgressResponse(startTime, percentComplete));
			string userDomainAccountName = callContextInfo.UserDomainAccountName;
			string locationIPAddress = callContextInfo.LocationIPAddress;
			string username = enumerable.FirstOrDefault((HardwareConfigData x) => x.HardwareId == cmd.HardwareId).Username;
			string newUserName = (string.IsNullOrEmpty(cmd.UserName) ? username : cmd.UserName);
			UpdateConfigurationManagerCommandHandler.ConfigurationChangedEventCollector configurationChangedEventCollector = new UpdateConfigurationManagerCommandHandler.ConfigurationChangedEventCollector(_serverEnvironment.Server.HomeSiteId, _serverEnvironment.Server.EVSEventConsumer);
			foreach (HardwareConfigData item in enumerable)
			{
				if (item.HardwareId == cmd.HardwareId || item.Username == username)
				{
					UpdateCredentialsInDatabase(item.HardwareId, item.Username, newUserName, cmd.Password, callContextInfo);
				}
				configurationChangedEventCollector.AddEvent(cmd.HardwareId.ToString(), cmd.HardwareName, ChangeAction.Updated, "Hardware");
			}
			configurationChangedEventCollector.FireConfigChangedEvent();
			Logger.LogInformation("Change hardware password Succeeded. HardwareId: {HardwareId}", cmd.HardwareId);
			percentComplete = 100;
			responseSink.AddResponse(new TaskProgressResponse(startTime, percentComplete));
			ChangeHardwarePasswordTaskResult changeHardwarePasswordTaskResult = new ChangeHardwarePasswordTaskResult
			{
				HardwareId = cmd.HardwareId,
				HardwareName = cmd.HardwareName,
				RecordingServerId = cmd.RecordingServerId
			};
			return new TaskSuccessResponse(changeHardwarePasswordTaskResult.ToXml());
		}
		return CreateTaskFailedResponse(66009, VmoResources.VMO66009, cmd.HardwareName);
	}

	private static TaskFailedResponse GetFailedResponse(ChangeHardwarePasswordTaskCommand cmd, ErrorResponse errorResponse)
	{
		return errorResponse.ErrorCode switch
		{
			20 => CreateTaskFailedResponse(66000, VmoResources.VMO66000, cmd.HardwareName), 
			19 => CreateTaskFailedResponse(66001, VmoResources.VMO66001, cmd.HardwareName), 
			24 => CreateTaskFailedResponse(66002, VmoResources.VMO66002, cmd.HardwareName), 
			0 => CreateTaskFailedResponse(66003, VmoResources.VMO66003, cmd.HardwareName), 
			9 => CreateTaskFailedResponse(66004, VmoResources.VMO66004, cmd.HardwareName), 
			3 => CreateTaskFailedResponse(66005, VmoResources.VMO66005, cmd.HardwareName), 
			4 => CreateTaskFailedResponse(66008, VmoResources.VMO66008, cmd.HardwareName), 
			5 => CreateTaskFailedResponse(66007, VmoResources.VMO66007, cmd.HardwareName), 
			6 => CreateTaskFailedResponse(66006, VmoResources.VMO66006, cmd.HardwareName), 
			22 => CreateTaskFailedResponse(66009, VmoResources.VMO66009, cmd.HardwareName), 
			23 => CreateTaskFailedResponse(66014, VmoResources.VMO66014, cmd.HardwareName), 
			7 => CreateTaskFailedResponse(66016, VmoResources.VMO66016, cmd.HardwareName), 
			25 => CreateTaskFailedResponse(66017, VmoResources.VMO66017, cmd.HardwareName), 
			15 => CreateTaskFailedResponse(66018, VmoResources.VMO66018, cmd.HardwareName), 
			16 => CreateTaskFailedResponse(66019, VmoResources.VMO66019, cmd.HardwareName), 
			13 => CreateTaskFailedResponse(66020, VmoResources.VMO66020, cmd.HardwareName), 
			_ => CreateTaskFailedResponse(66013, VmoResources.VMO66013, cmd.HardwareName), 
		};
	}

	private void UpdateCredentialsInDatabase(Guid hardwareId, string currentUserName, string newUserName, string newPassword, ICallContextInfo callContextInfo)
	{
		using IHardwareConfigDataComponent hardwareConfigDataComponent = ComponentFactory.CreateHardwareConfigDataComponent();
		try
		{
			string text = hardwareConfigDataComponent.QueryPasswordById(hardwareId);
			if (newPassword != text || newUserName != currentUserName)
			{
				DateTime utcNow = DateTime.UtcNow;
				hardwareConfigDataComponent.UpdateCredentials(hardwareId, newUserName, newPassword, utcNow, callContextInfo);
			}
		}
		catch (DatabaseException ex)
		{
			throw new UpdatePasswordException(string.Format(CultureInfo.CurrentCulture, "Failed to change the hardware password in database due to an SQL error: {0}", ex.Message), ex);
		}
		catch (Exception ex2)
		{
			throw new UpdatePasswordException(string.Format(CultureInfo.CurrentCulture, "Failed to change the hardware password in database due to an unknown error: {0}", ex2.Message), ex2);
		}
	}

	private static TaskFailedResponse CreateTaskFailedResponse(int errorCode, string errorText, params string[] arguments)
	{
		TaskError taskError = new TaskError
		{
			ErrorCode = errorCode,
			ErrorText = errorText
		};
		for (int i = 0; i < arguments.Length; i++)
		{
			taskError.Arguments.Add(i, arguments[i]);
		}
		return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
	}

	private HardwareConfigData[] GetAllDeviceInstances(Guid hardwareId)
	{
		HardwareConfigData[] result = null;
		using (IHardwareConfigDataComponent hardwareConfigDataComponent = DataComponentFactory.CreateHardwareConfigDataComponent())
		{
			try
			{
				result = hardwareConfigDataComponent.QueryHardwareWithSameMacAddressById(hardwareId);
			}
			catch (DatabaseException ex)
			{
				Logger.LogWarning("Change hardware password task - failed to query for other instances of device. Error: {ExceptionMessage}", ex.Message);
			}
		}
		return result;
	}
}
