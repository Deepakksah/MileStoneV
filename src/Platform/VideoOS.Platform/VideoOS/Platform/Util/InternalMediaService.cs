using System;
using VideoOS.Platform.Util.AdaptiveStreaming;

namespace VideoOS.Platform.Util;

public class InternalMediaService
{
	public class MulticastConfigDoneEventArgs : EventArgs
	{
		public Guid DeviceId { get; set; }

		public MulticastConfigDoneEventArgs(Guid deviceId)
		{
			DeviceId = deviceId;
		}
	}

	public class RawConnectParams
	{
		public FQID DeviceFqid { get; set; }

		public Guid StreamId { get; set; } = Guid.Empty;

		public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromMilliseconds(-1.0);

		public bool SendInitialImage { get; set; } = true;
	}

	public class VideoConnectParams : RawConnectParams
	{
		public bool EnableMulticast { get; set; }
	}

	public class LiveVideoRawConnectParams : VideoConnectParams
	{
		public string Media { get; set; }

		public bool RequestSignatures { get; set; }
	}

	public class TranscodedVideoConnectParams : VideoConnectParams
	{
		public int Width { get; set; }

		public int Height { get; set; }

		public bool KeepAspectRatio { get; set; }

		public bool FillSpace { get; set; }

		public bool KeyFramesOnly { get; set; }
	}

	public class LiveVideoConnectParams : TranscodedVideoConnectParams
	{
		public double Fps { get; set; }

		public bool AllowUpscaling { get; set; }

		public string HardwareDecodingMode { get; set; }

		public LiveVideoConnectParams()
		{
			HardwareDecodingMode = "Auto";
		}
	}

	public class LiveVideoConnectBitmapParams : TranscodedVideoConnectParams
	{
		public BitmapFormat BitmapFormat { get; set; }

		public double Fps { get; set; }
	}

	public class PcmAudioConnectParams : RawConnectParams
	{
		public uint Channels { get; set; }

		public uint SampleRate { get; set; }

		public uint BitsPerSample { get; set; }

		public PcmAudioConnectParams()
		{
			Channels = 1u;
			SampleRate = 8000u;
			BitsPerSample = 16u;
		}

		public PcmAudioConnectParams(PcmSourceSettings settings)
		{
			Channels = (uint)settings.Channels;
			SampleRate = settings.SamplingRate;
			BitsPerSample = (uint)settings.BitsPerSample;
		}
	}

	private static InternalMediaService _instance = new InternalMediaService();

	public static InternalMediaService Instance
	{
		get
		{
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public virtual bool Initialized => false;

	public event EventHandler<MulticastConfigDoneEventArgs> MulticastConfigDoneEvent = delegate
	{
	};

	public virtual object LiveVideoRawConnect(FQID deviceFQID, Guid streamId, string media, bool requestSignatures)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object LiveVideoRawConnect(FQID deviceFQID, Guid streamId, string media, bool requestSignatures, bool enableMulticast)
	{
		return LiveVideoRawConnect(deviceFQID, streamId, media, requestSignatures);
	}

	public virtual object LiveVideoRawConnect(FQID deviceFQID, Guid streamId, string media, bool requestSignatures, bool enableMulticast, TimeSpan connectTimeout)
	{
		return LiveVideoRawConnect(deviceFQID, streamId, media, requestSignatures, enableMulticast: false);
	}

	public virtual object LiveVideoRawConnect(LiveVideoRawConnectParams connectParams)
	{
		return LiveVideoRawConnect(connectParams.DeviceFqid, connectParams.StreamId, connectParams.Media, connectParams.RequestSignatures, connectParams.EnableMulticast, connectParams.ConnectTimeout);
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace)
	{
		return Instance.LiveVideoConnect(deviceFQID, width, height);
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, bool keyFramesOnly)
	{
		return Instance.LiveVideoConnect(deviceFQID, width, height, keepAspectRatio, fillSpace);
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, bool keyFramesOnly, Guid streamId)
	{
		return Instance.LiveVideoConnect(deviceFQID, width, height, keepAspectRatio, fillSpace);
	}

	public virtual object LiveVideoConnect(LiveVideoConnectParams parameters)
	{
		return Instance.LiveVideoConnect(parameters.DeviceFqid, parameters.Width, parameters.Height, parameters.KeepAspectRatio, parameters.FillSpace, parameters.KeyFramesOnly, parameters.StreamId);
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, BitmapFormat bitmapFormat)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat)
	{
		return Instance.LiveVideoConnect(deviceFQID, width, height, bitmapFormat);
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat, bool keyFramesOnly)
	{
		return Instance.LiveVideoConnect(deviceFQID, width, height, keepAspectRatio, fillSpace, bitmapFormat);
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat, bool keyFramesOnly, Guid streamId)
	{
		return Instance.LiveVideoConnect(deviceFQID, width, height, keepAspectRatio, fillSpace, bitmapFormat);
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat, bool keyFramesOnly, Guid streamId, bool enableMulticast)
	{
		return Instance.LiveVideoConnect(deviceFQID, width, height, keepAspectRatio, fillSpace, bitmapFormat);
	}

	public virtual object LiveVideoConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat, bool keyFramesOnly, Guid streamId, bool enableMulticast, TimeSpan connectTimeout)
	{
		return Instance.LiveVideoConnect(deviceFQID, width, height, keepAspectRatio, fillSpace, bitmapFormat, keyFramesOnly, streamId, enableMulticast);
	}

