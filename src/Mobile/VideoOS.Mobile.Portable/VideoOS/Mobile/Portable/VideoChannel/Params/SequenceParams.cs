using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class SequenceParams : IComparable<SequenceParams>, IComparable, ICloneable, IDefineKind
{
	private uint _paramMask;

	private SequenceParamsHelper.SequenceType _type;

	private DateTime _time;

	private TimeSpan _minTimeBetweenSequences;

	private TimeSpan _beforeTime;

	private int _beforeCount;

	private TimeSpan _afterTime;

	private int _afterCount;

	private readonly List<Guid> _itemIds = new List<Guid>();

	private string _itemKind;

	private bool _forceSequencesFromExport;

	private Guid _investigationId;

	public SequenceParamsHelper.SequenceType Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
			_paramMask |= 1u;
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
			_paramMask |= 2u;
		}
	}

	public TimeSpan MinTimeBetweenSequences
	{
		get
		{
			return _minTimeBetweenSequences;
		}
		set
		{
			_minTimeBetweenSequences = value;
			_paramMask |= 1024u;
		}
	}

	public TimeSpan BeforeTime
	{
		get
		{
			return _beforeTime;
		}
		set
		{
			_beforeTime = value;
			_paramMask |= 4u;
		}
	}

	public int BeforeCount
	{
		get
		{
			return _beforeCount;
		}
		set
		{
			_beforeCount = value;
			_paramMask |= 8u;
		}
	}

	public TimeSpan AfterTime
	{
		get
		{
			return _afterTime;
		}
		set
		{
			_afterTime = value;
			_paramMask |= 16u;
		}
	}

	public int AfterCount
	{
		get
		{
			return _afterCount;
		}
		set
		{
			_afterCount = value;
			_paramMask |= 32u;
		}
	}

	public IEnumerable<Guid> ItemIds => _itemIds;

	public uint ParamMask => _paramMask;

	public bool ForceSequencesFromExport
	{
		get
		{
			return _forceSequencesFromExport;
		}
		set
		{
			_forceSequencesFromExport = value;
			_paramMask |= 128u;
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
			_paramMask |= 256u;
		}
	}

	public string ItemKind
	{
		get
		{
			return _itemKind;
		}
		set
		{
			_itemKind = value;
			_paramMask |= 512u;
		}
	}

	public void SetItemIds(IEnumerable<Guid> itemIds)
	{
		_itemIds.Clear();
		AddItemIds(itemIds);
	}

	private void AddItemIds(IEnumerable<Guid> itemIds)
	{
		if (itemIds != null)
		{
			_itemIds.AddRange(itemIds);
			if (_itemIds.Any())
			{
				_paramMask |= 64u;
			}
		}
	}

	public SequenceParams()
	{
		InternalInit();
	}

	public SequenceParams(Command tCommand, SequenceParamsHelper.ParamMask unMaskToCheck = SequenceParamsHelper.ParamMask.All)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_type = SequenceParamsHelper.SequenceType.Unknown;
		_time = DateTime.UtcNow;
		_minTimeBetweenSequences = new TimeSpan(0L);
		_beforeTime = new TimeSpan(0L);
		_beforeCount = 0;
		_afterTime = new TimeSpan(0L);
		_afterCount = 0;
		_itemKind = "Camera";
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, SequenceParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public SequenceParams InitFromCommand(Command tCommand, SequenceParamsHelper.ParamMask unMaskToCheck = SequenceParamsHelper.ParamMask.All)
	{
		try
		{
			ParamsHelper.Convertion<TimeSpan> convFunc = (string x) => TimeSpan.FromSeconds(Convert.ToDouble(x, CultureInfo.InvariantCulture));
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "Time", ref _time, SequenceParamsHelper.ParamMask.Time, TimeConverter.FromString, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "SeqType", ref _type, SequenceParamsHelper.ParamMask.SeqType, EnumsHelper.EnumConverter<SequenceParamsHelper.SequenceType>, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "MinTimeBetweenSequences", ref _minTimeBetweenSequences, SequenceParamsHelper.ParamMask.MinTimeBetweenSequences, convFunc, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "BeforeTime", ref _beforeTime, SequenceParamsHelper.ParamMask.BeforeTime, convFunc, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "BeforeCount", ref _beforeCount, SequenceParamsHelper.ParamMask.BeforeCount, Convert.ToInt32, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "AfterTime", ref _afterTime, SequenceParamsHelper.ParamMask.AfterTime, convFunc, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "AfterCount", ref _afterCount, SequenceParamsHelper.ParamMask.AfterCount, Convert.ToInt32, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "ForceSequencesFromExport", ref _forceSequencesFromExport, SequenceParamsHelper.ParamMask.ForceSequencesFromExport, ParamsHelper.BoolConvertion, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "InvestigationId", ref _investigationId, SequenceParamsHelper.ParamMask.InvestigationId, ParamsHelper.GuidConvertion, (uint)unMaskToCheck);
			ConvertParameter(dictionary, "ItemKind", ref _itemKind, SequenceParamsHelper.ParamMask.ItemKind, ParamsHelper.StringConvertion, (uint)unMaskToCheck);
			IEnumerable<Guid> itemIds = (from param in tCommand.InputParams
				where string.CompareOrdinal(param.ParamName, "CameraId") == 0 || string.CompareOrdinal(param.ParamName, "ItemId") == 0
				select new
				{
					IsParsed = Guid.TryParse(param.ParamValue, out var result),
					ItemId = result
				} into el
				where el.IsParsed
				select el.ItemId).Distinct();
			AddItemIds(itemIds);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "SequenceParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, SequenceParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public static string SeekConvertorTo(PlaybackParamsHelper.SeekType seek)
	{
		return EnumsHelper.GetDescription(seek);
	}

	public SequenceParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			ParamsHelper.ConvToString<SequenceParamsHelper.SequenceType> converter = (SequenceParamsHelper.SequenceType x) => EnumsHelper.GetDescription(x);
			ParamsHelper.ConvToString<TimeSpan> converter2 = (TimeSpan x) => x.TotalSeconds.ToString();
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "Time", _time, SequenceParamsHelper.ParamMask.Time, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "SeqType", _type, SequenceParamsHelper.ParamMask.Time, converter);
			ConvertParameterTo(dictionary, "MinTimeBetweenSequences", _minTimeBetweenSequences, SequenceParamsHelper.ParamMask.MinTimeBetweenSequences, converter2);
			ConvertParameterTo(dictionary, "BeforeTime", _beforeTime, SequenceParamsHelper.ParamMask.BeforeTime, converter2);
			ConvertParameterTo(dictionary, "BeforeCount", _beforeCount, SequenceParamsHelper.ParamMask.BeforeCount);
			ConvertParameterTo(dictionary, "AfterTime", _afterTime, SequenceParamsHelper.ParamMask.AfterTime, converter2);
			ConvertParameterTo(dictionary, "AfterCount", _afterCount, SequenceParamsHelper.ParamMask.AfterCount);
			ConvertParameterTo(dictionary, "ForceSequencesFromExport", _forceSequencesFromExport, SequenceParamsHelper.ParamMask.ForceSequencesFromExport, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "InvestigationId", _investigationId, SequenceParamsHelper.ParamMask.InvestigationId);
			ConvertParameterTo(dictionary, "ItemKind", _itemKind, SequenceParamsHelper.ParamMask.ItemKind);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
			foreach (string item in _itemIds.Select((Guid guid) => guid.ToString()))
			{
				tCommand.InputParams.Add(new Parameter("CameraId", item));
				tCommand.InputParams.Add(new Parameter("ItemId", item));
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "SequenceParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckParams(SequenceParamsHelper.ParamMask tMaskToCheck)
	{
		return ParamsHelper.CheckParams((uint)tMaskToCheck, ParamMask);
	}

	private void SetField<T>(ref T tToSet, ref T tField, SequenceParamsHelper.ParamMask tCheckMask, ref uint tSetMask, ParamsHelper.CopyMethod<T> copyFunc = null)
	{
		ParamsHelper.SetField(ref tToSet, ref tField, ParamMask, (uint)tCheckMask, ref tSetMask, copyFunc);
	}

	public SequenceParams InitFromAnother(SequenceParams tAnother)
	{
		tAnother.SetItemIds(tAnother.ItemIds);
		tAnother.SetField(ref _type, ref tAnother._type, SequenceParamsHelper.ParamMask.SeqType, ref _paramMask);
		tAnother.SetField(ref _time, ref tAnother._time, SequenceParamsHelper.ParamMask.Time, ref _paramMask);
		tAnother.SetField(ref _beforeTime, ref tAnother._beforeTime, SequenceParamsHelper.ParamMask.BeforeTime, ref _paramMask);
		tAnother.SetField(ref _beforeCount, ref tAnother._beforeCount, SequenceParamsHelper.ParamMask.BeforeCount, ref _paramMask);
		tAnother.SetField(ref _afterTime, ref tAnother._afterTime, SequenceParamsHelper.ParamMask.AfterTime, ref _paramMask);
		tAnother.SetField(ref _afterCount, ref tAnother._afterCount, SequenceParamsHelper.ParamMask.AfterCount, ref _paramMask);
		return this;
	}

	private void CompareCheck<T>(ref int nResult, T first, T second, ParamsHelper.CompareFunc<T> func, uint unMaskToCheck, SequenceParamsHelper.ParamMask tMask)
	{
		ParamsHelper.CompareCheck(ref nResult, first, second, func, unMaskToCheck, (uint)tMask);
	}

	private static void CompareTypeE(ref int nResult, SequenceParamsHelper.SequenceType first, SequenceParamsHelper.SequenceType second)
	{
		VideoOS.Mobile.Portable.Utilities.Math.CompareE(ref nResult, (int)first, (int)second);
	}

	public int CompareTo(SequenceParams other, uint unMaskToCheck)
	{
		if (other == null)
		{
			return 1;
		}
		int nResult = 0;
		CompareCheck(ref nResult, ItemIds.OrderBy((Guid el) => el), other.ItemIds.OrderBy((Guid el) => el), VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, SequenceParamsHelper.ParamMask.ItemId);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, Type, other.Type, CompareTypeE, unMaskToCheck, SequenceParamsHelper.ParamMask.SeqType);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, Time, other.Time, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, SequenceParamsHelper.ParamMask.Time);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, BeforeTime, other.BeforeTime, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, SequenceParamsHelper.ParamMask.BeforeTime);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, BeforeCount, other.BeforeCount, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, SequenceParamsHelper.ParamMask.BeforeCount);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, AfterTime, other.AfterTime, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, SequenceParamsHelper.ParamMask.AfterTime);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, AfterCount, other.AfterCount, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, SequenceParamsHelper.ParamMask.AfterCount);
		return nResult;
	}

	public int CompareTo(SequenceParams other)
	{
		return CompareTo(other, 2047u);
	}

	public int CompareTo(object obj)
	{
		return CompareTo(obj as SequenceParams);
	}

	public object Clone()
	{
		return (SequenceParams)MemberwiseClone();
	}

	public bool CheckForMask(SequenceParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}
}
