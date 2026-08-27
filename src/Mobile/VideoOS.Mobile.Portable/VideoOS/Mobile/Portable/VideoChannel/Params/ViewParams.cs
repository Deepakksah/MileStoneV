using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ViewParams
{
	private uint _paramMask;

	private Guid _viewId;

	private bool _includeEmptyItems;

	public Guid ViewId
	{
		get
		{
			return _viewId;
		}
		set
		{
			_viewId = value;
			_paramMask |= 1u;
		}
	}

	public bool IncludeEmptyItems
	{
		get
		{
			return _includeEmptyItems;
		}
		set
		{
			_includeEmptyItems = value;
			_paramMask |= 2u;
		}
	}

	public uint ParamMask => _paramMask;

	public ViewParams()
	{
		InternalInit();
	}

	public ViewParams(Command tCommand, uint unMaskToCheck = 3u)
		: this()
	{
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_includeEmptyItems = false;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, ViewParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public ViewParams InitFromCommand(Command tCommand, uint unMaskToCheck = 3u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "ViewId", ref _viewId, ViewParamsHelper.ParamMask.ViewId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "IncludeEmptyItems", ref _includeEmptyItems, ViewParamsHelper.ParamMask.IncludeEmptyItems, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ViewParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, ViewParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public ViewParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "ViewId", _viewId, ViewParamsHelper.ParamMask.ViewId);
			ConvertParameterTo(dictionary, "IncludeEmptyItems", _includeEmptyItems, ViewParamsHelper.ParamMask.IncludeEmptyItems, ParamsHelper.BoolConvertionTo);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ViewParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public bool CheckForMask(ViewParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}
}
