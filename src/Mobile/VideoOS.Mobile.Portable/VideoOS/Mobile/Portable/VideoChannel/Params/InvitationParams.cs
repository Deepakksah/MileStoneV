using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class InvitationParams : ICloneable, IComparable, IComparable<InvitationParams>
{
	private string _appLink;

	private string _emails;

	private uint _paramMask;

	private Guid _serverId;

	private string _slc;

	private string _storeLink;

	private string _specificCulture;

	public uint ParamMask => _paramMask;

	public string AppLink
	{
		get
		{
			return _appLink;
		}
		set
		{
			_appLink = value;
			_paramMask |= 32u;
		}
	}

	public string StoreLink
	{
		get
		{
			return _storeLink;
		}
		set
		{
			_storeLink = value;
			_paramMask |= 16u;
		}
	}

	public string Emails
	{
		get
		{
			return _emails;
		}
		set
		{
			_emails = value;
			_paramMask |= 8u;
		}
	}

	public string SpecificCulture
	{
		get
		{
			return _specificCulture;
		}
		set
		{
			_specificCulture = value;
			_paramMask |= 64u;
		}
	}

	public string Slc
	{
		get
		{
			return _slc;
		}
		set
		{
			_slc = value;
			_paramMask |= 1u;
		}
	}

	public Guid ServerId
	{
		get
		{
			return _serverId;
		}
		set
		{
			_serverId = value;
			_paramMask |= 4u;
		}
	}

	public InvitationParams()
	{
		InternalInit();
	}

	public InvitationParams(Command tCommand, uint unMaskToCheck = 125u, Command.ParamsType paramsType = Command.ParamsType.Input)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck, paramsType);
	}

	public object Clone()
	{
		return (InvitationParams)MemberwiseClone();
	}

	public int CompareTo(object obj)
	{
		return CompareTo(obj as InvitationParams);
	}

	public int CompareTo(InvitationParams other)
	{
		return CompareTo(other, 125u);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_slc = string.Empty;
		_serverId = Guid.Empty;
		_emails = string.Empty;
		_appLink = string.Empty;
		_storeLink = string.Empty;
		_specificCulture = string.Empty;
	}

	public ErrorCodes CheckParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}

	private void SetField<T>(ref T tToSet, ref T tField, InvitationParamsHelper.ParamMask tCheckMask, ref uint tSetMask, ParamsHelper.CopyMethod<T> copyFunc = null)
	{
		ParamsHelper.SetField(ref tToSet, ref tField, ParamMask, (uint)tCheckMask, ref tSetMask, copyFunc);
	}

	public InvitationParams InitFromAnother(InvitationParams tAnother)
	{
		tAnother.SetField(ref _slc, ref tAnother._slc, InvitationParamsHelper.ParamMask.Slc, ref _paramMask);
		tAnother.SetField(ref _storeLink, ref tAnother._storeLink, InvitationParamsHelper.ParamMask.StoreLink, ref _paramMask);
		tAnother.SetField(ref _appLink, ref tAnother._appLink, InvitationParamsHelper.ParamMask.AppLink, ref _paramMask);
		tAnother.SetField(ref _emails, ref tAnother._emails, InvitationParamsHelper.ParamMask.Emails, ref _paramMask);
		tAnother.SetField(ref _serverId, ref tAnother._serverId, InvitationParamsHelper.ParamMask.ServerId, ref _paramMask);
		tAnother.SetField(ref _specificCulture, ref tAnother._specificCulture, InvitationParamsHelper.ParamMask.SpecificCulture, ref _paramMask);
		return this;
	}

	private void CompareCheck<T>(ref int nResult, T first, T second, ParamsHelper.CompareFunc<T> func, uint unMaskToCheck, InvitationParamsHelper.ParamMask tMask)
	{
		ParamsHelper.CompareCheck(ref nResult, first, second, func, unMaskToCheck, (uint)tMask);
	}

	private static void CompareBoolE(ref int nResult, bool first, bool second)
	{
		VideoOS.Mobile.Portable.Utilities.Math.CompareE(ref nResult, first, second);
	}

	public int CompareTo(InvitationParams other, uint unMaskToCheck)
	{
		if (other == null)
		{
			return 1;
		}
		if ((_paramMask & unMaskToCheck) != (other._paramMask & unMaskToCheck))
		{
			return 1;
		}
		int nResult = 0;
		CompareCheck(ref nResult, Slc, other.Slc, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, InvitationParamsHelper.ParamMask.Slc);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, StoreLink, other.StoreLink, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, InvitationParamsHelper.ParamMask.StoreLink);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, AppLink, other.AppLink, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, InvitationParamsHelper.ParamMask.AppLink);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, Emails, other.Emails, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, InvitationParamsHelper.ParamMask.Emails);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, ServerId, other.ServerId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, InvitationParamsHelper.ParamMask.ServerId);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, SpecificCulture, other.SpecificCulture, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, InvitationParamsHelper.ParamMask.SpecificCulture);
		return nResult;
	}

	public bool CheckForMask(InvitationParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}

	public ErrorCodes CheckParams(InvitationParamsHelper.ParamMask tMaskToCheck)
	{
		return ParamsHelper.CheckParams((uint)tMaskToCheck, ParamMask);
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, InvitationParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public InvitationParams InitFromCommand(Command tCommand, uint unMaskToCheck = 125u, Command.ParamsType paramsType = Command.ParamsType.Input)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(paramsType);
			ConvertParameter(dictionary, "Slc", ref _slc, InvitationParamsHelper.ParamMask.Slc, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "Emails", ref _emails, InvitationParamsHelper.ParamMask.Emails, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "ServerId", ref _serverId, InvitationParamsHelper.ParamMask.ServerId, ParamsHelper.GuidConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "StoreLink", ref _storeLink, InvitationParamsHelper.ParamMask.StoreLink, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "AppLink", ref _appLink, InvitationParamsHelper.ParamMask.AppLink, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SpecificCulture", ref _specificCulture, InvitationParamsHelper.ParamMask.SpecificCulture, ParamsHelper.StringConvertion, unMaskToCheck);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "InvitationParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, InvitationParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public InvitationParams ConvertToCommand(ref Command tCommand, Command.ParamsType paramsType = Command.ParamsType.Input)
	{
		try
		{
			tCommand.ConvertToDictionary(paramsType);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(paramsType);
			ConvertParameterTo(dictionary, "Slc", _slc, InvitationParamsHelper.ParamMask.Slc);
			ConvertParameterTo(dictionary, "Emails", _emails, InvitationParamsHelper.ParamMask.Emails);
			ConvertParameterTo(dictionary, "ServerId", _serverId, InvitationParamsHelper.ParamMask.ServerId);
			ConvertParameterTo(dictionary, "StoreLink", _storeLink, InvitationParamsHelper.ParamMask.StoreLink);
			ConvertParameterTo(dictionary, "AppLink", _appLink, InvitationParamsHelper.ParamMask.AppLink);
			ConvertParameterTo(dictionary, "SpecificCulture", _specificCulture, InvitationParamsHelper.ParamMask.SpecificCulture);
			tCommand.ConvertFromDictionary(paramsType);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "InvitationParams.ConvertToCommand", ex.Message);
		}
		return this;
	}
}
