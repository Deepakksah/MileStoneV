using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class OutputsEventsParams
{
	private Guid _cameraId;

	private uint _paramMask;

	public Guid CameraId
	{
		get
		{
			return _cameraId;
		}
		set
		{
			_cameraId = value;
			_paramMask |= 1u;
		}
	}

	public OutputsEventsParams()
	{
		InternalInit();
	}

	public OutputsEventsParams(Command tCommand, uint unMaskToCheck = 1u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_cameraId = Guid.Empty;
	}

	public OutputsEventsParams InitFromCommand(Command tCommand, uint unMaskToCheck = 1u)
	{
		try
		{
			ParamsHelper.Convertion<Guid> convFunc = (string x) => (!(x != string.Empty)) ? Guid.Empty : new Guid(x);
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "CameraId", ref _cameraId, OutputsEventsParamsHelper.ParamMask.CameraId, convFunc, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "OutputsEventsParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, OutputsEventsParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, OutputsEventsParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public OutputsEventsParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "CameraId", _cameraId, OutputsEventsParamsHelper.ParamMask.CameraId);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "OutputsEventsParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckOutputsAndEventsParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
