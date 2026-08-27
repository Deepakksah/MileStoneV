using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Platform.Common.ToolkitXmlProviders;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.ToolkitXmlProviders;
using VideoOS.Toolkit;

namespace VideoOS.Platform.Common;

public abstract class InternalCommandServiceCommon : InternalCommandService
{
	protected class MediaSessionInfo
	{
		public long LastPrivacyMaskId;

		public string LastPrivacyMask;

		public MediaSessionInfo()
		{
			LastPrivacyMaskId = PrivacyMaskProperty.UnknownId;
			LastPrivacyMask = null;
		}
	}

	protected readonly object _sync = new object();

	protected readonly Dictionary<IToolkit, MediaSessionInfo> _mediaSessionInfos = new Dictionary<IToolkit, MediaSessionInfo>();

	protected Dictionary<object, object> _sessionCustomDataDictionary = new Dictionary<object, object>();

	protected abstract IToolkitFactory ToolkitFactory { get; }

	public override bool Initialized => true;

	protected abstract string BuildToolkitXPCODbXml(FQID fqid, int width, int height, BitmapFormat bitmapFormat, bool keepAspectRatio, bool fillSpace);

	protected abstract string BuildToolkitXPCODbXml(FQID fqid, int width, int height, string media, bool keepAspectRatio, bool fillSpace, bool allowUpscaling);

	protected abstract string BuildToolkitIsXml(FQID fqid, int width, int height, BitmapFormat bitmapFormat, bool keepAspectRatio, bool fillSpace);

	protected abstract string BuildToolkitIsXml(FQID fqid, int width, int height, string media, bool keepAspectRatio, bool fillSpace, bool allowUpscaling);

	protected abstract string BuildToolkitRawXPCODbXml(FQID fqid);

	public override void SetCustomData(object sessionId, object customData)
	{
		if (sessionId is ISourceToolkit key)
		{
			if (customData != null)
			{
				_sessionCustomDataDictionary[sessionId] = customData;
			}
			else
			{
				_sessionCustomDataDictionary.Remove(key);
			}
		}
	}

	protected static void HandleToolkitExceptions(string method, Exception ex)
	{
		if (ex is NotConnectedException)
		{
			throw new CommunicationMIPException(ex.Message, ex);
		}
		EnvironmentManager.Instance.Log(error: false, method, ex.Message, new object[1] { ex });
		if (ex is ToolkitException ex2)
		{
			throw new MIPException(ex2.CompleteErrorMessage, ex2);
		}
		throw new MIPException(ex.Message, ex);
	}

	protected static void FillDateTime(IData record, out DateTime time, out bool isAvail, string key)
	{
		isAvail = false;
		time = DateTime.UtcNow;
		if (record.GetProperty(key) is UTCTimeProperty uTCTimeProperty)
		{
			time = uTCTimeProperty.Value;
			isAvail = true;
		}
	}

	protected static TReturnType DataGet<TReturnType>(object sessionId, FQID deviceFqid, DateTime time, Func<IPlaybackSourceToolkit, TReturnType> getAndHandleToolkitDataFunc, IPlaybackSourceToolkit.SearchCriteria searchCriteria) where TReturnType : class
	{
		if (sessionId is IPlaybackSourceToolkit playbackSourceToolkit)
		{
			try
			{
				if (playbackSourceToolkit.MoveTo(time, searchCriteria) == IPlaybackSourceToolkit.MoveStatus.MOVED_OK)
				{
					return getAndHandleToolkitDataFunc(playbackSourceToolkit);
				}
			}
			catch (Exception ex)
			{
				HandleToolkitExceptions("DataGet", ex);
			}
		}
		return null;
	}

