using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class PtzParams : ICloneable, IComparable<PtzParams>, IComparable
{
	private uint _paramMask;

	private PtzParamsHelper.PtzMoveTypes _ptzMoveType;

	private PtzParamsHelper.PtzMoves _ptzMove;

	private PtzParamsHelper.PtzGestureMove _ptzGestureMove;

	private string _ptzPreset;

	private Guid _cameraId;

	public PtzParamsHelper.PtzMoveTypes PtzMoveType
	{
		get
		{
			return _ptzMoveType;
		}
		set
		{
			_ptzMoveType = value;
			_paramMask |= 1u;
		}
	}

	public PtzParamsHelper.PtzMoves PtzMove
	{
		get
		{
			return _ptzMove;
		}
		set
		{
			_ptzMove = value;
			_paramMask |= 2u;
		}
	}

	public PtzParamsHelper.PtzGestureMove PtzGestureMove
	{
		get
		{
			return _ptzGestureMove;
		}
		set
		{
			_ptzGestureMove = value;
			_paramMask |= 4u;
		}
	}

	public string PtzPreset
	{
		get
		{
			return _ptzPreset;
		}
		set
		{
			_ptzPreset = value;
			_paramMask |= 8u;
		}
	}

	public Guid CameraId
	{
		get
		{
			return _cameraId;
		}
		set
		{
			_cameraId = value;
			_paramMask |= 16u;
		}
	}

	public uint ParamMask => _paramMask;

	public PtzParams()
	{
		InternalInit();
	}

	public PtzParams(Command tCommand, uint unMaskToCheck = 31u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_ptzMoveType = PtzParamsHelper.PtzMoveTypes.Step;
		_ptzMove = PtzParamsHelper.PtzMoves.Unknown;
		_ptzGestureMove = null;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, PtzParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string[] requiredSzKeys, string[] optionalSzKeys, ref T tOutput, PtzParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertions<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, requiredSzKeys, optionalSzKeys, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public PtzParams InitFromCommand(Command tCommand, uint unMaskToCheck = 31u)
	{
		try
		{
			ParamsHelper.Convertion<string> convFunc = (string x) => x;
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "Type", ref _ptzMoveType, PtzParamsHelper.ParamMask.PtzMoveType, EnumsHelper.EnumConverter<PtzParamsHelper.PtzMoveTypes>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "PtzMove", ref _ptzMove, PtzParamsHelper.ParamMask.PtzMove, EnumsHelper.EnumConverter<PtzParamsHelper.PtzMoves>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, new string[3] { "Type", "GestureXPercent", "GestureYPercent" }, new string[2] { "GestureDuration", "GestureTimeout" }, ref _ptzGestureMove, PtzParamsHelper.ParamMask.PtzGestureMove, PtzParamsHelper.PtzGestureMove.Converter<PtzParamsHelper.PtzGestureMove>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "PtzPreset", ref _ptzPreset, PtzParamsHelper.ParamMask.PtzPreset, convFunc, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "CameraId", ref _cameraId, PtzParamsHelper.ParamMask.CameraId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "PtzParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, PtzParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string[] szKeys, T input, PtzParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToStrings<T> converter)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKeys, input, (uint)eCheckMask, _paramMask, converter);
	}

	public PtzParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			ParamsHelper.ConvToString<PtzParamsHelper.PtzMoves> converter = (PtzParamsHelper.PtzMoves x) => EnumsHelper.GetDescription(x);
			ParamsHelper.ConvToString<PtzParamsHelper.PtzMoveTypes> converter2 = (PtzParamsHelper.PtzMoveTypes x) => EnumsHelper.GetDescription(x);
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			if (_ptzMoveType == PtzParamsHelper.PtzMoveTypes.Step)
			{
				ConvertParameterTo(dictionary, "Type", _ptzMoveType, PtzParamsHelper.ParamMask.PtzMoveType, converter2);
			}
			ConvertParameterTo(dictionary, "PtzMove", _ptzMove, PtzParamsHelper.ParamMask.PtzMove, converter);
			ConvertParameterTo(dictionary, new string[5] { "Type", "GestureXPercent", "GestureYPercent", "GestureDuration", "GestureTimeout" }, _ptzGestureMove, PtzParamsHelper.ParamMask.PtzGestureMove, PtzParamsHelper.PtzGestureMove.Converter);
			ConvertParameterTo(dictionary, "PtzPreset", _ptzPreset, PtzParamsHelper.ParamMask.PtzPreset);
			ConvertParameterTo(dictionary, "CameraId", _cameraId, PtzParamsHelper.ParamMask.CameraId);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "PtzParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	private void SetField<T>(ref T tToSet, ref T tField, PtzParamsHelper.ParamMask tCheckMask, ref uint tSetMask, ParamsHelper.CopyMethod<T> copyFunc = null)
	{
		ParamsHelper.SetField(ref tToSet, ref tField, ParamMask, (uint)tCheckMask, ref tSetMask, copyFunc);
	}

	public PtzParams InitFromAnother(PtzParams tAnother)
	{
		tAnother.SetField(ref _cameraId, ref tAnother._cameraId, PtzParamsHelper.ParamMask.CameraId, ref _paramMask);
		tAnother.SetField(ref _ptzMoveType, ref tAnother._ptzMoveType, PtzParamsHelper.ParamMask.PtzMoveType, ref _paramMask);
		tAnother.SetField(ref _ptzMove, ref tAnother._ptzMove, PtzParamsHelper.ParamMask.PtzMove, ref _paramMask);
		tAnother.SetField(ref _ptzGestureMove, ref tAnother._ptzGestureMove, PtzParamsHelper.ParamMask.PtzGestureMove, ref _paramMask);
		tAnother.SetField(ref _ptzPreset, ref tAnother._ptzPreset, PtzParamsHelper.ParamMask.PtzPreset, ref _paramMask);
		return this;
	}

	private void CompareCheck<T>(ref int nResult, T first, T second, ParamsHelper.CompareFunc<T> func, uint unMaskToCheck, PtzParamsHelper.ParamMask tMask)
	{
		ParamsHelper.CompareCheck(ref nResult, first, second, func, unMaskToCheck, (uint)tMask);
	}

	public int CompareTo(PtzParams other, uint unMaskToCheck)
	{
		if (other == null)
		{
			return 1;
		}
		int nResult = 0;
		CompareCheck(ref nResult, CameraId, other.CameraId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PtzParamsHelper.ParamMask.CameraId);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, (int)PtzMoveType, (int)other.PtzMoveType, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PtzParamsHelper.ParamMask.PtzMoveType);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, (int)PtzMove, (int)other.PtzMove, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PtzParamsHelper.ParamMask.PtzMove);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, PtzGestureMove, other.PtzGestureMove, PtzParamsHelper.PtzGestureMove.Compare, unMaskToCheck, PtzParamsHelper.ParamMask.PtzGestureMove);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, PtzPreset, other.PtzPreset, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PtzParamsHelper.ParamMask.PtzPreset);
		return nResult;
	}

	public bool CheckForMask(PtzParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}

	public object Clone()
	{
		return (PtzParams)MemberwiseClone();
	}

	public int CompareTo(PtzParams other)
	{
		return CompareTo(other, 31u);
	}

	public int CompareTo(object obj)
	{
		return CompareTo(obj as PtzParams);
	}
}
