#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Platform.Common.Util;
using VideoOS.Platform.Live;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.AdaptiveStreaming;
using VideoOS.Platform.Util.ToolkitXmlProviders;
using VideoOS.Toolkit;

namespace VideoOS.Platform.Common;

public abstract class InternalMediaServiceCommon : InternalMediaService
{
	protected class MulticastConfiguration
	{
		public bool MulticastSupported;

		public string IpAddress;

		public int Port;

		public int Ssrc;

		public string PrivacyMask;

		public int PrivacyMaskVersion;
	}

	public class MediaSessionInfo
	{
		public string IsForkId;

		public string JpegForkId;

		public string DecodeAllForkId;

		public string DecodeKeyforkId;

		public string BitmapForkId;

		public FQID DeviceFQID;

		public long LastPrivacyMaskId;

		public string LastPrivacyMask;

		public bool Multicast;

		public MediaSessionInfo(FQID deviceFqid)
		{
			DeviceFQID = deviceFqid;
			IsForkId = null;
			JpegForkId = null;
			DecodeAllForkId = null;
			DecodeKeyforkId = null;
			BitmapForkId = null;
			LastPrivacyMaskId = PrivacyMaskProperty.UnknownId;
			LastPrivacyMask = null;
			Multicast = false;
		}
	}

	private readonly IToolkitFactory _factory = new ToolkitFactory();

	public readonly Dictionary<IToolkit, MediaSessionInfo> _mediaSessionInfos = new Dictionary<IToolkit, MediaSessionInfo>();

	public readonly Dictionary<string, int> _activeToolkitForkCounter = new Dictionary<string, int>();

	private readonly object _stateLock = new object();

	private readonly Dictionary<Guid, string> _cameraForkIdPostfix = new Dictionary<Guid, string>();

	private IDictionaryToolkit _toolkitDictionary;

	protected object _multicastConfigurationLock = new object();

	protected Dictionary<Guid, MulticastConfiguration> _multicastConfigurations = new Dictionary<Guid, MulticastConfiguration>();

	public override bool Initialized => true;

	public abstract string GetDecoderImplementation();

	public abstract string GetDecoderThreads(int activeStreams);

	public abstract string IsRemovableMasksLifted(FQID fqid);

	public abstract Guid OnLiveDisconnectMulticast(FQID fqid);

	protected void DisposeFactory()
	{
		_factory.Dispose();
	}

	private object LiveConnect(string configurationXml, MediaSessionInfo sessionInfo, ISet<string> properties, TimeSpan connectTimeout)
	{
		ILiveSourceToolkit liveSourceToolkit;
		lock (_stateLock)
		{
			liveSourceToolkit = _factory.CreateInstance(configurationXml) as ILiveSourceToolkit;
			if (liveSourceToolkit == null)
			{
				throw new MIPException("Unknown Toolkit, or missing DLLs");
			}
			_mediaSessionInfos.Add(liveSourceToolkit, sessionInfo);
		}
		try
		{
			liveSourceToolkit.Connect(properties, out var _, connectTimeout);
			return liveSourceToolkit;
		}
		catch (NotConnectedException ex)
		{
			LiveDisconnect(liveSourceToolkit);
			throw new CommunicationMIPException(ex.Message, ex);
		}
		catch (Exception ex2)
		{
			LiveDisconnect(liveSourceToolkit);
			if (EnvironmentManager.Instance != null)
			{
				EnvironmentManager.Instance.Log(error: false, "LiveConnect", ex2.Message, new object[1] { ex2 });
			}
			throw new MIPException(ex2.Message, ex2);
		}
	}

	private object LiveConnect(RawConnectParams parameters, Func<MediaSessionInfo, string, string> useForkConfXml, Func<MediaSessionInfo, Guid, string> configXml, ISet<string> properties, bool enableMulticast)
	{
		MediaSessionInfo mediaSessionInfo = new MediaSessionInfo(parameters.DeviceFqid);
		string configurationXml;
		if (UseFork() && useForkConfXml != null)
		{
			string arg = CreateMediaForkWithIsSource(parameters, mediaSessionInfo, enableMulticast);
			configurationXml = useForkConfXml(mediaSessionInfo, arg);
		}
		else
		{
			configurationXml = configXml(mediaSessionInfo, parameters.StreamId);
		}
		return LiveConnect(configurationXml, mediaSessionInfo, properties, parameters.ConnectTimeout);
	}

	public override object LiveVideoRawConnect(FQID deviceFqid, Guid streamId, string media, bool requestSignatures)
	{
		return LiveVideoRawConnect(deviceFqid, streamId, media, requestSignatures, enableMulticast: false);
	}

	public override object LiveVideoRawConnect(FQID deviceFqid, Guid streamId, string media, bool requestSignatures, bool enableMulticast)
	{
		return LiveVideoRawConnect(deviceFqid, streamId, media, requestSignatures, enableMulticast, TimeSpan.FromMilliseconds(-1.0));
	}

	public override object LiveVideoRawConnect(FQID deviceFqid, Guid streamId, string media, bool requestSignatures, bool enableMulticast, TimeSpan connectTimeout)
	{
		LiveVideoRawConnectParams connectParams = new LiveVideoRawConnectParams
		{
			DeviceFqid = deviceFqid,
			StreamId = streamId,
			Media = media,
			RequestSignatures = requestSignatures,
			EnableMulticast = enableMulticast,
			ConnectTimeout = connectTimeout
		};
		return LiveVideoRawConnect(connectParams);
	}

