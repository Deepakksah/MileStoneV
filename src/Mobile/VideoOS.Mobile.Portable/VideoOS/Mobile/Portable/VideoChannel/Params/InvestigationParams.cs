using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class InvestigationParams
{
	private uint _paramMask;

	private Guid _investigationId;

	private DateTime _startTime;

	private DateTime _endTime;

	private InvestigationState _state;

	private string _name;

	private readonly List<Guid> _cameras = new List<Guid>();

	private bool _includePackages;

	public Guid InvestigationId
	{
		get
		{
			return _investigationId;
		}
		set
		{
			_investigationId = value;
			_paramMask |= 1u;
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

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			_paramMask |= 8u;
		}
	}

	public InvestigationState State
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			_paramMask |= 16u;
		}
	}

	public IEnumerable<Guid> Cameras => _cameras;

	public bool IncludePackages
	{
		get
		{
			return _includePackages;
		}
		set
		{
			_includePackages = value;
			_paramMask |= 64u;
		}
	}

	public uint ParamMask => _paramMask;

	public void SetCameras(IEnumerable<Guid> camerasGuids)
	{
		_cameras.Clear();
		_cameras.AddRange(camerasGuids);
		_paramMask |= 32u;
	}

	public InvestigationParams()
	{
		InternalInit();
	}

	public InvestigationParams(Command tCommand, uint unMaskToCheck = 127u)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_investigationId = Guid.Empty;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, InvestigationParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public InvestigationParams InitFromCommand(Command tCommand, uint unMaskToCheck = 127u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "ItemId", ref _investigationId, InvestigationParamsHelper.ParamMask.InvestigationId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "StartTime", ref _startTime, InvestigationParamsHelper.ParamMask.StartTime, TimeConverter.FromString, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "EndTime", ref _endTime, InvestigationParamsHelper.ParamMask.EndTime, TimeConverter.FromString, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Name", ref _name, InvestigationParamsHelper.ParamMask.Name, ParamsHelper.StringConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "State", ref _state, InvestigationParamsHelper.ParamMask.State, EnumsHelper.EnumConverter<InvestigationState>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "IncludePackages", ref _includePackages, InvestigationParamsHelper.ParamMask.IncludePackages, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			Guid result = Guid.Empty;
			foreach (Parameter inputParam in tCommand.InputParams)
			{
				if (string.CompareOrdinal(inputParam.ParamName, "CameraId") == 0 && Guid.TryParse(inputParam.ParamValue, out result))
				{
					_cameras.Add(result);
					_paramMask |= 32u;
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "InvestigationParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, InvestigationParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public InvestigationParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			ParamsHelper.ConvToString<InvestigationState> converter = (InvestigationState x) => EnumsHelper.GetDescription(x);
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "ItemId", _investigationId, InvestigationParamsHelper.ParamMask.InvestigationId);
			ConvertParameterTo(dictionary, "StartTime", _startTime, InvestigationParamsHelper.ParamMask.StartTime, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "EndTime", _endTime, InvestigationParamsHelper.ParamMask.EndTime, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "Name", _name, InvestigationParamsHelper.ParamMask.Name);
			ConvertParameterTo(dictionary, "State", _state, InvestigationParamsHelper.ParamMask.State, converter);
			ConvertParameterTo(dictionary, "IncludePackages", _includePackages, InvestigationParamsHelper.ParamMask.IncludePackages, ParamsHelper.BoolConvertionTo);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
			foreach (Guid camera in _cameras)
			{
				tCommand.InputParams.Add(new Parameter("CameraId", camera.ToString()));
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "InvestigationParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public bool CheckForMask(InvestigationParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}

	public ErrorCodes CheckInvestigationParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, ParamMask);
	}

	public void InitFromEntry(IInvestigationEntry entry)
	{
		Name = entry.Name;
		StartTime = entry.StartTime;
		EndTime = entry.EndTime;
		State = entry.State;
		List<Guid> cameras = entry.Cameras.Select((IInvestigationCamera camera) => camera.CameraId).ToList();
		SetCameras(cameras);
		if (entry.Id != Guid.Empty)
		{
			InvestigationId = entry.Id;
		}
	}

	public void CorrectParams()
	{
		DateTime value = DateTime.UtcNow.AddSeconds(-1.0);
		SetMemberMaxValue(InvestigationParamsHelper.ParamMask.StartTime, ref _startTime, value);
		SetMemberMaxValue(InvestigationParamsHelper.ParamMask.EndTime, ref _endTime, value);
		if (CheckForMask(InvestigationParamsHelper.ParamMask.EndTime))
		{
			SetMemberMaxValue(InvestigationParamsHelper.ParamMask.StartTime, ref _startTime, _endTime);
		}
		if (CheckForMask(InvestigationParamsHelper.ParamMask.Name))
		{
			_name = _name.ReplaceXssAttackChars();
		}
	}

	private void SetMemberMaxValue<T>(InvestigationParamsHelper.ParamMask mask, ref T member, T value) where T : IComparable<T>
	{
		if (CheckForMask(mask) && member.CompareTo(value) > 0)
		{
			member = value;
		}
	}
}