	public virtual object LiveVideoConnect(LiveVideoConnectBitmapParams connectParams)
	{
		return Instance.LiveVideoConnect(connectParams.DeviceFqid, connectParams.Width, connectParams.Height, connectParams.KeepAspectRatio, connectParams.FillSpace, connectParams.BitmapFormat, connectParams.KeyFramesOnly, connectParams.StreamId, connectParams.EnableMulticast, connectParams.ConnectTimeout);
	}

	public virtual object LiveAudioConnect(PcmAudioConnectParams connectParameters)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object LiveGet(object sessionId, TimeSpan waitTime)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object LiveGet(object sessionId, TimeSpan waitTime, bool rawData)
	{
		return LiveGet(sessionId, waitTime);
	}

	public virtual void LiveDisconnect(object sessionId)
	{
		HandleNotImplemented();
	}

	public virtual void SetLiveModeStart(object sessionId, bool start)
	{
		HandleNotImplemented();
	}

	public virtual void SetLiveModeStart(object sessionId, bool start, IStreamSelectionParams streamSelectionParams)
	{
		SetLiveModeStart(sessionId, start);
	}

	public virtual void SetNewToken(object sessionId, string token)
	{
		HandleNotImplemented();
	}

	public virtual void SetRemovableMasksLifted(object sessionId, bool lifted)
	{
	}

	public virtual void SetFps(object sessionId, double fps)
	{
	}

	public virtual void SetWidthHeight(object sessionId, int width, int height)
	{
		HandleNotImplemented();
	}

	public virtual void SetWidthHeight(object sessionId, int width, int height, bool allowUpscaling)
	{
		SetWidthHeight(sessionId, width, height);
	}

	public virtual void SetCropRectangle(object sessionId, double x, double y, double width, double height)
	{
		HandleNotImplemented();
	}

	public virtual void SetQuality(object sessionId, int quality)
	{
		HandleNotImplemented();
	}

	public virtual void SetDecoderWidthHeight(object sessionId, int width, int height)
	{
	}

	public virtual void SetPcmParameters(object sessionId, uint channels, uint samplingRate, uint bitsPerSample)
	{
		HandleNotImplemented();
	}

	public virtual void MarkSessionForNoReuse(object sessionId)
	{
	}

	public virtual bool SetupMulticast(FQID deviceFqid)
	{
		return false;
	}

	protected void FireMulticastConfigDone(Guid deviceId)
	{
		this.MulticastConfigDoneEvent(this, new MulticastConfigDoneEventArgs(deviceId));
	}

	private static void HandleNotImplemented()
	{
		if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Standalone)
		{
			EnvironmentManager.Instance.Log(error: true, "TryLoad", "VideoOS.Platform.SDK.Media.Environment.Initialize() not called.", null);
			throw new MIPException("VideoOS.Platform.SDK.Media.Environment.Initialize() not called.");
		}
		throw new MIPException("Method not supported for this Environment");
	}

	public virtual void SetStreamSelectionParams(object sessionId, IStreamSelectionParams streamSelectionParams)
	{
		EnvironmentManager.Instance.Log(error: true, "SetStreamSelectionParams", "SetStreamSelectionParams() not implemented in this environment", null);
	}
}
