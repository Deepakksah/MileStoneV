using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class FootageParams : DataParams
{
	private Guid _streamId;

	private string _footageName;

	private string _fileName;

	private long _fileSize;

	private StreamParamsHelper.ByteOrder _byteOrder;

	private Guid _exportId;

	public string FootageName
	{
		get
		{
			return _footageName;
		}
		set
		{
			_footageName = value;
			_paramMask |= 1u;
		}
	}

	public string FileName
	{
		get
		{
			return _fileName;
		}
		set
		{
			_fileName = value;
			_paramMask |= 2u;
		}
	}

	public long FileSize
	{
		get
		{
			return _fileSize;
		}
		set
		{
			_fileSize = value;
			_paramMask |= 4u;
		}
	}

	public override Guid StreamId
	{
		get
		{
			return _streamId;
		}
		set
		{
			_streamId = value;
			_paramMask |= 8u;
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
			_paramMask |= 16u;
		}
	}

	public Guid ExportId
	{
		get
		{
			return _exportId;
		}
		set
		{
			_exportId = value;
			_paramMask |= 32u;
		}
	}

	public uint ParamMask => _paramMask;

	public FootageParams()
	{
		InternalInit();
	}

	public FootageParams(Command tCommand, uint unMaskToCheck = 63u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, FootageParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public FootageParams InitFromCommand(Command tCommand, uint unMaskToCheck = 63u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "FootageName", ref _footageName, FootageParamsHelper.ParamMask.FootageName, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "FileName", ref _fileName, FootageParamsHelper.ParamMask.FileName, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "FileSize", ref _fileSize, FootageParamsHelper.ParamMask.FileSize, ParamsHelper.LongConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "StreamId", ref _streamId, FootageParamsHelper.ParamMask.StreamId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "ByteOrder", ref _byteOrder, FootageParamsHelper.ParamMask.ByteOrder, EnumsHelper.EnumConverter<StreamParamsHelper.ByteOrder>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExportId", ref _exportId, FootageParamsHelper.ParamMask.ExportId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "FootageParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, FootageParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public FootageParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "FootageName", _footageName, FootageParamsHelper.ParamMask.FootageName);
			ConvertParameterTo(dictionary, "FileName", _fileName, FootageParamsHelper.ParamMask.FileName);
			ConvertParameterTo(dictionary, "FileSize", _fileSize, FootageParamsHelper.ParamMask.FileSize, ParamsHelper.LongConvertionTo);
			ConvertParameterTo(dictionary, "StreamId", _streamId, FootageParamsHelper.ParamMask.StreamId);
			ConvertParameterTo(dictionary, "ByteOrder", _byteOrder, FootageParamsHelper.ParamMask.ByteOrder);
			ConvertParameterTo(dictionary, "ExportId", _exportId, FootageParamsHelper.ParamMask.ExportId);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "FootageParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public bool CheckForMask(FootageParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}

	public ErrorCodes CheckFootageParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, ParamMask);
	}

	public void CorrectParams()
	{
		if (CheckForMask(FootageParamsHelper.ParamMask.FootageName))
		{
			_footageName = _footageName.ReplaceXssAttackChars();
			_footageName = _footageName.ReplaceUrlReservedChars();
		}
		if (CheckForMask(FootageParamsHelper.ParamMask.FileName))
		{
			_fileName = _fileName.ReplaceXssAttackChars();
			_fileName = _fileName.ReplaceUrlReservedChars();
		}
		if (!CheckForMask(FootageParamsHelper.ParamMask.ByteOrder))
		{
			_byteOrder = StreamParamsHelper.ByteOrder.LittleEndian;
		}
	}

	public void GenerateName()
	{
		if (!CheckForMask(FootageParamsHelper.ParamMask.FootageName))
		{
			FootageName = FileName;
		}
	}
}
