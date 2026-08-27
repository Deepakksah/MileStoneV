using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ChallengesParams
{
	private uint _paramMask;

	private uint _numberChallenges;

	private Guid _challenge;

	private string _chalAnswer;

	public uint NumberChallenges
	{
		get
		{
			return _numberChallenges;
		}
		set
		{
			_numberChallenges = value;
			_paramMask |= 1u;
		}
	}

	public Guid Challenge
	{
		get
		{
			return _challenge;
		}
		set
		{
			_challenge = value;
			_paramMask |= 2u;
		}
	}

	public string ChalAnswer
	{
		get
		{
			return _chalAnswer;
		}
		set
		{
			_chalAnswer = value;
			_paramMask |= 4u;
		}
	}

	public uint ParamMask => _paramMask;

	public ChallengesParams()
	{
		InternalInit();
	}

	public ChallengesParams(Command tCommand, uint unMaskToCheck = 7u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, ChallengesParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public ChallengesParams InitFromCommand(Command tCommand, uint unMaskToCheck = 7u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "NumChallenges", ref _numberChallenges, ChallengesParamsHelper.ParamMask.NumberChallenges, Convert.ToUInt32, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Challenge", ref _challenge, ChallengesParamsHelper.ParamMask.Challenge, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "ChalAnswer", ref _chalAnswer, ChallengesParamsHelper.ParamMask.ChalAnswer, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ChallengesParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, ChallengesParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public ChallengesParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "NumChallenges", _numberChallenges, ChallengesParamsHelper.ParamMask.NumberChallenges);
			ConvertParameterTo(dictionary, "Challenge", _challenge, ChallengesParamsHelper.ParamMask.Challenge);
			ConvertParameterTo(dictionary, "ChalAnswer", _chalAnswer, ChallengesParamsHelper.ParamMask.ChalAnswer);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ChallengesParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public bool CheckForMask(ChallengesParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}
}
