using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;

public class ExecuteAccessControlCommandParams
{
	private Guid _accessControlSystemId;

	private Guid _accessControlOperationalInstanceId;

	private Guid _accessControlCommandTypeId;

	private Guid _eventId;

	private Guid _triggerSourceObjectId;

	private Guid _triggerSourceKindId;

	private uint _executeAccessControlCommandParamMask;

	public Guid AccessControlSystemId
	{
		get
		{
			return _accessControlSystemId;
		}
		set
		{
			_accessControlSystemId = value;
			_executeAccessControlCommandParamMask |= 1u;
		}
	}

	public Guid AccessControlOperationalInstanceId
	{
		get
		{
			return _accessControlOperationalInstanceId;
		}
		set
		{
			_accessControlOperationalInstanceId = value;
			_executeAccessControlCommandParamMask |= 2u;
		}
	}

	public Guid AccessControlCommandTypeId
	{
		get
		{
			return _accessControlCommandTypeId;
		}
		set
		{
			_accessControlCommandTypeId = value;
			_executeAccessControlCommandParamMask |= 4u;
		}
	}

	public Guid EventId
	{
		get
		{
			return _eventId;
		}
		set
		{
			_eventId = value;
			_executeAccessControlCommandParamMask |= 8u;
		}
	}

	public Guid TriggerSourceObjectId
	{
		get
		{
			return _triggerSourceObjectId;
		}
		set
		{
			_triggerSourceObjectId = value;
			_executeAccessControlCommandParamMask |= 16u;
		}
	}

	public Guid TriggerSourceKindId
	{
		get
		{
			return _triggerSourceKindId;
		}
		set
		{
			_triggerSourceKindId = value;
			_executeAccessControlCommandParamMask |= 32u;
		}
	}

	public ExecuteAccessControlCommandParams()
	{
		InternalInit();
	}

	public ExecuteAccessControlCommandParams(Command command, uint maskToCheck = 63u)
	{
		InternalInit();
		InitFromCommand(command, maskToCheck);
	}

	public ExecuteAccessControlCommandParams InitFromCommand(Command command, uint maskToCheck = 63u)
	{
		try
		{
			ParamsHelper.Convertion<Guid> convFunc = (string x) => (!Guid.TryParse(x, out var result)) ? Guid.Empty : result;
			_executeAccessControlCommandParamMask = 0u;
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "AccessControlSystemId", ref _accessControlSystemId, ExecuteAccessControlCommandParamMask.AccessControlSystemId, convFunc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlOperationalInstanceId", ref _accessControlOperationalInstanceId, ExecuteAccessControlCommandParamMask.AccessControlOperationalInstanceId, convFunc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlCommandTypeId", ref _accessControlCommandTypeId, ExecuteAccessControlCommandParamMask.AccessControlCommandTypeId, convFunc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlEventId", ref _eventId, ExecuteAccessControlCommandParamMask.EventId, convFunc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlTriggerSourceObjectId", ref _triggerSourceObjectId, ExecuteAccessControlCommandParamMask.TriggerSourceObjectId, convFunc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlKindId", ref _triggerSourceKindId, ExecuteAccessControlCommandParamMask.TriggerSourceKindId, convFunc, maskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ExecuteAccessControlCommandParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	public ExecuteAccessControlCommandParams ConvertToCommand(ref Command command)
	{
		try
		{
			command.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "AccessControlSystemId", _accessControlSystemId, ExecuteAccessControlCommandParamMask.AccessControlSystemId);
			ConvertParameterTo(dictionary, "AccessControlOperationalInstanceId", _accessControlOperationalInstanceId, ExecuteAccessControlCommandParamMask.AccessControlOperationalInstanceId);
			ConvertParameterTo(dictionary, "AccessControlCommandTypeId", _accessControlCommandTypeId, ExecuteAccessControlCommandParamMask.AccessControlCommandTypeId);
			ConvertParameterTo(dictionary, "AccessControlEventId", _eventId, ExecuteAccessControlCommandParamMask.EventId);
			ConvertParameterTo(dictionary, "AccessControlTriggerSourceObjectId", _triggerSourceObjectId, ExecuteAccessControlCommandParamMask.TriggerSourceObjectId);
			ConvertParameterTo(dictionary, "AccessControlKindId", _triggerSourceKindId, ExecuteAccessControlCommandParamMask.TriggerSourceKindId);
			command.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ExecuteAccessControlCommandParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckExecuteAccessControlCommandParams(uint maskToCheck)
	{
		if (maskToCheck == 63)
		{
			ErrorCodes errorCodes = ParamsHelper.CheckParams(7u, _executeAccessControlCommandParamMask);
			if (errorCodes == ErrorCodes.Ok)
			{
				return errorCodes;
			}
			errorCodes = ParamsHelper.CheckParams(56u, _executeAccessControlCommandParamMask);
			if (errorCodes == ErrorCodes.Ok)
			{
				return errorCodes;
			}
		}
		return ParamsHelper.CheckParams(maskToCheck, _executeAccessControlCommandParamMask);
	}

	private void InternalInit()
	{
		_executeAccessControlCommandParamMask = 0u;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, ExecuteAccessControlCommandParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _executeAccessControlCommandParamMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, ExecuteAccessControlCommandParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _executeAccessControlCommandParamMask, converter);
	}
}
