using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ActivateParams
{
	private Guid _objectId;

	private ActivateParamsHelper.TriggerType _triggerType;

	private uint _paramMask;

	public Guid ObjectId
	{
		get
		{
			return _objectId;
		}
		set
		{
			_objectId = value;
			_paramMask |= 1u;
		}
	}

	public ActivateParamsHelper.TriggerType TriggerType
	{
		get
		{
			return _triggerType;
		}
		set
		{
			_triggerType = value;
			_paramMask |= 2u;
		}
	}

	public ActivateParams()
	{
		InternalInit();
	}

	public ActivateParams(Command tCommand, uint unMaskToCheck = 1u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_objectId = Guid.Empty;
	}

	public ActivateParams InitFromCommand(Command tCommand, uint unMaskToCheck = 1u)
	{
		try
		{
			ParamsHelper.Convertion<Guid> convFunc = (string x) => (!(x != string.Empty)) ? Guid.Empty : new Guid(x);
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "ObjectId", ref _objectId, ActivateParamsHelper.ParamMask.ObjectId, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "TriggerType", ref _triggerType, ActivateParamsHelper.ParamMask.TriggerType, EnumsHelper.EnumConverter<ActivateParamsHelper.TriggerType>, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ActivateParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, ActivateParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, ActivateParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public ActivateParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			ParamsHelper.ConvToString<ActivateParamsHelper.TriggerType> converter = (ActivateParamsHelper.TriggerType x) => EnumsHelper.GetDescription(x);
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "ObjectId", _objectId, ActivateParamsHelper.ParamMask.ObjectId);
			ConvertParameterTo(dictionary, "TriggerType", _triggerType, ActivateParamsHelper.ParamMask.TriggerType, converter);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ActivateParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckActivateParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
