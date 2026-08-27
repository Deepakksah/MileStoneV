using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class GetAlarmParams
{
	private GetAlarmParamsHelper.OperatorType _operator;

	private int _count;

	private DateTime _timestamp;

	private Guid _alarmId;

	private bool _myAlarms;

	private string _statesNotEqual;

	private string _prioritiesNotEqual;

	private string _categoriesNotEqual;

	private uint _paramMask;

	public GetAlarmParamsHelper.OperatorType Operator
	{
		get
		{
			return _operator;
		}
		set
		{
			_operator = value;
			_paramMask |= 2u;
		}
	}

	public int Count
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value;
			_paramMask |= 1u;
		}
	}

	public DateTime Timestamp
	{
		get
		{
			return _timestamp;
		}
		set
		{
			_timestamp = value;
			_paramMask |= 4u;
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
			_paramMask |= 8u;
		}
	}

	public bool MyAlarms
	{
		get
		{
			return _myAlarms;
		}
		set
		{
			_myAlarms = value;
			_paramMask |= 16u;
		}
	}

	public string StatesNE
	{
		get
		{
			return _statesNotEqual;
		}
		set
		{
			_statesNotEqual = value;
			_paramMask |= 32u;
		}
	}

	public string PrioritiesNE
	{
		get
		{
			return _prioritiesNotEqual;
		}
		set
		{
			_prioritiesNotEqual = value;
			_paramMask |= 64u;
		}
	}

	public string CategoriesNE
	{
		get
		{
			return _categoriesNotEqual;
		}
		set
		{
			_categoriesNotEqual = value;
			_paramMask |= 128u;
		}
	}

	public GetAlarmParams()
	{
		InternalInit();
	}

	public GetAlarmParams(Command tCommand, uint unMaskToCheck = 255u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
	}

	public GetAlarmParams InitFromCommand(Command tCommand, uint unMaskToCheck = 255u)
	{
		try
		{
			ParamsHelper.Convertion<int> convFunc = (string x) => int.TryParse(x, out var result) ? result : 0;
			ParamsHelper.Convertion<Guid> convFunc2 = (string x) => Guid.TryParse(x, out var result) ? result : Guid.Empty;
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "Count", ref _count, GetAlarmParamsHelper.ParamMask.Count, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Operator", ref _operator, GetAlarmParamsHelper.ParamMask.Operator, EnumsHelper.EnumConverter<GetAlarmParamsHelper.OperatorType>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Timestamp", ref _timestamp, GetAlarmParamsHelper.ParamMask.Timestamp, TimeConverter.FromString, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "AlarmId", ref _alarmId, GetAlarmParamsHelper.ParamMask.AlarmId, convFunc2, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "MyAlarms", ref _myAlarms, GetAlarmParamsHelper.ParamMask.MyAlarms, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "State", ref _statesNotEqual, GetAlarmParamsHelper.ParamMask.StatesNE, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Priority", ref _prioritiesNotEqual, GetAlarmParamsHelper.ParamMask.PrioritiesNE, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Category", ref _categoriesNotEqual, GetAlarmParamsHelper.ParamMask.CategoriesNE, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "AlarmParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, GetAlarmParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, GetAlarmParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public GetAlarmParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			ParamsHelper.ConvToString<GetAlarmParamsHelper.OperatorType> converter = (GetAlarmParamsHelper.OperatorType x) => EnumsHelper.GetDescription(x);
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "Count", _count, GetAlarmParamsHelper.ParamMask.Count);
			ConvertParameterTo(dictionary, "Operator", _operator, GetAlarmParamsHelper.ParamMask.Operator, converter);
			ConvertParameterTo(dictionary, "Timestamp", _timestamp, GetAlarmParamsHelper.ParamMask.Timestamp, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "AlarmId", _alarmId, GetAlarmParamsHelper.ParamMask.AlarmId);
			ConvertParameterTo(dictionary, "MyAlarms", _myAlarms, GetAlarmParamsHelper.ParamMask.MyAlarms, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "State", _statesNotEqual, GetAlarmParamsHelper.ParamMask.StatesNE);
			ConvertParameterTo(dictionary, "Priority", _prioritiesNotEqual, GetAlarmParamsHelper.ParamMask.PrioritiesNE);
			ConvertParameterTo(dictionary, "Category", _categoriesNotEqual, GetAlarmParamsHelper.ParamMask.CategoriesNE);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GetAlarmParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckAlarmParams(uint tMaskToCheck)
	{
		if (tMaskToCheck == 255)
		{
			ErrorCodes errorCodes = ParamsHelper.CheckParams(7u, _paramMask);
			if (errorCodes == ErrorCodes.Ok)
			{
				return errorCodes;
			}
			errorCodes = ParamsHelper.CheckParams(8u, _paramMask);
			if (errorCodes == ErrorCodes.Ok)
			{
				return errorCodes;
			}
			errorCodes = ParamsHelper.CheckParams(33u, _paramMask);
			if (errorCodes == ErrorCodes.Ok)
			{
				return errorCodes;
			}
			errorCodes = ParamsHelper.CheckParams(65u, _paramMask);
			if (errorCodes == ErrorCodes.Ok)
			{
				return errorCodes;
			}
			return ParamsHelper.CheckParams(129u, _paramMask);
		}
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
