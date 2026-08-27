using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class StreamParams : DataParams, ICloneable, IComparable, IComparable<StreamParams>
{
	protected Guid _streamId;

	protected Guid _itemId = Guid.Empty;

	protected List<Guid> _itemsGuids;

	protected double _compressionLvl;

	protected StreamParamsHelper.SignalType _signalType;

	protected StreamParamsHelper.MethodType _methodType;

	protected StreamParamsHelper.StreamHeaders _streamHeaders;

	protected StreamParamsHelper.StreamDataType _streamDataType;

	protected StreamParamsHelper.StreamType _streamType;

	protected StreamParamsHelper.ByteOrder _byteOrder;

	protected bool _closeOldControllers;

	private bool _closeConnectionOnError;

	public static StreamParamsHelper.StreamIdUsed StreamIdUsed { get; set; }

	public override Guid StreamId
	{
		get
		{
			return VideoId;
		}
		set
		{
			VideoId = value;
		}
	}

	public Guid VideoId
	{
		get
		{
			return _streamId;
		}
		set
		{
			_streamId = value;
			_paramMask |= 4u;
		}
	}

	public Guid ItemId
	{
		get
		{
			return _itemId;
		}
		set
		{
			_itemId = value;
			_paramMask |= 1u;
		}
	}

	public List<Guid> ItemsIds
	{
		get
		{
			return _itemsGuids;
		}
		set
		{
			_itemsGuids = value;
			_paramMask |= 1u;
		}
	}

	public double CompressionLvl
	{
		get
		{
			return _compressionLvl;
		}
		set
		{
			_compressionLvl = value;
			_paramMask |= 2u;
		}
	}

	public StreamParamsHelper.SignalType SignalType
	{
		get
		{
			return _signalType;
		}
		set
		{
			_signalType = value;
			_paramMask |= 32u;
		}
	}

	public StreamParamsHelper.MethodType MethodType
	{
		get
		{
			return _methodType;
		}
		set
		{
			_methodType = value;
			_paramMask |= 64u;
		}
	}

	public StreamParamsHelper.StreamHeaders StreamHeaders
	{
		get
		{
			return _streamHeaders;
		}
		set
		{
			_streamHeaders = value;
			_paramMask |= 16u;
		}
	}

	public StreamParamsHelper.StreamDataType StreamDataType
	{
		get
		{
			return _streamDataType;
		}
		set
		{
			_streamDataType = value;
			_paramMask |= 8u;
		}
	}

	public StreamParamsHelper.StreamType StreamType
	{
		get
		{
			return _streamType;
		}
		set
		{
			_streamType = value;
			_paramMask |= 128u;
		}
	}

	public override StreamParamsHelper.ByteOrder ByteOrder
	{
		get
		{
			return _byteOrder;
		}
		set
		{
			_byteOrder = value;
			_paramMask |= 256u;
		}
	}

	public bool CloseConnectionOnError
	{
		get
		{
			return _closeConnectionOnError;
		}
		set
		{
			_closeConnectionOnError = value;
			_paramMask |= 512u;
		}
	}

	public uint ParamMask => _paramMask;

	static StreamParams()
	{
		StreamIdUsed = StreamParamsHelper.StreamIdUsed.StreamId;
	}

	protected void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, uint tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public virtual void InitFromCommand(Command tCommand, uint unMaskToCheck = 1023u, bool bRequest = true)
	{
		try
		{
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			_paramMask = 0u;
			ConvertParameter(dictionary, "CameraId", ref _itemId, 1u, ParamsHelper.GuidConvertion, unMaskToCheck, bRequest);
			GetAllItemIdsFromInputParams(dictionary, tCommand, unMaskToCheck);
			ConvertParameter(dictionary, "ComprLevel", ref _compressionLvl, 2u, ParamsHelper.DoubleConvertion, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "VideoId", ref _streamId, 4u, ParamsHelper.GuidConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "StreamId", ref _streamId, 4u, ParamsHelper.GuidConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SignalType", ref _signalType, 32u, EnumsHelper.EnumConverter<StreamParamsHelper.SignalType>, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "MethodType", ref _methodType, 64u, EnumsHelper.EnumConverter<StreamParamsHelper.MethodType>, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "StreamHeaders", ref _streamHeaders, 16u, EnumsHelper.EnumConverter<StreamParamsHelper.StreamHeaders>, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "StreamDataType", ref _streamDataType, 8u, EnumsHelper.EnumConverter<StreamParamsHelper.StreamDataType>, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "StreamType", ref _streamType, 128u, EnumsHelper.EnumConverter<StreamParamsHelper.StreamType>, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "ByteOrder", ref _byteOrder, 256u, EnumsHelper.EnumConverter<StreamParamsHelper.ByteOrder>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "CloseConnectionOnError", ref _closeConnectionOnError, 512u, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "StreamParams.InitFromCommand", ex.Message);
		}
	}

	private void GetAllItemIdsFromInputParams(Dictionary<string, string> dictInputParams, Command tCommand, uint unMaskToCheck)
	{
		if (dictInputParams.ContainsKey("ItemIds"))
		{
			string tOutput = string.Empty;
			ConvertParameter(dictInputParams, "ItemIds", ref tOutput, 1u, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			_itemsGuids = CommaSeparatedItemIdsToList(tOutput);
			return;
		}
		_itemsGuids = tCommand.InputParams.Where((Parameter p) => string.CompareOrdinal(p.ParamName, "ItemId") == 0).Select(delegate(Parameter i)
		{
			Guid result = Guid.Empty;
			Guid.TryParse(i.ParamValue, out result);
			_paramMask |= 1u;
			return result;
		}).ToList();
		if (_itemId != Guid.Empty)
		{
			_itemsGuids.Insert(0, _itemId);
		}
		_itemId = ((_itemsGuids.Count > 0) ? _itemsGuids.First() : _itemId);
	}

	private List<Guid> CommaSeparatedItemIdsToList(string csIds)
	{
		return csIds.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(delegate(string i)
		{
			Guid result = Guid.Empty;
			Guid.TryParse(i, out result);
			return result;
		}).ToList();
	}

	protected void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, uint eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, eCheckMask, _paramMask, converter);
	}

	public virtual void ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "CameraId", _itemId, 1u);
			ConvertParameterTo(dictionary, "ItemId", _itemId, 1u);
			ConvertParameterTo(dictionary, "ComprLevel", _compressionLvl, 2u, ParamsHelper.DoubleConvertionTo);
			if ((StreamIdUsed & StreamParamsHelper.StreamIdUsed.VideoId) == StreamParamsHelper.StreamIdUsed.VideoId)
			{
				ConvertParameterTo(dictionary, "VideoId", _streamId, 4u);
			}
			if ((StreamIdUsed & StreamParamsHelper.StreamIdUsed.StreamId) == StreamParamsHelper.StreamIdUsed.StreamId)
			{
				ConvertParameterTo(dictionary, "StreamId", _streamId, 4u);
			}
			ConvertParameterTo(dictionary, "SignalType", _signalType, 32u, (StreamParamsHelper.SignalType x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "MethodType", _methodType, 64u, (StreamParamsHelper.MethodType x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "StreamHeaders", _streamHeaders, 16u, (StreamParamsHelper.StreamHeaders x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "StreamDataType", _streamDataType, 8u, (StreamParamsHelper.StreamDataType x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "StreamType", _streamType, 128u, (StreamParamsHelper.StreamType x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "ByteOrder", _byteOrder, 256u, (StreamParamsHelper.ByteOrder x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "CloseConnectionOnError", _closeConnectionOnError, 512u, ParamsHelper.BoolConvertionTo);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "StreamParams.ConvertToCommand", ex.Message);
		}
	}

	protected void SetField<T>(ref T tToSet, ref T tField, uint tCheckMask, ref uint tSetMask, ParamsHelper.CopyMethod<T> copyFunc = null)
	{
		ParamsHelper.SetField(ref tToSet, ref tField, ParamMask, tCheckMask, ref tSetMask, copyFunc);
	}

	public void InitNonChangeableFields(StreamParams tAnother)
	{
		tAnother.SetField(ref _itemId, ref tAnother._itemId, 1u, ref _paramMask);
		tAnother.SetField(ref _streamId, ref tAnother._streamId, 4u, ref _paramMask);
		tAnother.SetField(ref _signalType, ref tAnother._signalType, 32u, ref _paramMask);
		tAnother.SetField(ref _methodType, ref tAnother._methodType, 64u, ref _paramMask);
		tAnother.SetField(ref _streamHeaders, ref tAnother._streamHeaders, 16u, ref _paramMask);
		tAnother.SetField(ref _streamDataType, ref tAnother._streamDataType, 8u, ref _paramMask);
		tAnother.SetField(ref _streamType, ref tAnother._streamType, 128u, ref _paramMask);
		tAnother.SetField(ref _byteOrder, ref tAnother._byteOrder, 256u, ref _paramMask);
		VideoId = tAnother.VideoId;
	}

	public virtual void InitFromAnother(StreamParams tAnother)
	{
		InitNonChangeableFields(tAnother);
		tAnother.SetField(ref _compressionLvl, ref tAnother._compressionLvl, 2u, ref _paramMask);
	}

	protected virtual void InitInternal()
	{
		_paramMask = 0u;
	}

	public StreamParams()
	{
		InitInternal();
	}

	public StreamParams(Command tCommand, uint unMaskToCheck = 1023u, bool bRequest = true)
	{
		InitInternal();
		InitFromCommand(tCommand, unMaskToCheck, bRequest);
	}

	public ErrorCodes CheckStreamParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, ParamMask);
	}

	protected void DoubleChecker(ref int nResult, double first, double second)
	{
		VideoOS.Mobile.Portable.Utilities.Math.CompareE(ref nResult, first, second, 0.1);
	}

	public int CompareTo(StreamParams other, uint unMaskToCheck)
	{
		if (other == null)
		{
			return 1;
		}
		int nResult = 0;
		ParamsHelper.CompareCheck(ref nResult, ItemId, other.ItemId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 1u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, StreamId, other.StreamId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 4u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, CompressionLvl, other.CompressionLvl, DoubleChecker, unMaskToCheck, 2u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, (int)SignalType, (int)other.SignalType, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 32u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, (int)StreamDataType, (int)other.StreamDataType, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 8u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, (int)MethodType, (int)other.MethodType, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 64u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, (int)StreamHeaders, (int)other.StreamHeaders, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 16u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, (int)StreamType, (int)other.StreamType, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 128u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, (int)ByteOrder, (int)other.ByteOrder, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 256u);
		return nResult;
	}

	public int CompareTo(StreamParams other)
	{
		return CompareTo(other, 1023u);
	}

	public int CompareTo(object obj)
	{
		return CompareTo(obj as StreamParams);
	}

	public virtual object Clone()
	{
		return (StreamParams)MemberwiseClone();
	}

	public bool CheckForMask(StreamParamsHelper.StreamParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}
}