	protected static TReturnType Command<TReturnType>(object sessionId, string command, Func<IPlaybackSourceToolkit, TReturnType> getAndHandleToolkitDataFunc) where TReturnType : class
	{
		if (sessionId is IPlaybackSourceToolkit playbackSourceToolkit)
		{
			try
			{
				IPlaybackSourceToolkit.MoveStatus moveStatus = IPlaybackSourceToolkit.MoveStatus.NOT_MOVED;
				switch (command)
				{
				case "Begin":
					moveStatus = playbackSourceToolkit.MoveFirst();
					break;
				case "End":
					moveStatus = playbackSourceToolkit.MoveLast();
					break;
				case "Previous":
					moveStatus = playbackSourceToolkit.MovePrevious();
					break;
				case "Next":
					moveStatus = playbackSourceToolkit.MoveNext();
					break;
				case "PreviousSequence":
					moveStatus = playbackSourceToolkit.MovePreviousSequence();
					break;
				case "NextSequence":
					moveStatus = playbackSourceToolkit.MoveNextSequence();
					break;
				}
				if (moveStatus == IPlaybackSourceToolkit.MoveStatus.MOVED_OK)
				{
					return getAndHandleToolkitDataFunc(playbackSourceToolkit);
				}
			}
			catch (Exception ex)
			{
				HandleToolkitExceptions("Toolkit.Command:" + command, ex);
			}
		}
		return null;
	}

	protected static IData GetRecordAndInfo(IPlaybackSourceToolkit toolkit, out DateTime prevTime, out bool isPrevAvail, out DateTime nextTime, out bool isNextAvail)
	{
		prevTime = DateTime.MinValue;
		nextTime = DateTime.MaxValue;
		isPrevAvail = false;
		isNextAvail = false;
		if (toolkit.GetData(out var data) == IPlaybackSourceToolkit.GetDataStatus.DATA_RETRIEVED)
		{
			FillDateTime(data, out prevTime, out isPrevAvail, "Prev_Begin_Time");
			FillDateTime(data, out nextTime, out isNextAvail, "Next_Begin_Time");
			if (!isPrevAvail)
			{
				prevTime = DateTime.MinValue;
			}
			if (!isNextAvail)
			{
				nextTime = DateTime.MaxValue;
			}
			return data;
		}
		return null;
	}

	protected static TReturnTypeList GetAndHandleToolkitMultiRecord<TReturnType, TReturnTypeList>(IPlaybackSourceToolkit toolkit, Action<IData> onRecord, Func<IMediaData, TReturnType> convertor, Func<IEnumerable<TReturnType>, TReturnTypeList> convertorList, Func<TReturnType, ISourceData> sourceDataProxy) where TReturnType : class where TReturnTypeList : class
	{
		return GetAndHandleToolkitMultiRecord(toolkit, onRecord, convertor, convertor, convertorList, sourceDataProxy);
	}