	public override object LiveVideoRawConnect(LiveVideoRawConnectParams parameters)
	{
		HashSet<string> hashSet = new HashSet<string> { "Privacy_Mask", "Has_Motion", "Sequence_Number" };
		if (parameters.RequestSignatures)
		{
			hashSet.Add("Recorded_Signatures");
		}
		return LiveConnect(parameters, (MediaSessionInfo sessionInfo, string forkId) => BuildToolkitBranchRawXml(forkId), (MediaSessionInfo sessionInfo, Guid strmId) => BuildIsSourceToolkitXml(parameters, sessionInfo, parameters.EnableMulticast), hashSet, parameters.EnableMulticast);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height)
	{
		return LiveVideoConnect(deviceFqid, width, height, keepAspectRatio: false, fillSpace: false);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, bool keepAspectRatio, bool fillSpace)
	{
		return LiveVideoConnect(deviceFqid, width, height, keepAspectRatio, fillSpace, keyFramesOnly: false);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, bool keepAspectRatio, bool fillSpace, bool keyFramesOnly)
	{
		return LiveVideoConnect(deviceFqid, width, height, keepAspectRatio, fillSpace, keyFramesOnly, Guid.Empty);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, bool keepAspectRatio, bool fillSpace, bool keyFramesOnly, Guid streamId)
	{
		LiveVideoConnectParams parameters = new LiveVideoConnectParams
		{
			DeviceFqid = deviceFqid,
			Width = width,
			Height = height,
			KeepAspectRatio = keepAspectRatio,
			FillSpace = fillSpace,
			KeyFramesOnly = keyFramesOnly,
			StreamId = streamId
		};
		return LiveVideoConnect(parameters);
	}

	public override object LiveVideoConnect(LiveVideoConnectParams parameters)
	{
		Func<MediaSessionInfo, string, string> useForkConfXml = delegate(MediaSessionInfo sessionInfo, string forkId)
		{
			string text = CreateMediaForkWithDecodeSource(parameters.DeviceFqid, forkId, sessionInfo, parameters.KeyFramesOnly);
			return (parameters.Width == 0 && parameters.Height == 0 && parameters.Fps == 0.0) ? BuildForkUseXml(CreateMediaForkWithJpegSource(text, sessionInfo)) : BuildForkToolkitJpegXml(text, parameters);
		};
		return LiveConnect(parameters, useForkConfXml, (MediaSessionInfo sessionInfo, Guid strmId) => BuildToolkitXml(parameters, sessionInfo), new HashSet<string> { "Decoder_Implementation" }, parameters.EnableMulticast);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, BitmapFormat bitmapFormat)
	{
		return LiveVideoConnect(deviceFqid, width, height, keepAspectRatio: false, fillSpace: false, bitmapFormat);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat)
	{
		return LiveVideoConnect(deviceFqid, width, height, keepAspectRatio, fillSpace, bitmapFormat, keyFramesOnly: false);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat, bool keyFramesOnly)
	{
		return LiveVideoConnect(deviceFqid, width, height, keepAspectRatio, fillSpace, bitmapFormat, keyFramesOnly, Guid.Empty);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat, bool keyFramesOnly, Guid streamId)
	{
		return LiveVideoConnect(deviceFqid, width, height, keepAspectRatio, fillSpace, bitmapFormat, keyFramesOnly, streamId, enableMulticast: false);
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat, bool keyFramesOnly, Guid streamId, bool enableMulticast)
	{
		return LiveVideoConnect(deviceFqid, width, height, keepAspectRatio, fillSpace, bitmapFormat, keyFramesOnly, streamId, enableMulticast, TimeSpan.FromMilliseconds(-1.0));
	}

	public override object LiveVideoConnect(FQID deviceFqid, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat bitmapFormat, bool keyFramesOnly, Guid streamId, bool enableMulticast, TimeSpan connectTimeout)
	{
		LiveVideoConnectBitmapParams connectParams = new LiveVideoConnectBitmapParams
		{
			DeviceFqid = deviceFqid,
			Width = width,
			Height = height,
			KeepAspectRatio = keepAspectRatio,
			FillSpace = fillSpace,
			BitmapFormat = bitmapFormat,
			KeyFramesOnly = keyFramesOnly,
			StreamId = streamId,
			EnableMulticast = enableMulticast,
			ConnectTimeout = connectTimeout
		};
		return LiveVideoConnect(connectParams);
	}

	public override object LiveVideoConnect(LiveVideoConnectBitmapParams parameters)
	{
		Func<MediaSessionInfo, string, string> useForkConfXml = delegate(MediaSessionInfo sessionInfo, string forkId)
		{
			if (parameters.Width == 0 && parameters.Height == 0 && parameters.BitmapFormat == BitmapFormat.BGR24 && parameters.Fps == 0.0)
			{
				string forkId2 = CreateMediaForkWithDecodeSource(parameters.DeviceFqid, forkId, sessionInfo, parameters.KeyFramesOnly);
				return BuildForkUseXml(CreateMediaForkWithBitmapSource(forkId2, sessionInfo));
			}
			return BuildForkToolkitBitmapXml(forkId, parameters);
		};
		Func<MediaSessionInfo, Guid, string> configXml = (MediaSessionInfo sessionInfo, Guid strmId) => BuildToolkitXmlBitmap(parameters, sessionInfo);
		return LiveConnect(parameters, useForkConfXml, configXml, new HashSet<string> { "Decoder_Implementation" }, parameters.EnableMulticast);
	}

	public void ChangeMulticastConfiguration(Guid objectId, string ipAddress, int port, int ssrc, string privacyMask, int privacyMaskVersion)
	{
		MulticastConfiguration multicastConfiguration = new MulticastConfiguration();
		if (ipAddress != string.Empty)
		{
			multicastConfiguration.MulticastSupported = true;
			multicastConfiguration.IpAddress = ipAddress;
			multicastConfiguration.Port = port;
			multicastConfiguration.Ssrc = ssrc;
			multicastConfiguration.PrivacyMask = privacyMask;
			multicastConfiguration.PrivacyMaskVersion = privacyMaskVersion;
		}
		else
		{
			multicastConfiguration.MulticastSupported = false;
			Trace.WriteLine("Multicast config not found");
		}
		lock (_multicastConfigurationLock)
		{
			_multicastConfigurations[objectId] = multicastConfiguration;
		}
	}

