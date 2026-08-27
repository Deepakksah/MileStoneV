using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class UpdateAlarmParams
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ParamsHelper.Convertion<Guid> _003C_003E9__28_0;

		public static ParamsHelper.Convertion<int> _003C_003E9__28_1;

		public static ParamsHelper.Convertion<string> _003C_003E9__28_2;

		public static ParamsHelper.Convertion<string> _003C_003E9__28_3;

		public Guid _003CInitFromCommand_003Eb__28_0(string x)
		{
			if (!(x != string.Empty))
			{
				return Guid.Empty;
			}
			return new Guid(x);
		}

		public int _003CInitFromCommand_003Eb__28_1(string x)
		{
			if (int.TryParse(x, out var result))
			{
				return result;
			}
			return 0;
		}

		public string _003CInitFromCommand_003Eb__28_2(string x)
		{
			return x;
		}

		public string _003CInitFromCommand_003Eb__28_3(string x)
		{
			return x;
		}
	}

	private Guid _id;

	private string _text;

	private Guid _state;

	private Guid _priority;

	private DateTime _time;

	private string _assignedTo;

	private uint _paramMask;

	public Guid Id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
			_paramMask |= 1u;
		}
	}

	public string Text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value;
			_paramMask |= 2u;
		}
	}

	public Guid State
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			_paramMask |= 4u;
		}
	}

	public Guid Priority
	{
		get
		{
			return _priority;
		}
		set
		{
			_priority = value;
			_paramMask |= 8u;
		}
	}

	public DateTime Time
	{
		get
		{
			return _time;
		}
		set
		{
			_time = value;
			_paramMask |= 16u;
		}
	}

	public string AssignedTo
	{
		get
		{
			return _assignedTo;
		}
		set
		{
			_assignedTo = value;
			_paramMask |= 32u;
		}
	}

	public UpdateAlarmParams()
	{
		InternalInit();
	}

	public UpdateAlarmParams(Command tCommand, uint unMaskToCheck = 63u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_id = Guid.Empty;
	}

	public UpdateAlarmParams InitFromCommand(Command tCommand, uint unMaskToCheck = 63u)
	{
		try
		{
			ParamsHelper.Convertion<Guid> convFunc = (string x) => (!(x != string.Empty)) ? Guid.Empty : new Guid(x);
			if (_003C_003Ec._003C_003E9__28_1 == null)
			{
				_003C_003Ec._003C_003E9__28_1 = (string x) => int.TryParse(x, out var result) ? result : 0;
			}
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "Id", ref _id, UpdateAlarmParamsHelper.ParamMask.Id, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Text", ref _text, UpdateAlarmParamsHelper.ParamMask.Text, (string x) => x, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "State", ref _state, UpdateAlarmParamsHelper.ParamMask.State, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Priority", ref _priority, UpdateAlarmParamsHelper.ParamMask.Priority, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Time", ref _time, UpdateAlarmParamsHelper.ParamMask.Time, TimeConverter.FromString, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "AssignedTo", ref _assignedTo, UpdateAlarmParamsHelper.ParamMask.AssignedTo, (string x) => x, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "UpdateAlarmParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, UpdateAlarmParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, UpdateAlarmParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public UpdateAlarmParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "Id", _id, UpdateAlarmParamsHelper.ParamMask.Id);
			ConvertParameterTo(dictionary, "Text", _text, UpdateAlarmParamsHelper.ParamMask.Text);
			ConvertParameterTo(dictionary, "State", _state, UpdateAlarmParamsHelper.ParamMask.State);
			ConvertParameterTo(dictionary, "Priority", _priority, UpdateAlarmParamsHelper.ParamMask.Priority);
			ConvertParameterTo(dictionary, "Time", _time, UpdateAlarmParamsHelper.ParamMask.Time, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "AssignedTo", _assignedTo, UpdateAlarmParamsHelper.ParamMask.AssignedTo);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "UpdateAlarmParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckAlarmParams(uint tMaskToCheck)
	{
		if (tMaskToCheck == 1 && _paramMask == tMaskToCheck)
		{
			return ErrorCodes.MissingInputParameter;
		}
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