	protected static TReturnTypeList GetAndHandleToolkitMultiRecord<TReturnType, TReturnTypeList, TRecordType>(IPlaybackSourceToolkit toolkit, Action<IData> onRecord, Func<TRecordType, TReturnType> convertorFromRecordType, Func<IMediaData, TReturnType> convertorFromMediaData, Func<IEnumerable<TReturnType>, TReturnTypeList> convertorList, Func<TReturnType, ISourceData> sourceDataProxy) where TReturnType : class where TReturnTypeList : class where TRecordType : class, IMediaData
	{
		DateTime prevTime;
		bool isPrevAvail;
		DateTime nextTime;
		bool isNextAvail;
		IData recordAndInfo = GetRecordAndInfo(toolkit, out prevTime, out isPrevAvail, out nextTime, out isNextAvail);
		if (recordAndInfo != null)
		{
			DateTime beginTimeStamp = recordAndInfo.BeginTimeStamp;
			onRecord?.Invoke(recordAndInfo);
			IMediaDataVector mediaDataVector = recordAndInfo as IMediaDataVector;
			TRecordType val = recordAndInfo as TRecordType;
			DateTime dateTime = prevTime;
			if (mediaDataVector != null)
			{
				List<TReturnType> list = new List<TReturnType>();
				IMediaData[] vector = mediaDataVector.GetVector();
				for (int i = 0; i < vector.Length; i++)
				{
					IMediaData mediaData = vector[i];
					DateTime beginTimeStamp2 = mediaData.BeginTimeStamp;
					DateTime dateTime2 = ((i == 0) ? prevTime : dateTime);
					DateTime dateTime3 = ((i == vector.Length - 1) ? nextTime : vector[i + 1].BeginTimeStamp);
					dateTime = beginTimeStamp2;
					TReturnType val2 = convertorFromMediaData(mediaData);
					ISourceData sourceData = sourceDataProxy(val2);
					if (val2 != null && sourceData != null)
					{
						sourceData.DateTime = beginTimeStamp2;
						sourceData.IsNextAvailable = dateTime3 != DateTime.MaxValue;
						sourceData.IsPreviousAvailable = dateTime2 != DateTime.MinValue;
						sourceData.NextDateTime = dateTime3;
						sourceData.PreviousDateTime = dateTime2;
					}
					list.Add(val2);
				}
				mediaDataVector.Dispose();
				return convertorList(list);
			}
			if (val != null)
			{
				if (beginTimeStamp.Year != 1970)
				{
					TReturnType val3 = convertorFromRecordType(val);
					ISourceData sourceData2 = sourceDataProxy(val3);
					if (val3 != null && sourceData2 != null)
					{
						sourceData2.DateTime = beginTimeStamp;
						sourceData2.IsNextAvailable = isNextAvail;
						sourceData2.IsPreviousAvailable = isPrevAvail;
						sourceData2.NextDateTime = nextTime;
						sourceData2.PreviousDateTime = prevTime;
					}
					return convertorList(new TReturnType[1] { val3 });
				}
				EnvironmentManager.Instance.Log(error: false, "GetAndHandleToolkitRaw", "The mediaData is != null, but BeginTime == 1970 -> Frame ignored", null);
			}
		}
		return null;
	}

	public string BuildToolkitXml(FQID fqid, int width, int height, string media, bool keepAspectRatio, bool fillSpace, bool allowUpscaling)
	{
		if (fqid.ServerId.ServerType == "BLK")
		{
			return BuildToolkitXPCODbXml(fqid, width, height, media, keepAspectRatio, fillSpace, allowUpscaling);
		}
		return BuildToolkitIsXml(fqid, width, height, media, keepAspectRatio, fillSpace, allowUpscaling);
	}

	private string BuildToolkitXml(FQID fqid, int width, int height, BitmapFormat format, bool keepAspectRatio, bool fillSpace)
	{
		if (fqid.ServerId.ServerType == "BLK")
		{
			return BuildToolkitXPCODbXml(fqid, width, height, format, keepAspectRatio, fillSpace);
		}
		return BuildToolkitIsXml(fqid, width, height, format, keepAspectRatio, fillSpace);
	}

	private static string BuildToolkitPcmXml(InternalMediaService.PcmAudioConnectParams pcmAudioConnectParameters)
	{
		AudioDecoderXmlProvider audioDecoderXmlProvider = new AudioDecoderXmlProvider(pcmAudioConnectParameters);
		audioDecoderXmlProvider.Chain(CommonSourceXmlProvider.GetRelevantProvider(pcmAudioConnectParameters.DeviceFqid, pcmAudioConnectParameters.StreamId));
		return audioDecoderXmlProvider.ConfigurationXml;
	}

	private string BuildToolkitRawXml(FQID fqid)
	{
		return BuildToolkitRawXml(fqid, DateTime.MinValue, DateTime.MinValue, Guid.Empty);
	}

	protected string BuildToolkitRawXml(FQID fqid, DateTime start, DateTime end, Guid trackId)
	{
		if (fqid.ServerId.ServerType == "BLK")
		{
			return BuildToolkitRawXPCODbXml(fqid);
		}
		return BuildToolkitRawIsXml(fqid, start, end, trackId);
	}

