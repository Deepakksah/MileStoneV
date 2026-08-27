using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CheckForChangesParams
{
	private uint _paramsMask;

	private bool _checkInvestigations;

	public CheckForChangesParamsMask ParamsMask => (CheckForChangesParamsMask)_paramsMask;

	public CheckForChangesParams()
		: this(null)
	{
	}

	public CheckForChangesParams(Command command, CheckForChangesParamsMask maskToCheck = CheckForChangesParamsMask.Investigations)
	{
		_paramsMask = 0u;
		if (command != null)
		{
			InitFromCommand(command, maskToCheck);
		}
	}

	public ErrorCodes CheckInvestigationParams(CheckForChangesParamsMask maskToCheck)
	{
		if (!ParamsMask.ContainsOption(maskToCheck))
		{
			return ErrorCodes.WrongInputParameter;
		}
		return ErrorCodes.Ok;
	}

	private void InitFromCommand(Command tCommand, CheckForChangesParamsMask maskToCheck = CheckForChangesParamsMask.Investigations)
	{
		try
		{
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "Investigations", ref _checkInvestigations, CheckForChangesParamsMask.Investigations, ParamsHelper.BoolConvertion, (uint)maskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "InvestigationParams.InitFromCommand", ex.Message);
		}
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, CheckForChangesParamsMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramsMask, bLog);
	}
}
