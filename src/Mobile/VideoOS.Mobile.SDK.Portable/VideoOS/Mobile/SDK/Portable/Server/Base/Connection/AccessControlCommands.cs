using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class AccessControlCommands : IAccessControlAsync, IAccessControlSync, IErrorResult
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public AccessControlCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult GetAccessControlEvents(GetAccessControlEventsParams getAccessControlEventsParams, OnResponse<GetAccessControlEventsResponse> onSuccess = null, OnResponse<GetAccessControlEventsResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessControlEvents);
		getAccessControlEventsParams.ConvertToCommand(ref command);
		return _connection.SendCommand(command, delegate(BaseCommandResponse responce)
		{
			onSuccess?.Invoke(new GetAccessControlEventsResponse(responce));
		}, delegate(BaseCommandResponse responce)
		{
			onFail?.Invoke(new GetAccessControlEventsResponse(responce));
		});
	}

	public IAsyncResult GetAccessRequestEvents(OnResponse<GetAccessRequestEventsResponse> onSuccess = null, OnResponse<GetAccessRequestEventsResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessRequestEvents);
		return _connection.SendCommand(command, delegate(BaseCommandResponse responce)
		{
			onSuccess?.Invoke(new GetAccessRequestEventsResponse(responce));
		}, delegate(BaseCommandResponse responce)
		{
			onFail?.Invoke(new GetAccessRequestEventsResponse(responce));
		});
	}

	public IAsyncResult GetAccessControlDoor(Guid doorId, OnResponse<GetAccessControlDoorsResponse> onSuccess = null, OnResponse<GetAccessControlDoorsResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessControlDoors);
		if (doorId != Guid.Empty)
		{
			command.InputParams.Add(new Parameter("AccessControlDoorId", doorId.ToString()));
		}
		return _connection.SendCommand(command, delegate(BaseCommandResponse response)
		{
			onSuccess?.Invoke(new GetAccessControlDoorsResponse(response));
		}, delegate(BaseCommandResponse response)
		{
			onFail?.Invoke(new GetAccessControlDoorsResponse(response));
		});
	}

	public IAsyncResult GetAccessControlDoors(OnResponse<GetAccessControlDoorsResponse> onSuccess = null, OnResponse<GetAccessControlDoorsResponse> onFail = null)
	{
		return GetAccessControlDoor(Guid.Empty, onSuccess, onFail);
	}

	public IAsyncResult GetAccessControlDoors(GetAccessControlDoorsParams getAccessControlDoorsParams, OnResponse<GetAccessControlDoorsResponse> onSuccess = null, OnResponse<GetAccessControlDoorsResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessControlDoors);
		getAccessControlDoorsParams.ConvertToCommand(ref command);
		return _connection.SendCommand(command, delegate(BaseCommandResponse response)
		{
			onSuccess?.Invoke(new GetAccessControlDoorsResponse(response));
		}, delegate(BaseCommandResponse response)
		{
			onFail?.Invoke(new GetAccessControlDoorsResponse(response));
		});
	}

	public IAsyncResult GetAccessControlDoorStates(Guid accessControlSystemId, Guid doorId, OnResponse<GetAccessControlDoorStatesResponse> onSuccess = null, OnResponse<GetAccessControlDoorStatesResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessControlDoorStates);
		command.InputParams.Add(new Parameter("AccessControlSystemId", accessControlSystemId.ToString()));
		command.InputParams.Add(new Parameter("AccessControlDoorId", doorId.ToString()));
		return _connection.SendCommand(command, delegate(BaseCommandResponse response)
		{
			onSuccess?.Invoke(new GetAccessControlDoorStatesResponse(response));
		}, delegate(BaseCommandResponse response)
		{
			onFail?.Invoke(new GetAccessControlDoorStatesResponse(response));
		});
	}

	public IAsyncResult GetAccessRequestEventTypes(List<Guid> accessControlSystemIds, OnResponse<GetAccessRequestEventTypesResponse> onSuccess = null, OnResponse<GetAccessRequestEventTypesResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessRequestEventTypes);
		foreach (Guid accessControlSystemId in accessControlSystemIds)
		{
			command.InputParams.Add(new Parameter("AccessControlSystemId", accessControlSystemId.ToString()));
		}
		return _connection.SendCommand(command, delegate(BaseCommandResponse response)
		{
			onSuccess?.Invoke(new GetAccessRequestEventTypesResponse(response));
		}, delegate(BaseCommandResponse response)
		{
			onFail?.Invoke(new GetAccessRequestEventTypesResponse(response));
		});
	}

	public IAsyncResult GetAccessControlSystems(OnResponse<GetAccessControlSystemsResponse> onSuccess = null, OnResponse<GetAccessControlSystemsResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessControlSystems);
		return _connection.SendCommand(command, delegate(BaseCommandResponse response)
		{
			onSuccess?.Invoke(new GetAccessControlSystemsResponse(response));
		}, delegate(BaseCommandResponse response)
		{
			onFail?.Invoke(new GetAccessControlSystemsResponse(response));
		});
	}

	public IAsyncResult GetAccessControlDoorCategories(OnResponse<GetAccessControlDoorCategoriesResponse> onSuccess = null, OnResponse<GetAccessControlDoorCategoriesResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessControlDoorCategories);
		return _connection.SendCommand(command, delegate(BaseCommandResponse response)
		{
			onSuccess?.Invoke(new GetAccessControlDoorCategoriesResponse(response));
		}, delegate(BaseCommandResponse response)
		{
			onFail?.Invoke(new GetAccessControlDoorCategoriesResponse(response));
		});
	}

	public IAsyncResult GetAccessControlDoorTypes(OnResponse<GetAccessControlDoorTypesResponse> onSuccess = null, OnResponse<GetAccessControlDoorTypesResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessControlDoorTypes);
		return _connection.SendCommand(command, delegate(BaseCommandResponse response)
		{
			onSuccess?.Invoke(new GetAccessControlDoorTypesResponse(response));
		}, delegate(BaseCommandResponse response)
		{
			onFail?.Invoke(new GetAccessControlDoorTypesResponse(response));
		});
	}

	public IAsyncResult GetAccessControlEventCategories(OnResponse<GetAccessControlEventCategoriesResponse> onSuccess = null, OnResponse<GetAccessControlEventCategoriesResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAccessControlEventCategories);
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetAccessControlEventCategoriesResponse(response));
	}

	public IAsyncResult ExecuteAccessControlCommand(ExecuteAccessControlCommandParams executeAccessControlCommandParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.ExecuteAccessControlCommand);
		executeAccessControlCommandParams.ConvertToCommand(ref command);
		return _connection.SendCommand(command, onSuccess, onFail);
	}

	public GetAccessControlEventsResponse GetAccessControlEvents(GetAccessControlEventsParams getAccessControlEventsParams, TimeSpan timeout)
	{
		IAsyncResult accessControlEvents = GetAccessControlEvents(getAccessControlEventsParams);
		return new GetAccessControlEventsResponse(_connection.ProcessResult(accessControlEvents, timeout));
	}

	public GetAccessRequestEventsResponse GetAccessRequestEvents(TimeSpan timeout)
	{
		IAsyncResult accessRequestEvents = GetAccessRequestEvents();
		return new GetAccessRequestEventsResponse(_connection.ProcessResult(accessRequestEvents, timeout));
	}

	public GetAccessControlDoorsResponse GetAccessControlDoor(Guid doorId, TimeSpan timeout)
	{
		IAsyncResult accessControlDoor = GetAccessControlDoor(doorId);
		return new GetAccessControlDoorsResponse(_connection.ProcessResult(accessControlDoor, timeout));
	}

	public GetAccessControlDoorsResponse GetAccessControlDoors(TimeSpan timeout)
	{
		return GetAccessControlDoor(Guid.Empty, timeout);
	}

	public GetAccessControlDoorsResponse GetAccessControlDoors(GetAccessControlDoorsParams getAccessControlDoorsParams, TimeSpan timeout)
	{
		IAsyncResult accessControlDoors = GetAccessControlDoors(getAccessControlDoorsParams);
		return new GetAccessControlDoorsResponse(_connection.ProcessResult(accessControlDoors, timeout));
	}

	public GetAccessRequestEventTypesResponse GetAccessRequestEventTypes(List<Guid> accessControlSystemIds, TimeSpan timeout)
	{
		IAsyncResult accessRequestEventTypes = GetAccessRequestEventTypes(accessControlSystemIds);
		return new GetAccessRequestEventTypesResponse(_connection.ProcessResult(accessRequestEventTypes, timeout));
	}

	public GetAccessControlDoorStatesResponse GetAccessControlDoorStates(Guid accessControlSystemId, Guid doorId, TimeSpan timeout)
	{
		IAsyncResult accessControlDoorStates = GetAccessControlDoorStates(accessControlSystemId, doorId);
		return new GetAccessControlDoorStatesResponse(_connection.ProcessResult(accessControlDoorStates, timeout));
	}

	public GetAccessControlSystemsResponse GetAccessControlSystems(TimeSpan timeout)
	{
		IAsyncResult accessControlSystems = GetAccessControlSystems();
		return new GetAccessControlSystemsResponse(_connection.ProcessResult(accessControlSystems, timeout));
	}

	public GetAccessControlDoorCategoriesResponse GetAccessControlDoorCategories(TimeSpan timeout)
	{
		IAsyncResult accessControlDoorCategories = GetAccessControlDoorCategories();
		return new GetAccessControlDoorCategoriesResponse(_connection.ProcessResult(accessControlDoorCategories, timeout));
	}

	public GetAccessControlDoorTypesResponse GetAccessControlDoorTypes(TimeSpan timeout)
	{
		IAsyncResult accessControlDoorTypes = GetAccessControlDoorTypes();
		return new GetAccessControlDoorTypesResponse(_connection.ProcessResult(accessControlDoorTypes, timeout));
	}

	public GetAccessControlEventCategoriesResponse GetAccessControlEventCategories(TimeSpan timeout)
	{
		IAsyncResult accessControlEventCategories = GetAccessControlEventCategories();
		return new GetAccessControlEventCategoriesResponse(_connection.ProcessResult(accessControlEventCategories, timeout));
	}

	public BaseCommandResponse ExecuteAccessControlCommand(ExecuteAccessControlCommandParams executeAccessControlCommandParams, TimeSpan timeout)
	{
		IAsyncResult iResult = ExecuteAccessControlCommand(executeAccessControlCommandParams);
		return _connection.ProcessResult(iResult, timeout);
	}
}
