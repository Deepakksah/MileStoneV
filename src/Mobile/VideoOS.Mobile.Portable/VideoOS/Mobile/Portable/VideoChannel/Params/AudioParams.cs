using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class AudioParams : StreamParams, IComparable, IComparable<AudioParams>
{
	protected StreamParamsHelper.AudioEncoding _audioEncoding;

	private uint _audioSamplingRate;

	private uint _audioBitsPerSample;

	private uint _audioChannelsNumber;

	public static Guid PushToAllId => Guid.Parse("{46B04F44-662A-4FCB-91C7-1CACDBFC37A0}");

	public static Guid PushToAllCameraRelatedId => Guid.Parse("{81E25EFE-206C-476A-8FD7-3709202AB439}");

	public StreamParamsHelper.AudioEncoding AudioEncoding
	{
		get
		{
			return _audioEncoding;
		}
		set
		{
			_audioEncoding = value;
			_paramMask |= 536870912u;
		}
	}

	public uint AudioSamplingRate
	{
		get
		{
			return _audioSamplingRate;
		}
		set
		{
			_audioSamplingRate = value;
			_paramMask |= 1073741824u;
		}
	}

	public uint AudioBitsPerSample
	{
		get
		{
			return _audioBitsPerSample;
		}
		set
		{
			_audioBitsPerSample = value;
			_paramMask |= 0u;
		}
	}

	public uint AudioChannelsNumber
	{
		get
		{
			return _audioChannelsNumber;
		}
		set
		{
			_audioChannelsNumber = value;
			_paramMask |= 2147483648u;
		}
	}

	public AudioParams()
	{
	}

	public AudioParams(Command tCommand, uint unMaskToCheck = 3758097407u, bool bRequest = true)
		: base(tCommand, unMaskToCheck, bRequest)
	{
	}

	public bool CheckForMask(StreamParamsHelper.AudioParamMask tMask)
	{
		return (base.ParamMask & (uint)tMask) == (uint)tMask;
	}

	public override void InitFromCommand(Command tCommand, uint unMaskToCheck = 1023u, bool bRequest = true)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			base.InitFromCommand(tCommand, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "AudioEncoding", ref _audioEncoding, 536870912u, EnumsHelper.EnumConverter<StreamParamsHelper.AudioEncoding>, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "AudioSamplingRate", ref _audioSamplingRate, 1073741824u, Convert.ToUInt32, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "AudioChannelsNumber", ref _audioChannelsNumber, 2147483648u, Convert.ToUInt32, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "AudioBitsPerSample", ref _audioBitsPerSample, 0u, Convert.ToUInt32, unMaskToCheck, bRequest);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "AudioParams.InitFromDict", ex.Message);
		}
	}

	public override void ConvertToCommand(ref Command tCommand)
	{
		try
		{
			base.ConvertToCommand(ref tCommand);
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			dictionary.Remove("CameraId");
			List<Guid> itemsIds = base.ItemsIds;
			if (itemsIds != null && itemsIds.Count > 0)
			{
				dictionary.Remove("ItemId");
			}
			ConvertParameterTo(dictionary, "AudioEncoding", _audioEncoding, 536870912u, (StreamParamsHelper.AudioEncoding x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "AudioChannelsNumber", _audioChannelsNumber, 2147483648u);
			ConvertParameterTo(dictionary, "AudioBitsPerSample", _audioBitsPerSample, 0u);
			ConvertParameterTo(dictionary, "AudioSamplingRate", _audioSamplingRate, 1073741824u);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
			foreach (Guid item in base.ItemsIds.EmptyIfNull())
			{
				tCommand.InputParams.Add(new Parameter("ItemId", item.ToString()));
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "AudioParams.ConvertToCommand", ex.Message);
		}
	}

	public override void InitFromAnother(StreamParams tAnother)
	{
		if (tAnother is AudioParams tAnother2)
		{
			InitFromAnother(tAnother2);
		}
		else
		{
			base.InitFromAnother(tAnother);
		}
	}

	public void InitFromAnother(AudioParams tAnother)
	{
		base.InitFromAnother(tAnother);
		tAnother.SetField(ref _audioEncoding, ref tAnother._audioEncoding, 536870912u, ref _paramMask);
		tAnother.SetField(ref _audioSamplingRate, ref tAnother._audioSamplingRate, 1073741824u, ref _paramMask);
		tAnother.SetField(ref _audioBitsPerSample, ref tAnother._audioBitsPerSample, 0u, ref _paramMask);
		tAnother.SetField(ref _audioChannelsNumber, ref tAnother._audioChannelsNumber, 2147483648u, ref _paramMask);
	}

	public int CompareTo(AudioParams other, uint unMaskToCheck)
	{
		if (other == null)
		{
			return 1;
		}
		int nResult = CompareTo((StreamParams)other, unMaskToCheck);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, (int)AudioEncoding, (int)other.AudioEncoding, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 536870912u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, AudioSamplingRate, other.AudioSamplingRate, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 1073741824u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, AudioBitsPerSample, other.AudioBitsPerSample, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 0u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, AudioChannelsNumber, other.AudioChannelsNumber, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 2147483648u);
		return nResult;
	}

	public new int CompareTo(object other)
	{
		return CompareTo(other as AudioParams);
	}

	public int CompareTo(AudioParams other)
	{
		return CompareTo(other, 3758097407u);
	}
}
