using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class GetServerStatusParams
{
	private bool _serverAccessibility;

	private Guid _userId;

	private uint _paramMask;

	public uint ParamMask => _paramMask;

	public bool ServerAccessibility
	{
		get
		{
			return _serverAccessibility;
		}
		set
		{
			_serverAccessibility = value;
			_paramMask |= 1u;
		}
	}

	public Guid UserId
	{
		get
		{
			return _userId;
		}
		set
		{
			_userId = value;
			_paramMask |= 2u;
		}
	}

	public GetServerStatusParams()
	{
		InternalInit();
	}

	public GetServerStatusParams(Command tCommand, GetServerStatusParamsHelper.ParamMask paramMask = GetServerStatusParamsHelper.ParamMask.All)
	{
		InternalInit();
		InitFromCommand(tCommand, paramMask);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_serverAccessibility = false;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, GetServerStatusParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public GetServerStatusParams InitFromCommand(Command tCommand, GetServerStatusParamsHelper.ParamMask maskToCheck, uint unMaskToCheck = 3u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "ServerAccessibility", ref _serverAccessibility, GetServerStatusParamsHelper.ParamMask.ServerAccessibility, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "UserId", ref _userId, GetServerStatusParamsHelper.ParamMask.UserId, ParamsHelper.GuidConvertion, unMaskToCheck);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GetServerStatusParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, GetServerStatusParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public GetServerStatusParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "ServerAccessibility", _serverAccessibility, GetServerStatusParamsHelper.ParamMask.ServerAccessibility, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "UserId", _userId, GetServerStatusParamsHelper.ParamMask.UserId);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GetServerStatusParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}

	public bool CheckForMask(GetServerStatusParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}
}
