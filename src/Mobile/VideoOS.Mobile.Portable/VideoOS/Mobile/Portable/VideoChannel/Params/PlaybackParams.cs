using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class PlaybackParams : ICloneable, IComparable<PlaybackParams>, IComparable
{
	private uint _paramMask;

	private double _speed;

	private PlaybackParamsHelper.SeekType _seekType;

	private DateTime _time;

	private bool _blocking;

	private Guid _controllerId;

	private Guid _investigationId;

	private TimeSpan _timeSpan;

	private bool _forcePlaybackFromExport;

	private DateTime _timeRangeEnd;

	private DateTime _timeRangeBegin;

	private bool _autoLoop;

	public double Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			_speed = value;
			_paramMask |= 1u;
		}
	}

	public PlaybackParamsHelper.SeekType SeekType
	{
		get
		{
			return _seekType;
		}
		set
		{
			_seekType = value;
			_paramMask |= 2u;
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
			_paramMask |= 4u;
		}
	}

	public DateTime TimeRangeBegin
	{
		get
		{
			return _timeRangeBegin;
		}
		set
		{
			_timeRangeBegin = value;
			_paramMask |= 256u;
		}
	}

	public DateTime TimeRangeEnd
	{
		get
		{
			return _timeRangeEnd;
		}
		set
		{
			_timeRangeEnd = value;
			_paramMask |= 512u;
		}
	}

	public bool Blocking
	{
		get
		{
			return _blocking;
		}
		set
		{
			_blocking = value;
			_paramMask |= 8u;
		}
	}

	public Guid ControllerId
	{
		get
		{
			return _controllerId;
		}
		set
		{
			_controllerId = value;
			_paramMask |= 16u;
		}
	}

	public Guid InvestigationId
	{
		get
		{
			return _investigationId;
		}
		set
		{
			_investigationId = value;
			_paramMask |= 32u;
		}
	}

	public TimeSpan TimeSpan
	{
		get
		{
			return _timeSpan;
		}
		set
		{
			_timeSpan = value;
			_paramMask |= 64u;
		}
	}

	public bool ForcePlaybackFromExport
	{
		get
		{
			return _forcePlaybackFromExport;
		}
		set
		{
			_forcePlaybackFromExport = value;
			_paramMask |= 128u;
		}
	}

	public bool AutoLoop
	{
		get
		{
			return _autoLoop;
		}
		set
		{
			_autoLoop = value;
			_paramMask |= 1024u;
		}
	}

	public uint ParamMask => _paramMask;

	public PlaybackParams()
	{
		InternalInit();
	}

	public PlaybackParams(Command tCommand, PlaybackParamsHelper.ParamMask unMaskToCheck = PlaybackParamsHelper.ParamMask.All)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_speed = 0.0;
		_seekType = PlaybackParamsHelper.SeekType.Nothing;
		_time = DateTime.MinValue;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, PlaybackParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public PlaybackParams InitFromCommand(Command tCommand, PlaybackParamsHelper.ParamMask unMaskToCheck = PlaybackParamsHelper.ParamMask.All)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "Speed", ref _speed, PlaybackParamsHelper.ParamMask.Speed, ParamsHelper.DoubleConvertion, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "SeekType", ref _seekType, PlaybackParamsHelper.ParamMask.SeekType, EnumsHelper.EnumConverter<PlaybackParamsHelper.SeekType>, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Time", ref _time, PlaybackParamsHelper.ParamMask.Time, TimeConverter.FromString, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Blocking", ref _blocking, PlaybackParamsHelper.ParamMask.Blocking, ParamsHelper.BoolConvertion, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "PlaybackControllerId", ref _controllerId, PlaybackParamsHelper.ParamMask.ControllerId, ParamsHelper.GuidConvertion, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "InvestigationId", ref _investigationId, PlaybackParamsHelper.ParamMask.InvestigationId, ParamsHelper.GuidConvertion, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "TimeSpan", ref _timeSpan, PlaybackParamsHelper.ParamMask.TimeSpan, ParamsHelper.TimeSpanConvertion, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "ForcePlaybackFromExport", ref _forcePlaybackFromExport, PlaybackParamsHelper.ParamMask.ForcePlaybackFromExport, ParamsHelper.BoolConvertion, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "TimeRangeBegin", ref _timeRangeBegin, PlaybackParamsHelper.ParamMask.TimeRangeBegin, ParamsHelper.TimeConvertion, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "TimeRangeEnd", ref _timeRangeEnd, PlaybackParamsHelper.ParamMask.TimeRangeEnd, ParamsHelper.TimeConvertion, (uint)unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "AutoLoop", ref _autoLoop, PlaybackParamsHelper.ParamMask.AutoLoop, ParamsHelper.BoolConvertion, (uint)unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "PlaybackParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, PlaybackParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public static string SeekConvertorTo(PlaybackParamsHelper.SeekType seek)
	{
		return EnumsHelper.GetDescription(seek);
	}

	public PlaybackParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			ParamsHelper.ConvToString<PlaybackParamsHelper.SeekType> converter = (PlaybackParamsHelper.SeekType x) => EnumsHelper.GetDescription(x);
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "Speed", _speed, PlaybackParamsHelper.ParamMask.Speed, ParamsHelper.DoubleConvertionTo);
			ConvertParameterTo(dictionary, "SeekType", _seekType, PlaybackParamsHelper.ParamMask.SeekType, converter);
			ConvertParameterTo(dictionary, "Time", _time, PlaybackParamsHelper.ParamMask.Time, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "Blocking", _blocking, PlaybackParamsHelper.ParamMask.Blocking, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "PlaybackControllerId", _controllerId, PlaybackParamsHelper.ParamMask.ControllerId);
			ConvertParameterTo(dictionary, "InvestigationId", _investigationId, PlaybackParamsHelper.ParamMask.InvestigationId);
			ConvertParameterTo(dictionary, "TimeSpan", _timeSpan, PlaybackParamsHelper.ParamMask.TimeSpan, ParamsHelper.TimeSpanConvertionTo);
			ConvertParameterTo(dictionary, "ForcePlaybackFromExport", _forcePlaybackFromExport, PlaybackParamsHelper.ParamMask.ForcePlaybackFromExport, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "TimeRangeBegin", _timeRangeBegin, PlaybackParamsHelper.ParamMask.TimeRangeBegin, ParamsHelper.TimeConvertionTo);
			ConvertParameterTo(dictionary, "TimeRangeEnd", _timeRangeEnd, PlaybackParamsHelper.ParamMask.TimeRangeEnd, ParamsHelper.TimeConvertionTo);
			ConvertParameterTo(dictionary, "AutoLoop", _autoLoop, PlaybackParamsHelper.ParamMask.AutoLoop, ParamsHelper.BoolConvertionTo);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "PlaybackParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	private void SetField<T>(ref T tToSet, ref T tField, PlaybackParamsHelper.ParamMask tCheckMask, ref uint tSetMask, ParamsHelper.CopyMethod<T> copyFunc = null)
	{
		ParamsHelper.SetField(ref tToSet, ref tField, ParamMask, (uint)tCheckMask, ref tSetMask, copyFunc);
	}

	public PlaybackParams InitFromAnother(PlaybackParams tAnother)
	{
		tAnother.SetField(ref _speed, ref tAnother._speed, PlaybackParamsHelper.ParamMask.Speed, ref _paramMask);
		tAnother.SetField(ref _seekType, ref tAnother._seekType, PlaybackParamsHelper.ParamMask.SeekType, ref _paramMask);
		tAnother.SetField(ref _time, ref tAnother._time, PlaybackParamsHelper.ParamMask.Time, ref _paramMask);
		tAnother.SetField(ref _blocking, ref tAnother._blocking, PlaybackParamsHelper.ParamMask.Blocking, ref _paramMask);
		tAnother.SetField(ref _controllerId, ref tAnother._controllerId, PlaybackParamsHelper.ParamMask.ControllerId, ref _paramMask);
		tAnother.SetField(ref _investigationId, ref tAnother._investigationId, PlaybackParamsHelper.ParamMask.InvestigationId, ref _paramMask);
		tAnother.SetField(ref _timeSpan, ref tAnother._timeSpan, PlaybackParamsHelper.ParamMask.TimeSpan, ref _paramMask);
		tAnother.SetField(ref _forcePlaybackFromExport, ref tAnother._forcePlaybackFromExport, PlaybackParamsHelper.ParamMask.ForcePlaybackFromExport, ref _paramMask);
		tAnother.SetField(ref _timeRangeBegin, ref tAnother._timeRangeBegin, PlaybackParamsHelper.ParamMask.TimeRangeBegin, ref _paramMask);
		tAnother.SetField(ref _timeRangeEnd, ref tAnother._timeRangeEnd, PlaybackParamsHelper.ParamMask.TimeRangeEnd, ref _paramMask);
		tAnother.SetField(ref _autoLoop, ref tAnother._autoLoop, PlaybackParamsHelper.ParamMask.AutoLoop, ref _paramMask);
		return this;
	}

	private void CompareCheck<T>(ref int nResult, T first, T second, ParamsHelper.CompareFunc<T> func, uint unMaskToCheck, PlaybackParamsHelper.ParamMask tMask)
	{
		ParamsHelper.CompareCheck(ref nResult, first, second, func, unMaskToCheck, (uint)tMask);
	}

	private static void CompareSeekE(ref int nResult, PlaybackParamsHelper.SeekType first, PlaybackParamsHelper.SeekType second)
	{
		VideoOS.Mobile.Portable.Utilities.Math.CompareE(ref nResult, (int)first, (int)second);
	}

	private static void CompareBoolE(ref int nResult, bool first, bool second)
	{
		VideoOS.Mobile.Portable.Utilities.Math.CompareE(ref nResult, first, second);
	}

	public int CompareTo(PlaybackParams other, uint unMaskToCheck)
	{
		if (other == null)
		{
			return 1;
		}
		int nResult = 0;
		ParamsHelper.CompareFunc<double> func = delegate(ref int result, double first, double second)
		{
			VideoOS.Mobile.Portable.Utilities.Math.CompareE(ref result, first, second, 0.1);
		};
		CompareCheck(ref nResult, Speed, other.Speed, func, unMaskToCheck, PlaybackParamsHelper.ParamMask.Speed);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, SeekType, other.SeekType, CompareSeekE, unMaskToCheck, PlaybackParamsHelper.ParamMask.SeekType);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, Time, other.Time, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PlaybackParamsHelper.ParamMask.Time);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, Blocking, other.Blocking, CompareBoolE, unMaskToCheck, PlaybackParamsHelper.ParamMask.Blocking);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, ControllerId, other.ControllerId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PlaybackParamsHelper.ParamMask.ControllerId);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, InvestigationId, other.InvestigationId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PlaybackParamsHelper.ParamMask.InvestigationId);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, TimeSpan, other.TimeSpan, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PlaybackParamsHelper.ParamMask.TimeSpan);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, ForcePlaybackFromExport, other.ForcePlaybackFromExport, CompareBoolE, unMaskToCheck, PlaybackParamsHelper.ParamMask.ForcePlaybackFromExport);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, TimeRangeBegin, other.TimeRangeBegin, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PlaybackParamsHelper.ParamMask.TimeRangeBegin);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, TimeRangeEnd, other.TimeRangeEnd, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, PlaybackParamsHelper.ParamMask.TimeRangeEnd);
		return nResult;
	}

	public int CompareTo(PlaybackParams other)
	{
		return CompareTo(other, 2047u);
	}

	public int CompareTo(object obj)
	{
		return CompareTo(obj as PlaybackParams);
	}

	public object Clone()
	{
		return (PlaybackParams)MemberwiseClone();
	}

	public bool CheckForMask(PlaybackParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}

	public ErrorCodes CheckParams(PlaybackParamsHelper.ParamMask tMaskToCheck)
	{
		return ParamsHelper.CheckParams((uint)tMaskToCheck, ParamMask);
	}

	public ErrorCodes ValidateParams()
	{
		ErrorCodes errorCodes = ErrorCodes.Ok;
		if (CheckForMask(PlaybackParamsHelper.ParamMask.TimeRangeBegin) || CheckForMask(PlaybackParamsHelper.ParamMask.TimeRangeEnd))
		{
			if (!CheckForMask(PlaybackParamsHelper.ParamMask.TimeRangeMandatory))
			{
				errorCodes = ErrorCodes.MissingInputParameter;
			}
			else if (TimeRangeEnd != TimeConverter.Dt1970 && CheckForMask(PlaybackParamsHelper.ParamMask.Time) && (Time > TimeRangeEnd || Time < TimeRangeBegin))
			{
				errorCodes = ErrorCodes.TimeOutOfRange;
			}
		}
		if (CheckForMask(PlaybackParamsHelper.ParamMask.AutoLoop) && !CheckForMask(PlaybackParamsHelper.ParamMask.Speed))
		{
			errorCodes |= ErrorCodes.WrongInputParameter;
		}
		return errorCodes;
	}
}
