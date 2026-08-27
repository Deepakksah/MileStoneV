using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ExportParams
{
	private uint _paramMask;

	private DateTime _startTime;

	private DateTime _endTime;

	private ExportType _exportType;

	private Guid _exportId;

	private Guid _investigationId;

	private Guid _reExportFromId;

	private string _codecName;

	private string _exportName;

	private bool _keepOriginals;

	private bool _includeAudio;

	private string _password;

	private readonly List<CameraRelatedIds> _cameras = new List<CameraRelatedIds>();

	private IEnumerable<Guid> _subExportIds;

	public const char FieldsSeparator = '-';

	public const char TimeSeparator = '.';

	public const char InvalidCharSeparator = '_';

	public const string DateInterval = "dd\\dhh\\hmm\\mss\\s";

	public const string HoursInterval = "hh\\hmm\\mss\\s";

	public const string MinutesInterval = "mm\\mss\\s";

	public const string SecondsInterval = "ss\\s";

	public static readonly string DateTimeFormat = string.Format("{0}yyyy{1}MM{1}dd{0}HH{1}mm{1}ss", '-', '.');

	public const int MaxExportNameLength = 80;

	public IEnumerable<CameraRelatedIds> Cameras => _cameras;

	public Guid CameraId
	{
		get
		{
			if (_cameras.Count > 0)
			{
				return _cameras[0].CameraId;
			}
			return Guid.Empty;
		}
		set
		{
			if (!_cameras.Any((CameraRelatedIds c) => c.CameraId == value))
			{
				_cameras.Insert(0, new CameraRelatedIds
				{
					CameraId = value
				});
			}
			_paramMask |= 1u;
		}
	}

	public Guid RelatedMicrophoneId
	{
		get
		{
			if (_cameras.Count > 0)
			{
				return _cameras.First().RelatedMicrohponeId;
			}
			return Guid.Empty;
		}
	}

	public Guid RelatedSpeakerId
	{
		get
		{
			if (_cameras.Count > 0)
			{
				return _cameras.First().RelatedSpeakerId;
			}
			return Guid.Empty;
		}
	}

	public DateTime StartTime
	{
		get
		{
			return _startTime;
		}
		set
		{
			_startTime = value;
			_paramMask |= 2u;
		}
	}

	public DateTime EndTime
	{
		get
		{
			return _endTime;
		}
		set
		{
			_endTime = value;
			_paramMask |= 4u;
		}
	}

	public ExportType Type
	{
		get
		{
			return _exportType;
		}
		set
		{
			_exportType = value;
			_paramMask |= 8u;
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
			_paramMask |= 16u;
		}
	}

	public string CodecName
	{
		get
		{
			return _codecName;
		}
		set
		{
			_codecName = value;
			_paramMask |= 32u;
		}
	}

	public string ExportName
	{
		get
		{
			return _exportName;
		}
		set
		{
			_exportName = value;
			_paramMask |= 64u;
		}
	}

	public bool KeepOriginals
	{
		get
		{
			return _keepOriginals;
		}
		set
		{
			_keepOriginals = value;
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

	public Guid ReExportFromId
	{
		get
		{
			return _reExportFromId;
		}
		set
		{
			_reExportFromId = value;
			_paramMask |= 512u;
		}
	}

	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			_password = value;
			_paramMask |= 2048u;
		}
	}

	public IEnumerable<Guid> SubExportIds
	{
		get
		{
			return _subExportIds;
		}
		set
		{
			_subExportIds = value;
			_paramMask |= 1024u;
		}
	}

	public bool IncludeAudio
	{
		get
		{
			return _includeAudio;
		}
		set
		{
			_includeAudio = value;
			_paramMask |= 144u;
		}
	}

	public uint ParamMask => _paramMask;

	public void SetCameras(IEnumerable<CameraRelatedIds> camerasGuids)
	{
		_cameras.Clear();
		_cameras.AddRange(camerasGuids);
		_paramMask |= 1u;
	}

	public void SetCameras(IEnumerable<Guid> camerasGuids)
	{
		List<CameraRelatedIds> list = new List<CameraRelatedIds>();
		foreach (Guid camerasGuid in camerasGuids)
		{
			list.Add(new CameraRelatedIds
			{
				CameraId = camerasGuid
			});
		}
		_cameras.Clear();
		_cameras.AddRange(list);
		_paramMask |= 1u;
	}

	public ExportParams()
	{
		InternalInit();
	}

	public ExportParams(Command tCommand, uint unMaskToCheck = 3071u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_exportId = Guid.Empty;
		_exportType = ExportType.Unknown;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, ExportParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public ExportParams InitFromCommand(Command tCommand, uint unMaskToCheck = 3071u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "StartTime", ref _startTime, ExportParamsHelper.ParamMask.StartTime, TimeConverter.FromString, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "EndTime", ref _endTime, ExportParamsHelper.ParamMask.EndTime, TimeConverter.FromString, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Type", ref _exportType, ExportParamsHelper.ParamMask.ExportType, EnumsHelper.EnumConverter<ExportType>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExportType", ref _exportType, ExportParamsHelper.ParamMask.ExportType, EnumsHelper.EnumConverter<ExportType>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExportId", ref _exportId, ExportParamsHelper.ParamMask.ExportId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "CodecName", ref _codecName, ExportParamsHelper.ParamMask.CodecName, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Name", ref _exportName, ExportParamsHelper.ParamMask.ExportName, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "KeepOriginals", ref _keepOriginals, ExportParamsHelper.ParamMask.KeepOriginals, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "IncludeAudio", ref _includeAudio, ExportParamsHelper.ParamMask.IncludeAudio, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "InvestigationId", ref _investigationId, ExportParamsHelper.ParamMask.InvestigationId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "ReExportFromId", ref _reExportFromId, ExportParamsHelper.ParamMask.ReExportFromId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Password", ref _password, ExportParamsHelper.ParamMask.Password, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			Guid result = Guid.Empty;
			List<Guid> list = new List<Guid>();
			foreach (Parameter inputParam in tCommand.InputParams)
			{
				if ((unMaskToCheck & 1) == 1 && string.CompareOrdinal(inputParam.ParamName, "CameraId") == 0 && Guid.TryParse(inputParam.ParamValue, out result))
				{
					_cameras.Add(new CameraRelatedIds
					{
						CameraId = result
					});
					_paramMask |= 1u;
				}
				if (string.CompareOrdinal(inputParam.ParamName, "SubExportId") == 0 && Guid.TryParse(inputParam.ParamValue, out result))
				{
					list.Add(result);
				}
			}
			if (list.Count > 0)
			{
				_subExportIds = list;
				_paramMask |= 1024u;
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ExportParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, ExportParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public ExportParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			ParamsHelper.ConvToString<ExportType> converter = (ExportType x) => EnumsHelper.GetDescription(x);
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "StartTime", _startTime, ExportParamsHelper.ParamMask.StartTime, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "EndTime", _endTime, ExportParamsHelper.ParamMask.EndTime, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "Type", _exportType, ExportParamsHelper.ParamMask.ExportType, converter);
			ConvertParameterTo(dictionary, "ExportType", _exportType, ExportParamsHelper.ParamMask.ExportType, converter);
			ConvertParameterTo(dictionary, "ExportId", _exportId, ExportParamsHelper.ParamMask.ExportId);
			ConvertParameterTo(dictionary, "CodecName", _codecName, ExportParamsHelper.ParamMask.CodecName);
			ConvertParameterTo(dictionary, "Name", _exportName, ExportParamsHelper.ParamMask.ExportName);
			ConvertParameterTo(dictionary, "KeepOriginals", _keepOriginals, ExportParamsHelper.ParamMask.KeepOriginals, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "InvestigationId", _investigationId, ExportParamsHelper.ParamMask.InvestigationId);
			ConvertParameterTo(dictionary, "ReExportFromId", _reExportFromId, ExportParamsHelper.ParamMask.ReExportFromId);
			ConvertParameterTo(dictionary, "IncludeAudio", _includeAudio, ExportParamsHelper.ParamMask.IncludeAudio, ParamsHelper.BoolConvertionTo);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
			foreach (CameraRelatedIds camera in _cameras)
			{
				tCommand.InputParams.Add(new Parameter("CameraId", camera.CameraId.ToString()));
				_paramMask |= 1u;
			}
			if (_subExportIds != null)
			{
				foreach (Guid subExportId in _subExportIds)
				{
					tCommand.InputParams.Add(new Parameter("SubExportId", subExportId.ToString()));
					_paramMask |= 1024u;
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ExportParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public bool CheckForMask(ExportParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}

	public ErrorCodes CheckExportParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, ParamMask);
	}

	public void CorrectParams()
	{
		DateTime value = DateTime.UtcNow.AddSeconds(-1.0);
		SetMemberMaxValue(ExportParamsHelper.ParamMask.StartTime, ref _startTime, value);
		SetMemberMaxValue(ExportParamsHelper.ParamMask.EndTime, ref _endTime, value);
		if (CheckForMask(ExportParamsHelper.ParamMask.EndTime))
		{
			SetMemberMaxValue(ExportParamsHelper.ParamMask.StartTime, ref _startTime, _endTime);
		}
		if (CheckForMask(ExportParamsHelper.ParamMask.ExportName))
		{
			_exportName = _exportName.ReplaceXssAttackChars();
			_exportName = _exportName.ReplaceUrlReservedChars();
		}
	}

	private void SetMemberMaxValue<T>(ExportParamsHelper.ParamMask mask, ref T member, T value) where T : IComparable<T>
	{
		if (CheckForMask(mask) && member.CompareTo(value) > 0)
		{
			member = value;
		}
	}

	public void GenerateName(string cameraName)
	{
		if (!CheckForMask(ExportParamsHelper.ParamMask.ExportName))
		{
			string text = "";
			string text2 = "";
			if (CheckForMask(ExportParamsHelper.ParamMask.StartTime))
			{
				text = StartTime.ToLocalTime().ToString(DateTimeFormat);
				if (CheckForMask(ExportParamsHelper.ParamMask.EndTime))
				{
					TimeSpan timeSpan = EndTime - StartTime;
					if (timeSpan.TotalSeconds > 0.0)
					{
						string text3 = "";
						text3 = ((timeSpan.Days > 0) ? "dd\\dhh\\hmm\\mss\\s" : ((timeSpan.Hours > 0) ? "hh\\hmm\\mss\\s" : ((timeSpan.Minutes <= 0) ? "ss\\s" : "mm\\mss\\s")));
						text2 = $"{'-'}{timeSpan.ToString(text3)}";
					}
				}
			}
			int maxLength = 80 - text.Length - text2.Length;
			ExportName = $"{ShortName(cameraName, maxLength)}{text}{text2}.{Type.ToString().ToLower()}";
		}
		else if (!ExportName.EndsWith(Type.ToString().ToLower()))
		{
			ExportName = $"{ShortName(ExportName, 80)}.{Type.ToString().ToLower()}";
		}
		else
		{
			string name = ExportName.Substring(0, ExportName.LastIndexOf(Type.ToString().ToLower()));
			ExportName = $"{ShortName(name, 80)}.{Type.ToString().ToLower()}";
		}
	}

	private string ShortName(string name, int maxLength)
	{
		string result = name;
		if (maxLength <= 0)
		{
			result = string.Empty;
		}
		if (!string.IsNullOrEmpty(name) && name.Length > maxLength)
		{
			result = name.Substring(0, maxLength);
		}
		return result;
	}
}
