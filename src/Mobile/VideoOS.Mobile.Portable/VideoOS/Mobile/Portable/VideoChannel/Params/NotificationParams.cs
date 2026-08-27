using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class NotificationParams
{
	private Guid _deviceId;

	private NotificationParamsHelper.CloudType _cloud;

	private string _deviceToken;

	private string _optionalParamX;

	private string _optionalParamY;

	private string _optionalParamZ;

	private string _message;

	private string _validationSLC;

	private Guid _serverId;

	private Guid _alarmId;

	private string _deviceName;

	private uint _settingsFlag;

	private uint _paramMask;

	public Guid DeviceId
	{
		get
		{
			return _deviceId;
		}
		set
		{
			_deviceId = value;
			_paramMask |= 1u;
		}
	}

	public NotificationParamsHelper.CloudType Cloud
	{
		get
		{
			return _cloud;
		}
		set
		{
			_cloud = value;
			_paramMask |= 2u;
		}
	}

	public string DeviceToken
	{
		get
		{
			return _deviceToken;
		}
		set
		{
			_deviceToken = value;
			_paramMask |= 4u;
		}
	}

	public string OptionalParamX
	{
		get
		{
			return _optionalParamX;
		}
		set
		{
			_optionalParamX = value;
			_paramMask |= 128u;
		}
	}

	public string OptionalParamY
	{
		get
		{
			return _optionalParamY;
		}
		set
		{
			_optionalParamY = value;
			_paramMask |= 256u;
		}
	}

	public string OptionalParamZ
	{
		get
		{
			return _optionalParamZ;
		}
		set
		{
			_optionalParamZ = value;
			_paramMask |= 512u;
		}
	}

	public string Message
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value;
			_paramMask |= 8u;
		}
	}

	public Guid ServerId
	{
		get
		{
			return _serverId;
		}
		set
		{
			_serverId = value;
			_paramMask |= 32u;
		}
	}

	public Guid AlarmId
	{
		get
		{
			return _alarmId;
		}
		set
		{
			_alarmId = value;
			_paramMask |= 64u;
		}
	}

	public string ValidationSLC
	{
		get
		{
			return _validationSLC;
		}
		set
		{
			_validationSLC = value;
			_paramMask |= 16u;
		}
	}

	public string DeviceName
	{
		get
		{
			return _deviceName;
		}
		set
		{
			_deviceName = value;
			_paramMask |= 1024u;
		}
	}

	public uint SettingsFlag
	{
		get
		{
			return _settingsFlag;
		}
		set
		{
			_settingsFlag = value;
			_paramMask |= 2048u;
		}
	}

	public NotificationParams()
	{
		InternalInit();
	}

	public NotificationParams(Command tCommand, uint unMaskToCheck = 3969u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
	}

	public NotificationParams InitFromCommand(Command tCommand, uint unMaskToCheck = 3969u)
	{
		try
		{
			ParamsHelper.Convertion<Guid> convFunc = (string x) => (!(x != string.Empty)) ? Guid.Empty : new Guid(x);
			ParamsHelper.Convertion<uint> convFunc2 = (string x) => uint.TryParse(x, out var result) ? result : 0u;
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "DeviceId", ref _deviceId, NotificationParamsHelper.ParamMask.DeviceId, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Cloud", ref _cloud, NotificationParamsHelper.ParamMask.Cloud, EnumsHelper.EnumConverter<NotificationParamsHelper.CloudType>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "X", ref _deviceToken, NotificationParamsHelper.ParamMask.DeviceToken, (string x) => x, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "PX", ref _optionalParamX, NotificationParamsHelper.ParamMask.OptionalParamX, (string x) => x, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "PY", ref _optionalParamY, NotificationParamsHelper.ParamMask.OptionalParamY, (string x) => x, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "PZ", ref _optionalParamZ, NotificationParamsHelper.ParamMask.OptionalParamZ, (string x) => x, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "loc-key", ref _message, NotificationParamsHelper.ParamMask.NotificationMessage, (string x) => x, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "ServerId", ref _serverId, NotificationParamsHelper.ParamMask.ServerId, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "AlarmId", ref _alarmId, NotificationParamsHelper.ParamMask.AlarmId, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "S", ref _validationSLC, NotificationParamsHelper.ParamMask.ValidationSLC, (string x) => x, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "DeviceName", ref _deviceName, NotificationParamsHelper.ParamMask.DeviceName, (string x) => x, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Settings", ref _settingsFlag, NotificationParamsHelper.ParamMask.Settings, convFunc2, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "NotificationParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, NotificationParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, NotificationParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public NotificationParams ConvertToCommand(ref Command tCommand, Command.ParamsType paramsType = Command.ParamsType.Input)
	{
		try
		{
			ParamsHelper.ConvToString<NotificationParamsHelper.CloudType> converter = (NotificationParamsHelper.CloudType x) => EnumsHelper.GetDescription(x);
			tCommand.ConvertToDictionary(paramsType);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(paramsType);
			ConvertParameterTo(dictionary, "DeviceId", _deviceId, NotificationParamsHelper.ParamMask.DeviceId);
			ConvertParameterTo(dictionary, "Cloud", _cloud, NotificationParamsHelper.ParamMask.Cloud, converter);
			ConvertParameterTo(dictionary, "X", _deviceToken, NotificationParamsHelper.ParamMask.DeviceToken);
			ConvertParameterTo(dictionary, "PX", _optionalParamX, NotificationParamsHelper.ParamMask.OptionalParamX);
			ConvertParameterTo(dictionary, "PY", _optionalParamY, NotificationParamsHelper.ParamMask.OptionalParamY);
			ConvertParameterTo(dictionary, "PZ", _optionalParamZ, NotificationParamsHelper.ParamMask.OptionalParamZ);
			ConvertParameterTo(dictionary, "loc-key", _message, NotificationParamsHelper.ParamMask.NotificationMessage);
			ConvertParameterTo(dictionary, "ServerId", _serverId, NotificationParamsHelper.ParamMask.ServerId);
			ConvertParameterTo(dictionary, "AlarmId", _alarmId, NotificationParamsHelper.ParamMask.AlarmId);
			ConvertParameterTo(dictionary, "S", _validationSLC, NotificationParamsHelper.ParamMask.ValidationSLC);
			ConvertParameterTo(dictionary, "DeviceName", _deviceName, NotificationParamsHelper.ParamMask.DeviceName);
			ConvertParameterTo(dictionary, "Settings", _settingsFlag, NotificationParamsHelper.ParamMask.Settings);
			tCommand.ConvertFromDictionary(paramsType);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "NotificationParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckNotificationParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}

	public bool HasParameter(NotificationParamsHelper.ParamMask maskToCheck)
	{
		return CheckNotificationParams((uint)maskToCheck) == ErrorCodes.Ok;
	}
}
