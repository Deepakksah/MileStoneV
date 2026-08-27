using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class PermittedUsersParams
{
	private Guid _sourceId;

	private uint _paramMask;

	public Guid SourceId
	{
		get
		{
			return _sourceId;
		}
		set
		{
			_sourceId = value;
			_paramMask |= 1u;
		}
	}

	public PermittedUsersParams()
	{
		InternalInit();
	}

	public PermittedUsersParams(Command tCommand, uint unMaskToCheck = 1u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
	}

	public PermittedUsersParams InitFromCommand(Command tCommand, uint unMaskToCheck = 1u)
	{
		try
		{
			ParamsHelper.Convertion<Guid> convFunc = (string x) => Guid.TryParse(x, out var result) ? result : Guid.Empty;
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "SourceId", ref _sourceId, PermittedUsersParamsHelper.ParamMask.SourceId, convFunc, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "PermittedUsersParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, PermittedUsersParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, PermittedUsersParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public PermittedUsersParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "SourceId", _sourceId, PermittedUsersParamsHelper.ParamMask.SourceId);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "PermittedUsersParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckAlarmParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
