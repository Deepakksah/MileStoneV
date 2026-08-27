using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class AcknowledgeAlarmParams
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ParamsHelper.Convertion<Guid> _003C_003E9__8_0;

		public static ParamsHelper.Convertion<int> _003C_003E9__8_1;

		public Guid _003CInitFromCommand_003Eb__8_0(string x)
		{
			if (!(x != string.Empty))
			{
				return Guid.Empty;
			}
			return new Guid(x);
		}

		public int _003CInitFromCommand_003Eb__8_1(string x)
		{
			if (int.TryParse(x, out var result))
			{
				return result;
			}
			return 0;
		}
	}

	private Guid _id;

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

	public AcknowledgeAlarmParams()
	{
		InternalInit();
	}

	public AcknowledgeAlarmParams(Command tCommand, uint unMaskToCheck = 1u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_id = Guid.Empty;
	}

	public AcknowledgeAlarmParams InitFromCommand(Command tCommand, uint unMaskToCheck = 1u)
	{
		try
		{
			ParamsHelper.Convertion<Guid> convFunc = (string x) => (!(x != string.Empty)) ? Guid.Empty : new Guid(x);
			if (_003C_003Ec._003C_003E9__8_1 == null)
			{
				_003C_003Ec._003C_003E9__8_1 = (string x) => int.TryParse(x, out var result) ? result : 0;
			}
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "Id", ref _id, AcknowledgeAlarmParamsHelper.ParamMask.Id, convFunc, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "AcknowledgeAlarmParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, AcknowledgeAlarmParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, AcknowledgeAlarmParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public AcknowledgeAlarmParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "Id", _id, AcknowledgeAlarmParamsHelper.ParamMask.Id);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "AcknowledgeAlarmParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckAlarmParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