	private static string BuildToolkitRawIsXml(FQID fqid, DateTime start, DateTime end, Guid trackId)
	{
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(fqid);
		if (loginSettings == null)
		{
			throw new MIPException("LoginSettings not found");
		}
		string media = fqid.ToMedia();
		Uri uri = new UriBuilder(fqid.ServerId.ServerScheme, fqid.ServerId.ServerHostname, fqid.ServerId.Serverport).Uri;
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			WriteIsSource(xmlWriter, uri, fqid, loginSettings, media, start, end, trackId);
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	protected static void WriteIsSource(XmlWriter xw, Uri uri, FQID fqid, LoginSettings loginSettings, string media, DateTime start, DateTime end, Guid trackId)
	{
		xw.WriteStartElement("toolkit");
		xw.WriteAttributeString("type", "source");
		xw.WriteElementString("provider", "is");
		xw.WriteStartElement("config");
		xw.WriteElementString("media_type", media);
		xw.WriteElementString("server_uri", uri.ToString());
		xw.WriteStartElement("device_id");
		if (trackId != Guid.Empty)
		{
			xw.WriteAttributeString("track_id", trackId.ToString());
		}
		xw.WriteString(fqid.ObjectId.ToString().ToUpper(CultureInfo.InvariantCulture));
		xw.WriteEndElement();
		xw.WriteStartElement("authorization");
		xw.WriteAttributeString("method", "token");
		xw.WriteStartElement("token");
		xw.WriteAttributeString("update_key", "token_key");
		xw.WriteString(loginSettings.Token);
		xw.WriteEndElement();
		xw.WriteEndElement();
		xw.WriteElementString("maximum_queue_size", "25");
		if (start != DateTime.MinValue && end != DateTime.MinValue)
		{
			WriteTimeRestrictionXml(xw, start, end);
		}
		xw.WriteEndElement();
		xw.WriteEndElement();
	}

	private static void WriteTimeRestrictionXml(XmlWriter xmlTextWriter, DateTime startTimeRestrictionUtc, DateTime endTimeRestrictionUtc)
	{
		xmlTextWriter.WriteStartElement("timerestriction");
		xmlTextWriter.WriteElementString("starttime", XmlConvert.ToString(startTimeRestrictionUtc, XmlDateTimeSerializationMode.Utc));
		xmlTextWriter.WriteElementString("endtime", XmlConvert.ToString(endTimeRestrictionUtc, XmlDateTimeSerializationMode.Utc));
		xmlTextWriter.WriteEndElement();
	}

	protected void ToolkitConnectWithExceptionHandling(IPlaybackSourceToolkit toolkit, ISet<string> properties, out ISet<string> availableProperties)
	{
		try
		{
			toolkit.Connect(properties, out availableProperties);
		}
		catch (NotConnectedException ex)
		{
			Cleanup(toolkit);
			throw new CommunicationMIPException(ex.Message, ex);
		}
		catch (Exception ex2)
		{
			EnvironmentManager.Instance.Log(error: false, "Toolkit.Connect", ex2.Message, new object[1] { ex2 });
			Cleanup(toolkit);
			throw new MIPException(ex2.Message, ex2);
		}
	}

	private void Cleanup(IPlaybackSourceToolkit toolkit)
	{
		try
		{
			ImageExporterDisconnect(toolkit);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageExporterDisconnect", ex.Message, new object[1] { ex });
		}
	}

	public override void ImageExporterDisconnect(object sessionId)
	{
		if (!(sessionId is IPlaybackSourceToolkit playbackSourceToolkit))
		{
			return;
		}
		_sessionCustomDataDictionary.Remove(playbackSourceToolkit);
		playbackSourceToolkit.Disconnect();
		lock (_sync)
		{
			ToolkitFactory.DeleteInstance(playbackSourceToolkit);
			if (!_mediaSessionInfos.Remove(playbackSourceToolkit))
			{
				EnvironmentManager.Instance.Log(error: false, "ImageExporterDisconnect", "Unable to free toolkit source, not found in dictionary");
			}
		}
	}

	protected object Connect(IPlaybackSourceToolkit toolkit, string additionalProperty = null)
	{
		ISet<string> set = new SortedSet<string> { "Next_Begin_Time", "Next_End_Time", "Prev_Begin_Time", "Prev_End_Time", "Has_Motion", "Privacy_Mask", "Sequence_Number", "Decoder_Implementation" };
		if (!string.IsNullOrWhiteSpace(additionalProperty))
		{
			set.Add(additionalProperty);
		}
		ToolkitConnectWithExceptionHandling(toolkit, set, out var _);
		return toolkit;
	}

	protected T CreateAndRegisterToolkit<T>(string configXml) where T : class, IToolkit
	{
		IToolkit toolkit;
		lock (_sync)
		{
			toolkit = ToolkitFactory.CreateInstance(configXml);
			_mediaSessionInfos.Add(toolkit, new MediaSessionInfo());
		}
		return toolkit as T;
	}

	protected IPlaybackSourceToolkit CreateToolkitSource(FQID fqid, int width, int height, bool keepAspectRatio, bool fillSpace, bool allowUpscaling)
	{
		string configXml = BuildToolkitXml(fqid, width, height, "VIDEO", keepAspectRatio, fillSpace, allowUpscaling);
		return CreateAndRegisterToolkit<IPlaybackSourceToolkit>(configXml);
	}

	protected IPlaybackSourceToolkit CreateToolkitSource(FQID fqid, int width, int height, BitmapFormat format, bool keepAspectRatio, bool fillSpace)
	{
		string configXml = BuildToolkitXml(fqid, width, height, format, keepAspectRatio, fillSpace);
		return CreateAndRegisterToolkit<IPlaybackSourceToolkit>(configXml);
	}

	protected IPlaybackSourceToolkit CreateToolkitSource(FQID fqid)
	{
		string configXml = BuildToolkitRawXml(fqid);
		return CreateAndRegisterToolkit<IPlaybackSourceToolkit>(configXml);
	}

	private IPlaybackSourceToolkit CreateToolkitSource(InternalMediaService.PcmAudioConnectParams pcmAudioConnectParameters)
	{
		string configXml = BuildToolkitPcmXml(pcmAudioConnectParameters);
		return CreateAndRegisterToolkit<IPlaybackSourceToolkit>(configXml);
	}

	public override PcmAudioDataList PcmAudioGetAt(object sessionId, FQID deviceFQID, DateTime time)
	{
		return DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitPcm, IPlaybackSourceToolkit.SearchCriteria.AT_OR_AFTER);
	}

