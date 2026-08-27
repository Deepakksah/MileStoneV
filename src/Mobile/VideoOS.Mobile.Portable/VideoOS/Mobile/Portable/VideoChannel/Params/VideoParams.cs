using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.Utilities.Rects;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class VideoParams : StreamParams, IComparable, IComparable<VideoParams>
{
	protected int _destWidth;

	protected int _destHeight;

	protected double _fps;

	protected IntRect _srcRect;

	protected uint _resamplingTag;

	protected int _srcStrideX;

	protected int _srcStrideY;

	protected bool _keyFramesOnly;

	protected bool _requestPrevNextSyncTime;

	protected bool _requestMotionAndSequenceId;

	protected bool _requestSignatures;

	protected bool _requiresDownsampling;

	protected bool _requestSize;

	protected TimeSpan _fragmentDuration;

	public Guid CameraId
	{
		get
		{
			return base.ItemId;
		}
		set
		{
			base.ItemId = value;
		}
	}

	public int DestWidth
	{
		get
		{
			return _destWidth;
		}
		set
		{
			_destWidth = value;
			_paramMask |= 1024u;
		}
	}

	public int DestHeight
	{
		get
		{
			return _destHeight;
		}
		set
		{
			_destHeight = value;
			_paramMask |= 2048u;
		}
	}

	public double FPS
	{
		get
		{
			return _fps;
		}
		set
		{
			_fps = value;
			_paramMask |= 4096u;
		}
	}

	public IntRect SrcRect
	{
		get
		{
			return _srcRect;
		}
		set
		{
			_srcRect = value;
			_paramMask |= 122880u;
		}
	}

	public uint ResamplingTag
	{
		get
		{
			return _resamplingTag;
		}
		set
		{
			_resamplingTag = value;
			_paramMask |= 524288u;
		}
	}

	public int SrcStrideX
	{
		get
		{
			return _srcStrideX;
		}
		set
		{
			_srcStrideX = value;
			_paramMask |= 131072u;
		}
	}

	public int SrcStrideY
	{
		get
		{
			return _srcStrideY;
		}
		set
		{
			_srcStrideY = value;
			_paramMask |= 262144u;
		}
	}

	public bool KeyFramesOnly
	{
		get
		{
			return _keyFramesOnly;
		}
		set
		{
			_keyFramesOnly = value;
			_paramMask |= 1048576u;
		}
	}

	public bool RequestPrevNextSyncTime
	{
		get
		{
			return _requestPrevNextSyncTime;
		}
		set
		{
			_requestPrevNextSyncTime = value;
			_paramMask |= 2097152u;
		}
	}

	public bool RequestMotionAndSequenceId
	{
		get
		{
			return _requestMotionAndSequenceId;
		}
		set
		{
			_requestMotionAndSequenceId = value;
			_paramMask |= 4194304u;
		}
	}

	public bool RequestSignatures
	{
		get
		{
			return _requestSignatures;
		}
		set
		{
			_requestSignatures = value;
			_paramMask |= 8388608u;
		}
	}

	public bool RequestSize
	{
		get
		{
			return _requestSize;
		}
		set
		{
			_requestSize = value;
			_paramMask |= 16777216u;
		}
	}

	public bool RequiresDownsampling
	{
		get
		{
			return _requiresDownsampling;
		}
		set
		{
			_requiresDownsampling = value;
			_paramMask |= 67108864u;
		}
	}

	public bool CloseOldControllers
	{
		get
		{
			return _closeOldControllers;
		}
		set
		{
			_closeOldControllers = value;
			_paramMask |= 134217728u;
		}
	}

	public TimeSpan FragmentDuration
	{
		get
		{
			return _fragmentDuration;
		}
		set
		{
			_fragmentDuration = value;
			_paramMask |= 268435456u;
		}
	}

	static VideoParams()
	{
		StreamParams.StreamIdUsed = StreamParamsHelper.StreamIdUsed.VideoId;
	}

	public ErrorCodes ValidateParams()
	{
		if (_destWidth < 0 || _destHeight < 0 || _fps < 0.0 || _srcStrideX < 0 || _srcStrideY < 0 || !_srcRect.ValidateRectParams() || (CheckForMask(StreamParamsHelper.StreamParamMask.StreamType) && _streamType == StreamParamsHelper.StreamType.Unknown))
		{
			return ErrorCodes.WrongInputParameter;
		}
		return ErrorCodes.Ok;
	}

	public override void InitFromCommand(Command tCommand, uint unMaskToCheck = 1023u, bool bRequest = true)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			base.InitFromCommand(tCommand, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "DestWidth", ref _destWidth, 1024u, Convert.ToInt32, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "DestHeight", ref _destHeight, 2048u, Convert.ToInt32, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "Fps", ref _fps, 4096u, ParamsHelper.DoubleConvertion, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "SrcWidth", ref _srcStrideX, 131072u, Convert.ToInt32, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "SrcHeight", ref _srcStrideY, 262144u, Convert.ToInt32, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "KeyFramesOnly", ref _keyFramesOnly, 1048576u, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "RequestPrevNextSyncTime", ref _requestPrevNextSyncTime, 2097152u, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "RequestMotionAndSequenceId", ref _requestMotionAndSequenceId, 4194304u, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "RequestSignatures", ref _requestSignatures, 8388608u, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "RequestSize", ref _requestSize, 16777216u, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "UserInitiatedDownsampling", ref _requiresDownsampling, 67108864u, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
			int tOutput = 0;
			int tOutput2 = 0;
			int tOutput3 = 0;
			int tOutput4 = 0;
			ConvertParameter(dictionary, "SrcLeft", ref tOutput, 8192u, Convert.ToInt32, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "SrcTop", ref tOutput2, 16384u, Convert.ToInt32, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "SrcRight", ref tOutput3, 32768u, Convert.ToInt32, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "SrcBottom", ref tOutput4, 65536u, Convert.ToInt32, unMaskToCheck, bLog: false);
			if (CheckForMask(StreamParamsHelper.VideoParamMask.SrcRect))
			{
				SrcRect = new IntRect(tOutput, tOutput2, tOutput3, tOutput4);
			}
			ConvertParameter(dictionary, "CloseOldControllers", ref _closeOldControllers, 134217728u, ParamsHelper.BoolConvertion, unMaskToCheck, bRequest);
			ConvertParameter(dictionary, "FragmentDurationMs", ref _fragmentDuration, 268435456u, ParamsHelper.TimeSpanConvertion, unMaskToCheck, bRequest);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "VideoParams.InitFromDict", ex.Message);
		}
	}

	public bool CheckForMask(StreamParamsHelper.VideoParamMask tMask)
	{
		return (base.ParamMask & (uint)tMask) == (uint)tMask;
	}

	public override void ConvertToCommand(ref Command tCommand)
	{
		try
		{
			base.ConvertToCommand(ref tCommand);
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "DestWidth", _destWidth, 1024u);
			ConvertParameterTo(dictionary, "DestHeight", _destHeight, 2048u);
			ConvertParameterTo(dictionary, "Fps", _fps, 4096u, ParamsHelper.DoubleConvertionTo);
			ConvertParameterTo(dictionary, "SrcWidth", _srcStrideX, 131072u);
			ConvertParameterTo(dictionary, "SrcHeight", _srcStrideY, 262144u);
			ConvertParameterTo(dictionary, "KeyFramesOnly", _keyFramesOnly, 1048576u, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "RequestPrevNextSyncTime", _requestPrevNextSyncTime, 2097152u, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "RequestMotionAndSequenceId", _requestMotionAndSequenceId, 4194304u, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "RequestSignatures", _requestSignatures, 8388608u, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "RequestSize", _requestSize, 16777216u, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "UserInitiatedDownsampling", _requiresDownsampling, 67108864u, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SrcLeft", _srcRect.Left, 8192u);
			ConvertParameterTo(dictionary, "SrcTop", _srcRect.Top, 16384u);
			ConvertParameterTo(dictionary, "SrcRight", _srcRect.Right, 32768u);
			ConvertParameterTo(dictionary, "SrcBottom", _srcRect.Bottom, 65536u);
			ConvertParameterTo(dictionary, "CloseOldControllers", _closeOldControllers, 134217728u, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "FragmentDurationMs", _fragmentDuration, 268435456u, ParamsHelper.TimeSpanConvertionTo);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "VideoParams.ConvertToCommand", ex.Message);
		}
	}

	public override void InitFromAnother(StreamParams tAnother)
	{
		if (tAnother is VideoParams tAnother2)
		{
			InitFromAnother(tAnother2);
		}
		else
		{
			base.InitFromAnother(tAnother);
		}
	}

	public void InitFromAnother(VideoParams tAnother)
	{
		base.InitFromAnother(tAnother);
		tAnother.SetField(ref _destWidth, ref tAnother._destWidth, 1024u, ref _paramMask);
		tAnother.SetField(ref _destHeight, ref tAnother._destHeight, 2048u, ref _paramMask);
		tAnother.SetField(ref _fps, ref tAnother._fps, 4096u, ref _paramMask);
		tAnother.SetField(ref _keyFramesOnly, ref tAnother._keyFramesOnly, 1048576u, ref _paramMask);
		tAnother.SetField(ref _requestPrevNextSyncTime, ref tAnother._requestPrevNextSyncTime, 2097152u, ref _paramMask);
		tAnother.SetField(ref _requestMotionAndSequenceId, ref tAnother._requestMotionAndSequenceId, 4194304u, ref _paramMask);
		tAnother.SetField(ref _requestSignatures, ref tAnother._requestSignatures, 8388608u, ref _paramMask);
		tAnother.SetField(ref _requestSize, ref tAnother._requestSize, 16777216u, ref _paramMask);
		tAnother.SetField(ref _requiresDownsampling, ref tAnother._requiresDownsampling, 67108864u, ref _paramMask);
		tAnother.SetField(ref _srcStrideX, ref tAnother._srcStrideX, 131072u, ref _paramMask);
		tAnother.SetField(ref _srcStrideY, ref tAnother._srcStrideY, 262144u, ref _paramMask);
		tAnother.SetField(ref _srcRect, ref tAnother._srcRect, 122880u, ref _paramMask, ParamsHelper.CopyRects);
		tAnother.SetField(ref _closeOldControllers, ref tAnother._closeOldControllers, 134217728u, ref _paramMask);
		tAnother.SetField(ref _fragmentDuration, ref tAnother._fragmentDuration, 268435456u, ref _paramMask);
		tAnother.SetField(ref _resamplingTag, ref tAnother._resamplingTag, 524288u, ref _paramMask);
	}

	protected override void InitInternal()
	{
		base.InitInternal();
		_srcRect = new IntRect();
		_resamplingTag = 1u;
		_fragmentDuration = TimeSpan.MaxValue;
	}

	public VideoParams()
	{
		InitInternal();
	}

	public VideoParams(Command tCommand, uint unMaskToCheck = 503316479u, bool bRequest = true)
		: base(tCommand, unMaskToCheck, bRequest)
	{
		InitInternal();
		InitFromCommand(tCommand, unMaskToCheck, bRequest);
	}

	public int CompareTo(VideoParams other, uint unMaskToCheck)
	{
		if (other == null)
		{
			return 1;
		}
		int nResult = CompareTo((StreamParams)other, unMaskToCheck);
		ParamsHelper.CompareCheck(ref nResult, DestWidth, other.DestWidth, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 1024u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, DestHeight, other.DestHeight, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 2048u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, FPS, other.FPS, base.DoubleChecker, unMaskToCheck, 4096u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, SrcRect, other.SrcRect, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 122880u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, ResamplingTag, other.ResamplingTag, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 524288u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, KeyFramesOnly, other.KeyFramesOnly, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 1048576u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, SrcStrideX, other.SrcStrideX, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 131072u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, SrcStrideY, other.SrcStrideY, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 262144u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, RequestMotionAndSequenceId, other.RequestMotionAndSequenceId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 4194304u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, RequestPrevNextSyncTime, other.RequestPrevNextSyncTime, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 2097152u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, RequestSignatures, other.RequestSignatures, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 8388608u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, RequestSize, other.RequestSize, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 16777216u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, RequiresDownsampling, other.RequiresDownsampling, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 67108864u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, CloseOldControllers, other.CloseOldControllers, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 134217728u);
		if (nResult != 0)
		{
			return nResult;
		}
		ParamsHelper.CompareCheck(ref nResult, FragmentDuration, other.FragmentDuration, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, 268435456u);
		return nResult;
	}

	public int CompareTo(VideoParams other)
	{
		return CompareTo(other, 503316479u);
	}

	public new int CompareTo(object other)
	{
		return CompareTo(other as VideoParams);
	}

	public override object Clone()
	{
		VideoParams obj = (VideoParams)MemberwiseClone();
		obj._srcRect = new IntRect(SrcRect.Left, SrcRect.Top, SrcRect.Right, SrcRect.Bottom);
		return obj;
	}
}