	public override object LiveAudioConnect(PcmAudioConnectParams connectParameters)
	{
		return LiveConnect(connectParameters, null, delegate
		{
			AudioDecoderXmlProvider audioDecoderXmlProvider = new AudioDecoderXmlProvider(connectParameters);
			audioDecoderXmlProvider.Chain(new IsSourceXmlProvider
			{
				Fqid = connectParameters.DeviceFqid,
				StreamId = connectParameters.StreamId
			});
			return audioDecoderXmlProvider.ConfigurationXml;
		}, new HashSet<string>(), enableMulticast: false);
	}

	public override void SetPcmParameters(object sessionId, uint channels, uint samplingRate, uint bitsPerSample)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit)
		{
			AudioDecoderXmlProvider audioDecoderXmlProvider = new AudioDecoderXmlProvider();
			audioDecoderXmlProvider.Channels = channels;
			audioDecoderXmlProvider.SamplingRate = samplingRate;
			audioDecoderXmlProvider.BitsPerSample = bitsPerSample;
			audioDecoderXmlProvider.UpdateConfiguration(liveSourceToolkit.UpdateConfiguration);
		}
	}

	public override object LiveGet(object sessionId, TimeSpan waitTime)
	{
		return LiveGet(sessionId, waitTime, rawData: false);
	}

	public override object LiveGet(object sessionId, TimeSpan waitTime, bool rawData)
	{
		if (!(sessionId is ILiveSourceToolkit liveSourceToolkit))
		{
			throw new MIPException("LiveGet sessionId is invalid");
		}
		try
		{
			IData data;
			switch (liveSourceToolkit.GetLiveData(out data, waitTime))
			{
			case ILiveSourceToolkit.GetLiveStatus.LIVE_REQUEST_TIMED_OUT:
				return new LiveSourceTimeout();
			default:
				return null;
			case ILiveSourceToolkit.GetLiveStatus.LIVE_DATA_RETRIEVED:
			{
				IMediaData mediaData = data as IMediaData;
				IOutOfBandData outOfBandData = data as IOutOfBandData;
				if (data is IImage image)
				{
					string hardwareDecodingStatus = "ipp";
					if (mediaData.GetProperty("Decoder_Implementation") is StringProperty stringProperty)
					{
						hardwareDecodingStatus = stringProperty.Value;
					}
					return new MediaLiveSourceBitmapContent(image)
					{
						BeginTime = data.BeginTimeStamp,
						EndTime = data.EndTimeStamp,
						Properties = new Dictionary<string, string>(),
						ObjectToDispose = data,
						HardwareDecodingStatus = hardwareDecodingStatus
					};
				}
				if (mediaData != null)
				{
					if (rawData)
					{
						MediaSessionInfo value;
						lock (_stateLock)
						{
							if (!_mediaSessionInfos.TryGetValue(liveSourceToolkit, out value))
							{
								throw new MIPException("LiveGet sessionId is not registered");
							}
						}
						byte[] data2 = mediaData.GetData();
						UpdatePrivacyMask(mediaData.GetProperty("Privacy_Mask") as IPrivacyMaskProperty, value);
						LiveSourceRawContent result = new LiveSourceRawContent
						{
							Content = data2,
							PrivacyMask = value.LastPrivacyMask,
							ObjectToDispose = null
						};
						data.Dispose();
						return result;
					}
					string hardwareDecodingStatus2 = "ipp";
					if (mediaData.GetProperty("Decoder_Implementation") is StringProperty stringProperty2)
					{
						hardwareDecodingStatus2 = stringProperty2.Value;
					}
					byte[] mediaBody = mediaData.GetMediaBody();
					LiveSourceContent result2 = new LiveSourceContent
					{
						BeginTime = data.BeginTimeStamp,
						EndTime = data.EndTimeStamp,
						Content = mediaBody,
						Properties = new Dictionary<string, string>(),
						HardwareDecodingStatus = hardwareDecodingStatus2
					};
					data.Dispose();
					return result2;
				}
				if (outOfBandData != null)
				{
					byte[] data3 = outOfBandData.GetData();
					LiveSourceStatus result3 = new LiveSourceStatus(Encoding.UTF8.GetString(data3));
					data.Dispose();
					return result3;
				}
				EnvironmentManager.Instance.Log(error: true, "InternalLiveVideo", "Unknown type returned from Toolkit GetLiveData(): " + data, null);
				data.Dispose();
				return null;
			}
			}
		}
		catch (NotConnectedException ex)
		{
			throw new CommunicationMIPException(ex.Message, ex);
		}
		catch (Exception ex2)
		{
			if (EnvironmentManager.Instance != null)
			{
				EnvironmentManager.Instance.Log(error: false, "LiveGet", ex2.Message, new object[1] { ex2 });
			}
			throw new MIPException(ex2.Message, ex2);
		}
	}

	private static void UpdatePrivacyMask(IPrivacyMaskProperty privacyMaskProperty, MediaSessionInfo sessionInfo)
	{
		if (privacyMaskProperty == null)
		{
			sessionInfo.LastPrivacyMask = null;
			sessionInfo.LastPrivacyMaskId = PrivacyMaskProperty.UnknownId;
		}
		else
		{
			if (sessionInfo.LastPrivacyMaskId != PrivacyMaskProperty.UnknownId && privacyMaskProperty.Id != PrivacyMaskProperty.UnknownId && sessionInfo.LastPrivacyMaskId == privacyMaskProperty.Id)
			{
				return;
			}
			int gridSize = (int)privacyMaskProperty.GridSize;
			if (gridSize == 0)
			{
				sessionInfo.LastPrivacyMask = null;
				sessionInfo.LastPrivacyMaskId = privacyMaskProperty.Id;
			}
			StringBuilder stringBuilder = new StringBuilder(gridSize * gridSize + gridSize + 5);
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}x{0}", gridSize);
			for (byte b = 0; b < gridSize; b++)
			{
				for (byte b2 = 0; b2 < gridSize; b2++)
				{
					PrivacyMaskCellData cellData = privacyMaskProperty.GetCellData(b, b2);
					stringBuilder.Append((cellData.Method == PrivacyMaskCellMethod.None) ? '0' : '1');
				}
			}
			sessionInfo.LastPrivacyMask = stringBuilder.ToString();
			sessionInfo.LastPrivacyMaskId = privacyMaskProperty.Id;
		}
	}

	public override void SetLiveModeStart(object sessionId, bool start)
	{
		SetLiveModeStart(sessionId, start, null);
	}

	public override void SetLiveModeStart(object sessionId, bool start, IStreamSelectionParams streamSelectionParams)
	{
		ILiveSourceToolkit liveSourceToolkit = sessionId as ILiveSourceToolkit;
		LivePlaybackSourceToolkit livePlaybackSourceToolkit = sessionId as LivePlaybackSourceToolkit;
		if (liveSourceToolkit == null)
		{
			throw new MIPException("SetLiveModeStart sessionId is invalid");
		}
		if (start)
		{
			if (livePlaybackSourceToolkit != null && streamSelectionParams != null)
			{
				livePlaybackSourceToolkit.StartLiveStream(streamSelectionParams.ToLiveStreamSelection());
			}
			else
			{
				liveSourceToolkit.StartLiveStream();
			}
		}
		else
		{
			liveSourceToolkit.StopLiveStream();
		}
	}

	public override void LiveDisconnect(object sessionId)
	{
		if (!(sessionId is ILiveSourceToolkit liveSourceToolkit))
		{
			return;
		}
		try
		{
			liveSourceToolkit.StopLiveStream();
			liveSourceToolkit.Disconnect();
		}
		catch
		{
		}
		Guid guid = Guid.Empty;
		lock (_stateLock)
		{
			_factory.DeleteInstance(liveSourceToolkit);
			if (_mediaSessionInfos.TryGetValue(liveSourceToolkit, out var value))
			{
				if (value.Multicast)
				{
					guid = OnLiveDisconnectMulticast(value.DeviceFQID);
				}
				UnregisterUseOfToolkit(value.JpegForkId);
				UnregisterUseOfToolkit(value.BitmapForkId);
				UnregisterUseOfToolkit(value.DecodeAllForkId);
				UnregisterUseOfToolkit(value.DecodeKeyforkId);
				UnregisterUseOfToolkit(value.IsForkId);
				_mediaSessionInfos.Remove(liveSourceToolkit);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: false, "Media LiveDisconnect", "Unable to free toolkit source, not found in dictionary");
			}
		}
		if (guid != Guid.Empty)
		{
			lock (_multicastConfigurationLock)
			{
				_multicastConfigurations.Remove(guid);
			}
		}
	}

	public override void SetNewToken(object sessionId, string token)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit)
		{
			liveSourceToolkit.UpdateConfiguration("token_key", token);
		}
	}

	public override void SetRemovableMasksLifted(object sessionId, bool lifted)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit)
		{
			liveSourceToolkit.UpdateConfiguration("removable_masks_lifted_key", lifted ? "true" : "false");
		}
	}

	public override void SetFps(object sessionId, double fps)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit)
		{
			if (fps == 0.0)
			{
				liveSourceToolkit.UpdateConfiguration("fps_enable_key", "false");
				return;
			}
			liveSourceToolkit.UpdateConfiguration("interval_key", CalculateFrameInterval(fps));
			liveSourceToolkit.UpdateConfiguration("fps_enable_key", "true");
		}
	}

	public override void SetWidthHeight(object sessionId, int width, int height)
	{
		SetWidthHeight(sessionId, width, height, allowUpscaling: true);
	}

	public override void SetWidthHeight(object sessionId, int width, int height, bool allowUpscaling)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit && width != 0 && height != 0)
		{
			liveSourceToolkit.UpdateConfiguration("width_key", width.ToString() ?? "");
			liveSourceToolkit.UpdateConfiguration("height_key", height.ToString() ?? "");
			liveSourceToolkit.UpdateConfiguration("allow_upscaling_key", allowUpscaling ? "true" : "false");
			liveSourceToolkit.UpdateConfiguration("decode_width_key", ConvertToDecoderDimension(width));
			liveSourceToolkit.UpdateConfiguration("decode_height_key", ConvertToDecoderDimension(height));
		}
	}

	public override void SetQuality(object sessionId, int quality)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit)
		{
			liveSourceToolkit.UpdateConfiguration("quality_key", quality.ToString() ?? "");
		}
	}

	public override void SetCropRectangle(object sessionId, double x, double y, double width, double height)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit)
		{
			if (!(x >= 0.0) || !(y >= 0.0) || !(width >= 0.0) || !(height >= 0.0) || !(x + width <= 1.0) || !(y + height <= 1.0))
			{
				throw new InvalidParameterMIPException("Crop Rectangle invalid: x=" + x + ", y=" + y + " width=" + width + ", height=" + height);
			}
			liveSourceToolkit.UpdateConfiguration("cropx_key", x.ToString("0.000000", CultureInfo.InvariantCulture));
			liveSourceToolkit.UpdateConfiguration("cropy_key", y.ToString("0.000000", CultureInfo.InvariantCulture));
			liveSourceToolkit.UpdateConfiguration("cropwidth_key", width.ToString("0.000000", CultureInfo.InvariantCulture));
			liveSourceToolkit.UpdateConfiguration("cropheight_key", height.ToString("0.000000", CultureInfo.InvariantCulture));
		}
	}

	public override void MarkSessionForNoReuse(object sessionId)
	{
		if (sessionId is ILiveSourceToolkit key && _mediaSessionInfos.TryGetValue(key, out var value))
		{
			string value2 = "";
			int result = 0;
			if (_cameraForkIdPostfix.TryGetValue(value.DeviceFQID.ObjectId, out value2))
			{
				int.TryParse(value2, out result);
			}
			result++;
			_cameraForkIdPostfix[value.DeviceFQID.ObjectId] = result.ToString() ?? "";
		}
	}

	public override void SetDecoderWidthHeight(object sessionId, int width, int height)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit)
		{
			liveSourceToolkit.UpdateConfiguration("decode_width_key", ConvertToDecoderDimension(width));
			liveSourceToolkit.UpdateConfiguration("decode_height_key", ConvertToDecoderDimension(height));
		}
	}

	public override void SetStreamSelectionParams(object sessionId, IStreamSelectionParams streamSelectionParams)
	{
		if (!(sessionId is LivePlaybackSourceToolkit livePlaybackSourceToolkit) || streamSelectionParams == null)
		{
			EnvironmentManager.Instance?.Log(error: false, "SetStreamSelectionParams", "Null parameter passed to the method");
		}
		else
		{
			livePlaybackSourceToolkit.UpdateLiveStreamSelection(streamSelectionParams.ToLiveStreamSelection());
		}
	}

	private static bool UseFork()
	{
		return MediaCommonUtility.IsToolkitForkEnabled();
	}

	public static string ConvertToMedia(FQID fqid)
	{
		return fqid.ToMedia();
	}

	private void RegisterUseOfToolkit(string toolkitForkName, string xml)
	{
		lock (_stateLock)
		{
			if (_activeToolkitForkCounter.ContainsKey(toolkitForkName))
			{
				_activeToolkitForkCounter[toolkitForkName]++;
				return;
			}
			_activeToolkitForkCounter.Add(toolkitForkName, 1);
			_toolkitDictionary.Add(toolkitForkName, xml);
		}
	}

	private void UnregisterUseOfToolkit(string toolkitForkName)
	{
		if (toolkitForkName == null)
		{
			return;
		}
		lock (_stateLock)
		{
			if (!_activeToolkitForkCounter.ContainsKey(toolkitForkName))
			{
				return;
			}
			_activeToolkitForkCounter[toolkitForkName]--;
			if (_activeToolkitForkCounter[toolkitForkName] <= 0)
			{
				try
				{
					_toolkitDictionary.Remove(toolkitForkName);
				}
				catch
				{
				}
				_activeToolkitForkCounter.Remove(toolkitForkName);
			}
		}
	}

	private string CreateMediaForkWithIsSource(RawConnectParams parameters, MediaSessionInfo sessionInfo, bool enableMulticast)
	{
		string value = "";
		_cameraForkIdPostfix.TryGetValue(parameters.DeviceFqid.ObjectId, out value);
		string text = parameters.DeviceFqid.ObjectId.ToString() + parameters.StreamId.ToString() + value;
		CheckAndCreateToolkitDictionary();
		string text2 = BuildIsSourceToolkitXml(parameters, sessionInfo, enableMulticast);
		string xml = "<toolkit type=\"source\"><provider>mfork</provider><config><max_queue_length>30</max_queue_length><buffer_current_gop/>" + text2 + "</config></toolkit>";
		RegisterUseOfToolkit(text, xml);
		sessionInfo.IsForkId = text;
		return text;
	}

	private string CreateMediaForkWithDecodeSource(FQID fqid, string forkId, MediaSessionInfo sessionInfo, bool keyFramesOnly)
	{
		string text = BuildForkToolkitDecodingXml(fqid, forkId, keyFramesOnly, 0, 0);
		string xml = "<toolkit type=\"source\"><provider>mfork</provider><config>" + text + "</config></toolkit>";
		string text2 = (keyFramesOnly ? "_DecodeKey" : "_DecodeAll");
		string text3 = forkId + text2;
		RegisterUseOfToolkit(text3, xml);
		sessionInfo.DecodeAllForkId = text3;
		return text3;
	}

	private string CreateMediaForkWithJpegSource(string forkId, MediaSessionInfo sessionInfo)
	{
		string text = BuildForkToolkitJpegXml(forkId);
		string xml = "<toolkit type=\"source\"><provider>mfork</provider><config>" + text + "</config></toolkit>";
		string text2 = forkId + "_Jpeg";
		RegisterUseOfToolkit(text2, xml);
		sessionInfo.JpegForkId = text2;
		return text2;
	}

	private string CreateMediaForkWithBitmapSource(string forkId, MediaSessionInfo sessionInfo)
	{
		string text = BuildForkToolkitBitmapOnDecoderXml(forkId);
		string xml = "<toolkit type=\"source\"><provider>mfork</provider><config>" + text + "</config></toolkit>";
		string text2 = forkId + "_Bitmap";
		RegisterUseOfToolkit(text2, xml);
		sessionInfo.BitmapForkId = text2;
		return text2;
	}

	private static string BuildForkUseXml(string forkId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			WriteToolkitBranch(xmlWriter, forkId);
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private static string ConvertToDecoderDimension(int dim)
	{
		if (dim != 0)
		{
			return dim.ToString(CultureInfo.InvariantCulture);
		}
		return "-1";
	}

	private string BuildToolkitXml(LiveVideoConnectParams parameters, MediaSessionInfo sessionInfo)
	{
		if (LoginSettingsCache.GetLoginSettings(parameters.DeviceFqid) == null)
		{
			throw new CommunicationMIPException("LoginSettings not found");
		}
		StringBuilder stringBuilder = new StringBuilder();
		using StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
		xmlTextWriter.WriteStartElement("toolkit");
		xmlTextWriter.WriteAttributeString("type", "source");
		xmlTextWriter.WriteElementString("provider", "mmp");
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteStartElement("jpeg_encoder");
		xmlTextWriter.WriteAttributeString("quality_update_key", "quality_key");
		WriteResizeTransformStart(xmlTextWriter, parameters, parameters.AllowUpscaling);
		xmlTextWriter.WriteStartElement("image_crop");
		WriteCropAttributes(xmlTextWriter);
		WriteSubsamplerElement(parameters.Fps, xmlTextWriter);
		xmlTextWriter.WriteStartElement(parameters.KeyFramesOnly ? "key_frames_only_video_decoder" : "video_decoder");
		xmlTextWriter.WriteAttributeString("normalize_pixel_aspect_ratio", "true");
		string decoderImplementation = GetDecoderImplementation();
		xmlTextWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
		xmlTextWriter.WriteAttributeString("number_of_threads", GetDecoderThreads(_mediaSessionInfos.Count));
		xmlTextWriter.WriteAttributeString("removable_masks_lifted", IsRemovableMasksLifted(parameters.DeviceFqid));
		xmlTextWriter.WriteAttributeString("removable_masks_lifted_update_key", "removable_masks_lifted_key");
		xmlTextWriter.WriteAttributeString("target_resolution_width", ConvertToDecoderDimension(parameters.Width));
		xmlTextWriter.WriteAttributeString("target_resolution_width_update_key", "decode_width_key");
		xmlTextWriter.WriteAttributeString("target_resolution_height", ConvertToDecoderDimension(parameters.Height));
		xmlTextWriter.WriteAttributeString("target_resolution_height_update_key", "decode_height_key");
		xmlTextWriter.WriteRaw(BuildIsSourceToolkitXml(parameters, sessionInfo, parameters.EnableMulticast));
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		WriteResizeTransformEnd(xmlTextWriter, parameters);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Flush();
		return stringBuilder.ToString();
	}

	private static string BuildForkToolkitJpegXml(string branchId, LiveVideoConnectParams parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
		xmlTextWriter.WriteStartElement("toolkit");
		xmlTextWriter.WriteAttributeString("type", "source");
		xmlTextWriter.WriteElementString("provider", "mmp");
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteStartElement("jpeg_encoder");
		xmlTextWriter.WriteAttributeString("quality_update_key", "quality_key");
		WriteResizeTransformStart(xmlTextWriter, parameters, parameters.AllowUpscaling);
		xmlTextWriter.WriteStartElement("image_crop");
		WriteCropAttributes(xmlTextWriter);
		WriteSubsamplerElement(parameters.Fps, xmlTextWriter);
		WriteToolkitBranch(xmlTextWriter, branchId);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		WriteResizeTransformEnd(xmlTextWriter, parameters);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Flush();
		return stringBuilder.ToString();
	}

	private static void WriteSubsamplerElement(double fps, XmlWriter xw)
	{
		xw.WriteStartElement("video_subsampler");
		xw.WriteAttributeString("interval_update_key", "interval_key");
		xw.WriteAttributeString("enable_update_key", "fps_enable_key");
		if (fps == 0.0)
		{
			xw.WriteAttributeString("interval", "250");
			xw.WriteAttributeString("enable", "false");
		}
		else
		{
			xw.WriteAttributeString("interval", CalculateFrameInterval(fps));
			xw.WriteAttributeString("enable", "true");
		}
	}

	private static string CalculateFrameInterval(double fps)
	{
		return Convert.ToInt32(1000.0 / fps).ToString(CultureInfo.InvariantCulture);
	}

	private static string BuildForkToolkitJpegXml(string branchId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "source");
			xmlWriter.WriteElementString("provider", "mmp");
			xmlWriter.WriteStartElement("config");
			xmlWriter.WriteStartElement("jpeg_encoder");
			WriteToolkitBranch(xmlWriter, branchId);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private string BuildToolkitXmlBitmap(LiveVideoConnectBitmapParams parameters, MediaSessionInfo sessionInfo)
	{
		if (LoginSettingsCache.GetLoginSettings(parameters.DeviceFqid) == null)
		{
			throw new CommunicationMIPException("LoginSettings not found");
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "source");
			xmlWriter.WriteElementString("provider", "mmp");
			xmlWriter.WriteStartElement("config");
			xmlWriter.WriteStartElement("image_color_format_convert");
			xmlWriter.WriteAttributeString("format", parameters.BitmapFormat.ToString());
			WriteResizeTransformStart(xmlWriter, parameters, allowUpscaling: true);
			xmlWriter.WriteStartElement("image_crop");
			WriteCropAttributes(xmlWriter);
			WriteSubsamplerElement(parameters.Fps, xmlWriter);
			xmlWriter.WriteStartElement(parameters.KeyFramesOnly ? "key_frames_only_video_decoder" : "video_decoder");
			xmlWriter.WriteAttributeString("normalize_pixel_aspect_ratio", "true");
			string decoderImplementation = GetDecoderImplementation();
			xmlWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
			xmlWriter.WriteAttributeString("number_of_threads", GetDecoderThreads(_mediaSessionInfos.Count));
			xmlWriter.WriteAttributeString("removable_masks_lifted", IsRemovableMasksLifted(parameters.DeviceFqid));
			xmlWriter.WriteAttributeString("removable_masks_lifted_update_key", "removable_masks_lifted_key");
			xmlWriter.WriteAttributeString("target_resolution_width", ConvertToDecoderDimension(parameters.Width));
			xmlWriter.WriteAttributeString("target_resolution_width_update_key", "decode_width_key");
			xmlWriter.WriteAttributeString("target_resolution_height", ConvertToDecoderDimension(parameters.Height));
			xmlWriter.WriteAttributeString("target_resolution_height_update_key", "decode_height_key");
			xmlWriter.WriteRaw(BuildIsSourceToolkitXml(parameters, sessionInfo, parameters.EnableMulticast));
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			WriteResizeTransformEnd(xmlWriter, parameters);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private string BuildForkToolkitBitmapXml(string branchId, LiveVideoConnectBitmapParams parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "source");
			xmlWriter.WriteElementString("provider", "mmp");
			xmlWriter.WriteStartElement("config");
			xmlWriter.WriteStartElement("image_color_format_convert");
			xmlWriter.WriteAttributeString("format", parameters.BitmapFormat.ToString());
			WriteResizeTransformStart(xmlWriter, parameters, allowUpscaling: true);
			xmlWriter.WriteStartElement("image_crop");
			WriteCropAttributes(xmlWriter);
			WriteSubsamplerElement(parameters.Fps, xmlWriter);
			xmlWriter.WriteStartElement(parameters.KeyFramesOnly ? "key_frames_only_video_decoder" : "video_decoder");
			xmlWriter.WriteAttributeString("normalize_pixel_aspect_ratio", "true");
			string decoderImplementation = GetDecoderImplementation();
			xmlWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
			xmlWriter.WriteAttributeString("number_of_threads", GetDecoderThreads(_mediaSessionInfos.Count));
			xmlWriter.WriteAttributeString("removable_masks_lifted", IsRemovableMasksLifted(parameters.DeviceFqid));
			xmlWriter.WriteAttributeString("removable_masks_lifted_update_key", "removable_masks_lifted_key");
			xmlWriter.WriteAttributeString("target_resolution_width", ConvertToDecoderDimension(parameters.Width));
			xmlWriter.WriteAttributeString("target_resolution_width_update_key", "decode_width_key");
			xmlWriter.WriteAttributeString("target_resolution_height", ConvertToDecoderDimension(parameters.Height));
			xmlWriter.WriteAttributeString("target_resolution_height_update_key", "decode_height_key");
			WriteToolkitBranch(xmlWriter, branchId);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			WriteResizeTransformEnd(xmlWriter, parameters);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private static string BuildForkToolkitBitmapOnDecoderXml(string branchId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "source");
			xmlWriter.WriteElementString("provider", "mmp");
			xmlWriter.WriteStartElement("config");
			xmlWriter.WriteStartElement("image_color_format_convert");
			xmlWriter.WriteAttributeString("format", BitmapFormat.BGR24.ToString());
			WriteToolkitBranch(xmlWriter, branchId);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private static void WriteResizeTransformStart(XmlWriter xw, TranscodedVideoConnectParams parameters, bool allowUpscaling)
	{
		int width = parameters.Width;
		int height = parameters.Height;
		bool keepAspectRatio = parameters.KeepAspectRatio;
		bool fillSpace = parameters.FillSpace;
		if (width != 0 && height != 0)
		{
			if (keepAspectRatio && fillSpace)
			{
				xw.WriteStartElement("image_filled_resize");
				xw.WriteAttributeString("width", width.ToString() ?? "");
				xw.WriteAttributeString("width_update_key", "width_key");
				xw.WriteAttributeString("height", height.ToString() ?? "");
				xw.WriteAttributeString("height_update_key", "height_key");
				xw.WriteAttributeString("allow_upscaling", allowUpscaling ? "true" : "false");
				xw.WriteAttributeString("allow_upscaling_update_key", "allow_upscaling_key");
				return;
			}
			xw.WriteStartElement("image_resize");
			xw.WriteAttributeString("width", width.ToString() ?? "");
			xw.WriteAttributeString("width_update_key", "width_key");
			xw.WriteAttributeString("height", height.ToString() ?? "");
			xw.WriteAttributeString("height_update_key", "height_key");
			xw.WriteAttributeString("allow_upscaling", allowUpscaling ? "true" : "false");
			xw.WriteAttributeString("allow_upscaling_update_key", "allow_upscaling_key");
			xw.WriteAttributeString("enable", keepAspectRatio ? "false" : "true");
			xw.WriteAttributeString("keep_pixel_aspect_ratio", "true");
			xw.WriteAttributeString("enable_update_key", "imageresize_key");
			xw.WriteStartElement("image_boxed_resize");
			xw.WriteAttributeString("width", width.ToString() ?? "");
			xw.WriteAttributeString("width_update_key", "width_key");
			xw.WriteAttributeString("height", height.ToString() ?? "");
			xw.WriteAttributeString("height_update_key", "height_key");
			xw.WriteAttributeString("allow_upscaling", allowUpscaling ? "true" : "false");
			xw.WriteAttributeString("allow_upscaling_update_key", "allow_upscaling_key");
			xw.WriteAttributeString("enable", keepAspectRatio ? "true" : "false");
			xw.WriteAttributeString("enable_update_key", "ibresize_key");
		}
	}

	private static void WriteResizeTransformEnd(XmlWriter xw, TranscodedVideoConnectParams parameters)
	{
		int width = parameters.Width;
		int height = parameters.Height;
		bool keepAspectRatio = parameters.KeepAspectRatio;
		bool fillSpace = parameters.FillSpace;
		if (width != 0 && height != 0)
		{
			if (keepAspectRatio && fillSpace)
			{
				xw.WriteEndElement();
				return;
			}
			xw.WriteEndElement();
			xw.WriteEndElement();
		}
	}

	private static void WriteCropAttributes(XmlWriter xw)
	{
		xw.WriteAttributeString("offset_x", "0.0");
		xw.WriteAttributeString("offset_x_update_key", "cropx_key");
		xw.WriteAttributeString("offset_y", "0.0");
		xw.WriteAttributeString("offset_y_update_key", "cropy_key");
		xw.WriteAttributeString("width", "1.0");
		xw.WriteAttributeString("width_update_key", "cropwidth_key");
		xw.WriteAttributeString("height", "1.0");
		xw.WriteAttributeString("height_update_key", "cropheight_key");
	}

	private static void WriteToolkitBranch(XmlWriter xw, string branchId)
	{
		xw.WriteStartElement("toolkit");
		xw.WriteAttributeString("type", "source");
		xw.WriteElementString("provider", "mfork");
		xw.WriteStartElement("config");
		xw.WriteStartElement("get_branch");
		xw.WriteElementString("on_overflow_skip_to_most_recent_gop", "");
		xw.WriteStartElement("forward_configuration_update");
		xw.WriteAttributeString("key", "token_key");
		xw.WriteEndElement();
		xw.WriteStartElement("forward_configuration_update");
		xw.WriteAttributeString("key", "removable_masks_lifted_key");
		xw.WriteEndElement();
		xw.WriteStartElement("forward_configuration_update");
		xw.WriteAttributeString("key", "decode_width_key");
		xw.WriteEndElement();
		xw.WriteStartElement("forward_configuration_update");
		xw.WriteAttributeString("key", "decode_height_key");
		xw.WriteEndElement();
		xw.WriteStartElement("toolkit");
		xw.WriteAttributeString("type", "generic");
		xw.WriteElementString("provider", "mtd");
		xw.WriteStartElement("config");
		xw.WriteStartElement("lookup");
		xw.WriteAttributeString("name", branchId);
		xw.WriteEndElement();
		xw.WriteEndElement();
		xw.WriteEndElement();
		xw.WriteEndElement();
		xw.WriteEndElement();
		xw.WriteEndElement();
	}

	private string BuildForkToolkitDecodingXml(FQID fqid, string branchId, bool keyFramesOnly, int width, int height)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "source");
			xmlWriter.WriteElementString("provider", "mmp");
			xmlWriter.WriteStartElement("config");
			xmlWriter.WriteStartElement(keyFramesOnly ? "key_frames_only_video_decoder" : "video_decoder");
			xmlWriter.WriteAttributeString("normalize_pixel_aspect_ratio", "true");
			string decoderImplementation = GetDecoderImplementation();
			xmlWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
			xmlWriter.WriteAttributeString("number_of_threads", GetDecoderThreads(_mediaSessionInfos.Count));
			xmlWriter.WriteAttributeString("removable_masks_lifted", IsRemovableMasksLifted(fqid));
			xmlWriter.WriteAttributeString("removable_masks_lifted_update_key", "removable_masks_lifted_key");
			xmlWriter.WriteAttributeString("target_resolution_width", ConvertToDecoderDimension(width));
			xmlWriter.WriteAttributeString("target_resolution_width_update_key", "decode_width_key");
			xmlWriter.WriteAttributeString("target_resolution_height", ConvertToDecoderDimension(height));
			xmlWriter.WriteAttributeString("target_resolution_height_update_key", "decode_height_key");
			WriteToolkitBranch(xmlWriter, branchId);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private static string BuildToolkitBranchRawXml(string branchId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			WriteToolkitBranch(xmlWriter, branchId);
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private string BuildIsSourceToolkitXml(RawConnectParams parameters, MediaSessionInfo sessionInfo, bool enableMulticast)
	{
		FQID deviceFqid = parameters.DeviceFqid;
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(deviceFqid);
		if (loginSettings == null)
		{
			throw new CommunicationMIPException("LoginSettings not found");
		}
		string value = ConvertToMedia(deviceFqid);
		UriBuilder uriBuilder = new UriBuilder(deviceFqid.ServerId.ServerScheme, deviceFqid.ServerId.ServerHostname, deviceFqid.ServerId.Serverport);
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "source");
			xmlWriter.WriteElementString("provider", "is");
			xmlWriter.WriteStartElement("config");
			xmlWriter.WriteElementString("media_type", value);
			MulticastConfiguration value2 = null;
			sessionInfo.Multicast = false;
			xmlWriter.WriteStartElement("server_uri");
			if (enableMulticast)
			{
				lock (_multicastConfigurationLock)
				{
					if (_multicastConfigurations.TryGetValue(deviceFqid.ObjectId, out value2) && value2.MulticastSupported)
					{
						uriBuilder = new UriBuilder(Uri.UriSchemeHttp, value2.IpAddress, value2.Port);
						xmlWriter.WriteAttributeString("multicast", "true");
						if (value2.Ssrc >= 0)
						{
							xmlWriter.WriteAttributeString("ssrc", value2.Ssrc.ToString(CultureInfo.InvariantCulture));
						}
						sessionInfo.Multicast = true;
						Trace.WriteLine("Multicast config used: " + value2.IpAddress + ":" + value2.Port.ToString(CultureInfo.InvariantCulture) + "," + value2.Ssrc);
					}
					else
					{
						Trace.WriteLine("No multicast configuration found or multicast not supported");
					}
				}
			}
			xmlWriter.WriteString(uriBuilder.ToString());
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("device_id");
			if (parameters.StreamId != Guid.Empty)
			{
				xmlWriter.WriteAttributeString("stream_id", parameters.StreamId.ToString());
			}
			xmlWriter.WriteString(deviceFqid.ObjectId.ToString().ToUpper(CultureInfo.InvariantCulture));
			xmlWriter.WriteEndElement();
			if (value2 != null && !string.IsNullOrEmpty(value2.PrivacyMask))
			{
				xmlWriter.WriteStartElement("privacy_mask");
				xmlWriter.WriteAttributeString("version", value2.PrivacyMaskVersion.ToString(CultureInfo.InvariantCulture));
				xmlWriter.WriteString(value2.PrivacyMask);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteStartElement("authorization");
			xmlWriter.WriteAttributeString("method", "token");
			xmlWriter.WriteStartElement("token");
			xmlWriter.WriteAttributeString("update_key", "token_key");
			xmlWriter.WriteString(loginSettings.Token);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteElementString("maximum_queue_size", "30");
			xmlWriter.WriteElementString("sendinitialimage", parameters.SendInitialImage.ToYesNo());
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private void CheckAndCreateToolkitDictionary()
	{
		lock (_stateLock)
		{
			if (_toolkitDictionary != null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			string configurationXml;
			try
			{
				using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
				stringWriter = null;
				xmlWriter.WriteStartElement("toolkit");
				xmlWriter.WriteAttributeString("type", "generic");
				xmlWriter.WriteElementString("provider", "mtd");
				xmlWriter.WriteStartElement("config");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
				configurationXml = stringBuilder.ToString();
			}
			finally
			{
				stringWriter?.Close();
			}
			_toolkitDictionary = _factory.CreateInstance(configurationXml) as IDictionaryToolkit;
		}
	}
}