	public override PcmAudioDataList PcmAudioGetNearest(object sessionId, FQID deviceFQID, DateTime time)
	{
		return DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitPcm, IPlaybackSourceToolkit.SearchCriteria.NEAREST);
	}

	public override PcmAudioDataList PcmAudioGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		return DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitPcm, IPlaybackSourceToolkit.SearchCriteria.AT_OR_BEFORE);
	}

	public override PcmAudioDataList PcmAudioCommand(object sessionId, string command)
	{
		return Command(sessionId, command, GetAndHandleToolkitPcm);
	}

	private PcmAudioDataList GetAndHandleToolkitPcm(IPlaybackSourceToolkit toolkit)
	{
		return GetAndHandleToolkitMultiRecord(toolkit, null, delegate(IMediaData mediaData)
		{
			PcmAudioData result = null;
			byte[] mediaBody = mediaData.GetMediaBody();
			if (mediaBody != null)
			{
				result = new PcmAudioData
				{
					Data = mediaBody
				};
			}
			mediaData.Dispose();
			return result;
		}, (IEnumerable<PcmAudioData> pcmAudioDatas) => new PcmAudioDataList(pcmAudioDatas), (PcmAudioData pcmAudioData) => pcmAudioData);
	}

	public override object PcmAudioConnect(InternalMediaService.PcmAudioConnectParams pcmAudioConnectParameters)
	{
		return Connect(CreateToolkitSource(pcmAudioConnectParameters));
	}
}
