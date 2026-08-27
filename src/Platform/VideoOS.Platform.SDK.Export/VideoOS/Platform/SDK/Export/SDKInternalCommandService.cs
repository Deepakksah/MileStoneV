using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Xml;
using VideoOS.DatabaseUtility;
using VideoOS.Platform.Common;
using VideoOS.Platform.Common.Util;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;
using VideoOS.Toolkit;

namespace VideoOS.Platform.SDK.Export;

public class SDKInternalCommandService : InternalCommandServiceCommon
{
	public class AlertThreadPoolContainer
	{
		private readonly Thread _callerThread;

		private readonly AsyncCallbackHandler _handler;

		private readonly FQID _alertFQID;

		private readonly DateTime _dateTime;

		private readonly TimeSpan _timeBefore;

		private readonly int _maxCountBefore;

		private readonly TimeSpan _timeAfter;

		private readonly int _maxCountAfter;

		private readonly object _asyncState;

		public Thread CallerThread => _callerThread;

		public AsyncCallbackHandler AsyncItemsHandler => _handler;

		public object AsyncState => _asyncState;

		public FQID AlertFQID => _alertFQID;

		public DateTime DateTime => _dateTime;

		public TimeSpan TimeBefore => _timeBefore;

		public int MaxCountBefore => _maxCountBefore;

		public TimeSpan TimeAfter => _timeAfter;

		public int MaxCountAfter => _maxCountAfter;

		public AlertThreadPoolContainer(AsyncCallbackHandler handler, FQID alertFQID, object asyncState, DateTime dateTime, TimeSpan timeBefore, int maxCountBefore, TimeSpan timeAfter, int maxCountAfter)
		{
			_callerThread = Thread.CurrentThread;
			_handler = handler;
			_asyncState = asyncState;
			_alertFQID = alertFQID;
			_dateTime = dateTime;
			_timeBefore = timeBefore;
			_maxCountBefore = maxCountBefore;
			_timeAfter = timeAfter;
			_maxCountAfter = maxCountAfter;
		}
	}

	public class SequenceThreadPoolContainer
	{
		private readonly Thread _callerThread;

		private readonly AsyncCallbackHandler _handler;

		private readonly FQID _seqFQID;

		private readonly DateTime _centerTime;

		private readonly TimeSpan _timeBefore;

		private readonly int _maxCountBefore;

		private readonly TimeSpan _timeAfter;

		private readonly int _maxCountAfter;

		private readonly object _asyncState;

		private readonly Guid _seqType;

		public Thread CallerThread => _callerThread;

		public AsyncCallbackHandler AsyncItemsHandler => _handler;

		public object AsyncState => _asyncState;

		public FQID SeqFQID => _seqFQID;

		public DateTime CenterTime => _centerTime;

		public TimeSpan TimeBefore => _timeBefore;

		public int MaxCountBefore => _maxCountBefore;

		public TimeSpan TimeAfter => _timeAfter;

		public int MaxCountAfter => _maxCountAfter;

		public Guid SeqType => _seqType;

		public SequenceThreadPoolContainer(AsyncCallbackHandler handler, FQID seqFQID, object asyncState, Guid seqType, DateTime centerTime, TimeSpan timeBefore, int maxCountBefore, TimeSpan timeAfter, int maxCountAfter)
		{
			_callerThread = Thread.CurrentThread;
			_handler = handler;
			_asyncState = asyncState;
			_seqFQID = seqFQID;
			_centerTime = centerTime;
			_timeBefore = timeBefore;
			_maxCountBefore = maxCountBefore;
			_timeAfter = timeAfter;
			_maxCountAfter = maxCountAfter;
			_seqType = seqType;
		}
	}

	private static List<object> _activeSessions = new List<object>();

	private readonly Lazy<IToolkitFactory> _factory;

	protected override IToolkitFactory ToolkitFactory => _factory.Value;

	public SDKInternalCommandService()
	{
		_factory = new Lazy<IToolkitFactory>(() => new ToolkitFactory());
	}

	public override VideoOS.Platform.Util.SequenceEntry[] SequencesGetAroundWithSpan(FQID deviceFQID, Guid sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		return SequencesGetAroundWithSpan(deviceFQID, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, throwException: false);
	}

	public override VideoOS.Platform.Util.SequenceEntry[] SequencesGetAroundWithSpan(FQID deviceFQID, Guid sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, bool throwException)
	{
		try
		{
			ISystemInfo systemInfo = ((SDKConfiguration)deviceFQID.ServerId.UserContext.Configuration).FindSystemInfo(deviceFQID.ServerId, populate: true);
			if (systemInfo == null)
			{
				throw new MIPException("Unable to find system for device with ObjectId:" + deviceFQID.ObjectId.ToString());
			}
			if (deviceFQID.ServerId.IsExportType)
			{
				VideoOS.DatabaseUtility.SequenceType sequenceType2 = VideoOS.DatabaseUtility.SequenceType.RecordingSequence;
				if (sequenceType == DataType.SequenceTypeGuids.MotionSequence)
				{
					sequenceType2 = VideoOS.DatabaseUtility.SequenceType.MotionSequence;
				}
				if (sequenceType == DataType.SequenceTypeGuids.RecordingWithTriggerSequence)
				{
					sequenceType2 = VideoOS.DatabaseUtility.SequenceType.RecordingWithTriggerSequence;
				}
				SequencesResponse[] array = DatabaseSequences.FindOrCreate(deviceFQID, systemInfo).SequencesGetAroundWithSpan(sequenceType2, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
				VideoOS.Platform.Util.SequenceEntry[] array2 = new VideoOS.Platform.Util.SequenceEntry[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					SequencesResponse sequencesResponse = array[i];
					array2[i] = new VideoOS.Platform.Util.SequenceEntry
					{
						TimeTriggedField = sequencesResponse.TimeTrigged,
						TimeBeginField = sequencesResponse.TimeBegin,
						TimeEndField = sequencesResponse.TimeEnd
					};
				}
				return array2;
			}
			SequenceCollection sequencesForDevice = GetSequencesForDevice(deviceFQID, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, systemInfo);
			if (sequencesForDevice == null)
			{
				return Array.Empty<VideoOS.Platform.Util.SequenceEntry>();
			}
			VideoOS.Platform.Util.SequenceEntry[] array3 = new VideoOS.Platform.Util.SequenceEntry[sequencesForDevice.Count];
			for (int j = 0; j < sequencesForDevice.Count; j++)
			{
				Sequence sequence = sequencesForDevice[j];
				array3[j] = new VideoOS.Platform.Util.SequenceEntry
				{
					TimeTriggedField = sequence.TriggerTime,
					TimeBeginField = sequence.BeginTime,
					TimeEndField = sequence.EndTime
				};
			}
			return array3;
		}
		catch (Exception exception)
		{
			if (throwException)
			{
				throw;
			}
			EnvironmentManager.Instance.ExceptionHandler("SequencesGetAroundWithSpan", exception);
			return Array.Empty<VideoOS.Platform.Util.SequenceEntry>();
		}
	}

	private static SequenceCollection GetSequencesForDevice(FQID deviceFqid, Guid sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, ISystemInfo systemInfo)
	{
		if (deviceFqid.Kind == Kind.Camera)
		{
			return systemInfo.Cameras[deviceFqid.ObjectId].GetSequenceAtTimeMinutes(centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, sequenceType);
		}
		if (deviceFqid.Kind == Kind.Metadata)
		{
			return systemInfo.MetadataDevices[deviceFqid.ObjectId].GetSequenceAtTimeMinutes(centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, sequenceType);
		}
		if (deviceFqid.Kind == Kind.Microphone)
		{
			return systemInfo.Microphones[deviceFqid.ObjectId].GetSequenceAtTimeMinutes(centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, sequenceType);
		}
		if (deviceFqid.Kind == Kind.Speaker)
		{
			return systemInfo.Speakers[deviceFqid.ObjectId].GetSequenceAtTimeMinutes(centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, sequenceType);
		}
		throw new ImplementationMIPException("Sequences not implemented for device type " + deviceFqid.Kind.ToString());
	}

	public override void AsyncSequencesGetAroundWithSpan(FQID deviceFQID, object asyncState, Guid sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, AsyncCallbackHandler resultCallbackHandler)
	{
		ThreadPool.UnsafeQueueUserWorkItem(AsyncSequencesGetAroundWithSpanHandler, new SequenceThreadPoolContainer(resultCallbackHandler, deviceFQID, asyncState, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter));
	}

	private void AsyncSequencesGetAroundWithSpanHandler(object parm)
	{
		try
		{
			SequenceThreadPoolContainer sequenceThreadPoolContainer = (SequenceThreadPoolContainer)parm;
			VideoOS.Platform.Util.SequenceEntry[] result = SequencesGetAroundWithSpan(sequenceThreadPoolContainer.SeqFQID, sequenceThreadPoolContainer.SeqType, sequenceThreadPoolContainer.CenterTime, sequenceThreadPoolContainer.TimeBefore, sequenceThreadPoolContainer.MaxCountBefore, sequenceThreadPoolContainer.TimeAfter, sequenceThreadPoolContainer.MaxCountAfter);
			sequenceThreadPoolContainer.AsyncItemsHandler(result, sequenceThreadPoolContainer.AsyncState);
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("CommandService", "AsyncSequencesGetAroundWithSpanHandler", exception);
		}
	}

	public override VideoOS.Platform.Util.SequenceType[] SequencesGetTypes(FQID deviceFQID)
	{
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.SequenceType[] sequenceTypesFromDevice = GetSequenceTypesFromDevice(deviceFQID);
		if (sequenceTypesFromDevice != null)
		{
			List<VideoOS.Platform.Util.SequenceType> list = sequenceTypesFromDevice.Select((VideoOS.Platform.SDK.Proxy.RecorderServices.Net.SequenceType type) => new VideoOS.Platform.Util.SequenceType
			{
				Id = type.Id,
				Name = type.Name
			}).ToList();
			list.Add(new VideoOS.Platform.Util.SequenceType
			{
				Id = DataType.SequenceTypeGuids.TimelineMotionDetected,
				Name = "MotionSequenceFromTimeline"
			});
			list.Add(new VideoOS.Platform.Util.SequenceType
			{
				Id = DataType.SequenceTypeGuids.TimelineRecording,
				Name = "RecordingsFromTimeline"
			});
			return list.ToArray();
		}
		return new VideoOS.Platform.Util.SequenceType[3]
		{
			new VideoOS.Platform.Util.SequenceType
			{
				Id = DataType.SequenceTypeGuids.MotionSequence,
				Name = "MotionSequence"
			},
			new VideoOS.Platform.Util.SequenceType
			{
				Id = DataType.SequenceTypeGuids.RecordingSequence,
				Name = "RecordingSequence"
			},
			new VideoOS.Platform.Util.SequenceType
			{
				Id = DataType.SequenceTypeGuids.RecordingWithTriggerSequence,
				Name = "RecordingWithTriggerSequence"
			}
		};
	}

	private static VideoOS.Platform.SDK.Proxy.RecorderServices.Net.SequenceType[] GetSequenceTypesFromDevice(FQID deviceFQID)
	{
		ISystemInfo systemInfo = ((SDKConfiguration)deviceFQID.ServerId.UserContext.Configuration).FindSystemInfo(deviceFQID.ServerId, populate: true);
		if (deviceFQID.Kind == Kind.Camera)
		{
			return systemInfo.Cameras[deviceFQID.ObjectId].GetSequenceTypes();
		}
		if (deviceFQID.Kind == Kind.Metadata)
		{
			return systemInfo.MetadataDevices[deviceFQID.ObjectId].GetSequenceTypes();
		}
		if (deviceFQID.Kind == Kind.Microphone)
		{
			return systemInfo.Microphones[deviceFQID.ObjectId].GetSequenceTypes();
		}
		if (deviceFQID.Kind == Kind.Speaker)
		{
			return systemInfo.Speakers[deviceFQID.ObjectId].GetSequenceTypes();
		}
		throw new ImplementationMIPException("Sequences not implemented for device type " + deviceFQID.Kind.ToString());
	}

	public override void AsyncSequencesGetTypes(FQID deviceFQID, object asyncState, AsyncCallbackHandler resultCallbackHandler)
	{
		VideoOS.Platform.SDK.Proxy.RecorderServices.Net.SequenceType[] sequenceTypesFromDevice = GetSequenceTypesFromDevice(deviceFQID);
		if (sequenceTypesFromDevice != null)
		{
			List<VideoOS.Platform.Util.SequenceType> list = new List<VideoOS.Platform.Util.SequenceType>();
			for (int i = 0; i < sequenceTypesFromDevice.Length; i++)
			{
				list.Add(new VideoOS.Platform.Util.SequenceType
				{
					Id = sequenceTypesFromDevice[i].Id,
					Name = sequenceTypesFromDevice[i].Name
				});
			}
			list.Add(new VideoOS.Platform.Util.SequenceType
			{
				Id = DataType.SequenceTypeGuids.TimelineMotionDetected,
				Name = "MotionSequenceFromTimeline"
			});
			list.Add(new VideoOS.Platform.Util.SequenceType
			{
				Id = DataType.SequenceTypeGuids.TimelineRecording,
				Name = "RecordingsFromTimeline"
			});
			resultCallbackHandler(list.ToArray(), asyncState);
		}
		else
		{
			resultCallbackHandler(Array.Empty<VideoOS.Platform.Util.SequenceEntry>(), asyncState);
		}
	}

	public override object ImageExporterConnect(FQID deviceFQID)
	{
		return ImageExporterConnect(deviceFQID, 0, 0);
	}

	public override object ImageExporterConnect(FQID deviceFQID, int width, int height)
	{
		return ImageExporterConnect(deviceFQID, width, height, keepAspectRatio: false, fillSpace: false);
	}

	public override object ImageExporterConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace)
	{
		return ImageExporterConnect(deviceFQID, width, height, keepAspectRatio, fillSpace, allowUspcaling: true);
	}

	public override object ImageExporterConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, bool allowUspcaling)
	{
		return Connect(CreateToolkitSource(deviceFQID, width, height, keepAspectRatio, fillSpace, allowUspcaling));
	}

	public override object ImageExporterConnect(FQID deviceFQID, int width, int height, BitmapFormat format)
	{
		return ImageExporterConnect(deviceFQID, width, height, keepAspectRatio: false, fillSpace: false, format);
	}

	public override object ImageExporterConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat format)
	{
		return Connect(CreateToolkitSource(deviceFQID, width, height, format, keepAspectRatio, fillSpace));
	}

	public override object ImageExporterConnect(FQID deviceFQID, string media, bool requestSignatures)
	{
		return Connect(CreateToolkitSource(deviceFQID), requestSignatures ? "Recorded_Signatures" : null);
	}

	public object ToolkitExportCreate(Item camera, List<Item> audioSources, DateTime start, DateTime end, string fullPath)
	{
		string cameraSourceString = null;
		if (camera != null)
		{
			cameraSourceString = BuildToolkitRawXml(camera.FQID, start, end, Guid.Empty);
		}
		List<string> list = new List<string>();
		foreach (Item audioSource in audioSources)
		{
			string item = BuildToolkitRawXml(audioSource.FQID, start, end, Guid.Empty);
			list.Add(item);
		}
		string configXml = BuildExportToolkitMkv(fullPath, start, end, cameraSourceString, list, camera.FQID.ServerId.UserContext.PrivacyMaskLifted);
		return CreateAndRegisterToolkit<IToolkit>(configXml);
	}

	public object ToolkitExportXpcoCreate(Item exportItem, DateTime start, DateTime end, string fullPath, EncryptionStrength encryptionStrength, string encryptionPassword, bool signData, string privacyMask, bool reExportEnabled, Guid trackId, bool failOnInvalidSignature)
	{
		string sourceString = BuildToolkitRawXml(exportItem.FQID, start, end, trackId);
		bool encryptionEnabled = !string.IsNullOrWhiteSpace(encryptionPassword);
		string configXml = BuildExportToolkitXpco(exportItem, fullPath, start, end, sourceString, privacyMask, reExportEnabled, encryptionEnabled, encryptionStrength, encryptionPassword, signData, trackId, exportItem.FQID.ServerId.UserContext.PrivacyMaskLifted, failOnInvalidSignature);
		return CreateAndRegisterToolkit<IToolkit>(configXml);
	}

	public static int ToolkitExportStart(object sessionId)
	{
		if (sessionId is IExportToolkit exportToolkit)
		{
			IData data;
			switch (exportToolkit.Export(out data))
			{
			case IExportToolkit.ExportResult.EXPORT_RESULT_CANCELLED:
				return 0;
			case IExportToolkit.ExportResult.EXPORT_RESULT_EMPTY:
				return 1;
			case IExportToolkit.ExportResult.EXPORT_RESULT_DONE:
				return 2;
			case IExportToolkit.ExportResult.EXPORT_RESULT_DONE_WITH_SIGN_ERROR:
				return 3;
			}
			data?.Dispose();
		}
		return 0;
	}

	public static void ToolkitExportUpdateToken(object sessionId, string token)
	{
		if (sessionId is IExportToolkit exportToolkit)
		{
			try
			{
				exportToolkit.UpdateConfiguration("token_key", token);
			}
			catch (NotConnectedException ex)
			{
				throw new CommunicationMIPException(ex.Message, ex);
			}
			catch (Exception ex2)
			{
				EnvironmentManager.Instance.Log(error: false, "Toolkit.UpdateToken", ex2.Message, new object[1] { ex2 });
				throw new MIPException(ex2.Message, ex2);
			}
		}
	}

	public static void ToolkitExportCancel(object sessionId)
	{
		if (sessionId is IExportToolkit exportToolkit)
		{
			exportToolkit.CancelExport(keepExportedData: true);
		}
	}

	public static ulong ToolkitExportGetEstimation(object sessionId)
	{
		if (sessionId is IExportToolkit exportToolkit)
		{
			exportToolkit.EstimateSizeOfExport(out var estimatedSize);
			return estimatedSize;
		}
		return 0uL;
	}

	public static int ToolkitExportPercentCompleted(object sessionId)
	{
		if (sessionId is IExportToolkit exportToolkit)
		{
			return exportToolkit.GetPercentageCompleted();
		}
		return -1;
	}

	public void ToolkitExportClose(object sessionId)
	{
		if (!(sessionId is IExportToolkit exportToolkit))
		{
			return;
		}
		lock (_sync)
		{
			ToolkitFactory.DeleteInstance(exportToolkit);
			if (!_mediaSessionInfos.Remove(exportToolkit))
			{
				EnvironmentManager.Instance.Log(error: false, "ToolkitExportClose", "Unable to free toolkit source, not found in dictionary");
			}
		}
	}

	public override VideoOS.Platform.Data.JPEGData ImageExporterJPEGGetAt(object sessionId, FQID deviceFQID, DateTime time)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitData, IPlaybackSourceToolkit.SearchCriteria.AT_OR_AFTER);
	}

	public override VideoOS.Platform.Data.JPEGData ImageExporterJPEGGetNearest(object sessionId, FQID deviceFQID, DateTime time)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitData, IPlaybackSourceToolkit.SearchCriteria.NEAREST);
	}

	public override VideoOS.Platform.Data.JPEGData ImageExporterJPEGGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitData, IPlaybackSourceToolkit.SearchCriteria.AT_OR_BEFORE);
	}

	public override VideoOS.Platform.Data.JPEGData ImageExporterCommand(object sessionId, string command)
	{
		return InternalCommandServiceCommon.Command(sessionId, command, GetAndHandleToolkitData);
	}

	public override void ImageExporterGoTo(object sessionId, DateTime time, string direction)
	{
		ImageExporterGoTo2(sessionId, time, direction);
	}

	public override bool ImageExporterGoTo2(object sessionId, DateTime time, string direction)
	{
		if (sessionId is IPlaybackSourceToolkit playbackSourceToolkit)
		{
			IPlaybackSourceToolkit.SearchCriteria searchCriteria = IPlaybackSourceToolkit.SearchCriteria.NEAREST;
			if (!(direction == "Forward"))
			{
				if (direction == "Reverse")
				{
					searchCriteria = IPlaybackSourceToolkit.SearchCriteria.AT_OR_BEFORE;
				}
			}
			else
			{
				searchCriteria = IPlaybackSourceToolkit.SearchCriteria.AT_OR_AFTER;
			}
			try
			{
				return playbackSourceToolkit.MoveTo(time, searchCriteria) == IPlaybackSourceToolkit.MoveStatus.MOVED_OK;
			}
			catch (Exception ex)
			{
				InternalCommandServiceCommon.HandleToolkitExceptions("Toolkit.GoTo", ex);
			}
		}
		return false;
	}

	public override BitmapData ImageExporterBitmapGetAt(object sessionId, FQID deviceFQID, DateTime time)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitBitmap, IPlaybackSourceToolkit.SearchCriteria.AT_OR_AFTER);
	}

	public override BitmapData ImageExporterBitmapGetNearest(object sessionId, FQID deviceFQID, DateTime time)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitBitmap, IPlaybackSourceToolkit.SearchCriteria.NEAREST);
	}

	public override BitmapData ImageExporterBitmapGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, GetAndHandleToolkitBitmap, IPlaybackSourceToolkit.SearchCriteria.AT_OR_BEFORE);
	}

	public override BitmapData ImageExporterBitmapCommand(object sessionId, string command)
	{
		return InternalCommandServiceCommon.Command(sessionId, command, GetAndHandleToolkitBitmap);
	}

	public override RawVideoSourceDataList ImageExporterRawGetAt(object sessionId, FQID deviceFQID, DateTime time)
	{
		return ImageExporterRawGetAt(sessionId, deviceFQID, time, withHeader: true);
	}

	public override RawVideoSourceDataList ImageExporterRawGetAt(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, (IPlaybackSourceToolkit toolkit) => GetAndHandleToolkitRaw(toolkit, withHeader), IPlaybackSourceToolkit.SearchCriteria.AT_OR_AFTER);
	}

	public override RawVideoSourceDataList ImageExporterRawGetNearest(object sessionId, FQID deviceFQID, DateTime time)
	{
		return ImageExporterRawGetNearest(sessionId, deviceFQID, time, withHeader: true);
	}

	public override RawVideoSourceDataList ImageExporterRawGetNearest(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, (IPlaybackSourceToolkit toolkit) => GetAndHandleToolkitRaw(toolkit, withHeader), IPlaybackSourceToolkit.SearchCriteria.NEAREST);
	}

	public override RawVideoSourceDataList ImageExporterRawGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		return ImageExporterRawGetAtOrBefore(sessionId, deviceFQID, time, withHeader: true);
	}

	public override RawVideoSourceDataList ImageExporterRawGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, (IPlaybackSourceToolkit toolkit) => GetAndHandleToolkitRaw(toolkit, withHeader), IPlaybackSourceToolkit.SearchCriteria.AT_OR_BEFORE);
	}

	public override RawVideoSourceDataList ImageExporterRawGetBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		return ImageExporterRawGetBefore(sessionId, deviceFQID, time, withHeader: true);
	}

	public override RawVideoSourceDataList ImageExporterRawGetBefore(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, (IPlaybackSourceToolkit toolkit) => GetAndHandleToolkitRaw(toolkit, withHeader), IPlaybackSourceToolkit.SearchCriteria.BEFORE);
	}

	public override RawVideoSourceDataList ImageExporterRawGetAfter(object sessionId, FQID deviceFQID, DateTime time)
	{
		return ImageExporterRawGetAfter(sessionId, deviceFQID, time, withHeader: true);
	}

	public override RawVideoSourceDataList ImageExporterRawGetAfter(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		return InternalCommandServiceCommon.DataGet(sessionId, deviceFQID, time, (IPlaybackSourceToolkit toolkit) => GetAndHandleToolkitRaw(toolkit, withHeader), IPlaybackSourceToolkit.SearchCriteria.AFTER);
	}

	public override RawVideoSourceDataList ImageExporterRawCommand(object sessionId, string command)
	{
		return ImageExporterRawCommand(sessionId, command, withHeader: true);
	}

	public override RawVideoSourceDataList ImageExporterRawCommand(object sessionId, string command, bool withHeader)
	{
		return InternalCommandServiceCommon.Command(sessionId, command, (IPlaybackSourceToolkit toolkit) => GetAndHandleToolkitRaw(toolkit, withHeader));
	}

	public override void SetNewToken(object sessionId, string token)
	{
		if (sessionId is ISourceToolkit sourceToolkit)
		{
			sourceToolkit.UpdateConfiguration("token_key", token);
		}
	}

	public override void SetRemovableMasksLifted(object sessionId, bool lifted)
	{
		if (sessionId is ILiveSourceToolkit liveSourceToolkit)
		{
			liveSourceToolkit.UpdateConfiguration("removable_masks_lifted_key", lifted ? "true" : "false");
		}
	}

	public override void SetCompression(object sessionId, int quality)
	{
		if (sessionId is ISourceToolkit sourceToolkit)
		{
			sourceToolkit.UpdateConfiguration("quality_key", quality.ToString() ?? "");
		}
	}

	public override void SetCropRectangle(object sessionId, double x, double y, double width, double height)
	{
		SetCropRectangleCommon(sessionId, x, y, width, height);
	}

	private static void SetCropRectangleCommon(object sessionId, double x, double y, double width, double height)
	{
		if (sessionId is ISourceToolkit sourceToolkit)
		{
			if (x + width > 1.0)
			{
				width = 1.0 - x;
			}
			if (y + height > 1.0)
			{
				height = 1.0 - y;
			}
			if (x >= 0.0 && y >= 0.0 && width >= 0.0 && height >= 0.0 && x + width <= 1.0 && y + height <= 1.0)
			{
				sourceToolkit.UpdateConfiguration("cropx_key", x.ToString("0.000000", CultureInfo.InvariantCulture));
				sourceToolkit.UpdateConfiguration("cropy_key", y.ToString("0.000000", CultureInfo.InvariantCulture));
				sourceToolkit.UpdateConfiguration("cropwidth_key", width.ToString("0.000000", CultureInfo.InvariantCulture));
				sourceToolkit.UpdateConfiguration("cropheight_key", height.ToString("0.000000", CultureInfo.InvariantCulture));
				EnvironmentManager.Instance.Log(error: false, "SetCropRectangleCommon", "Cropheight_key=" + height.ToString("0.000000", CultureInfo.InvariantCulture));
			}
		}
	}

	public override void SetWidthHeight(object sessionId, int width, int height)
	{
		SetWidthHeight(sessionId, width, height, allowUpscaling: true);
	}

	public override void SetWidthHeight(object sessionId, int width, int height, bool allowUpscaling)
	{
		if (sessionId is ISourceToolkit sourceToolkit && width != 0 && height != 0)
		{
			sourceToolkit.UpdateConfiguration("width_key", width.ToString() ?? "");
			sourceToolkit.UpdateConfiguration("height_key", height.ToString() ?? "");
			sourceToolkit.UpdateConfiguration("decode_width_key", ConvertToDecoderDimension(width));
			sourceToolkit.UpdateConfiguration("decode_height_key", ConvertToDecoderDimension(height));
			sourceToolkit.UpdateConfiguration("allow_upscaling_key", allowUpscaling ? "true" : "false");
		}
	}

	public override void SetDecoderWidthHeight(object sessionId, int width, int height)
	{
		if (sessionId is ISourceToolkit sourceToolkit)
		{
			sourceToolkit.UpdateConfiguration("decode_width_key", ConvertToDecoderDimension(width));
			sourceToolkit.UpdateConfiguration("decode_height_key", ConvertToDecoderDimension(height));
		}
	}

	public override IAVIExporter CreateAVIExporter()
	{
		return new AVIExporter();
	}

	public override IWAVExporter CreateWAVExporter()
	{
		return new SDKWAVExporter();
	}

	public override IDBExporter CreateDBExporter()
	{
		return new DBExporterXpco();
	}

	public override IDBExporter CreateDBExporter(bool blkFormat)
	{
		return new DBExporterXpco();
	}

	public override IMKVExporter CreateMKVExporter()
	{
		return new MKVExporter();
	}

	private static TReturnType GetAndHandleToolkitRecord<TReturnType, TRecordType>(IPlaybackSourceToolkit toolkit, Func<TRecordType, string, TReturnType> convertor, Func<TReturnType, ISourceData> sourceDataProxy) where TReturnType : class where TRecordType : class, IMediaData
	{
		string decoderImplementation = "ipp";
		return InternalCommandServiceCommon.GetAndHandleToolkitMultiRecord(toolkit, delegate(IData record)
		{
			if (record.GetProperty("Decoder_Implementation") is StringProperty stringProperty)
			{
				decoderImplementation = stringProperty.Value;
			}
		}, (TRecordType record) => convertor(record, decoderImplementation), (IMediaData mediaData) => convertor(mediaData as TRecordType, decoderImplementation), (IEnumerable<TReturnType> list) => list.First(), sourceDataProxy);
	}

	private VideoOS.Platform.Data.JPEGData GetAndHandleToolkitData(IPlaybackSourceToolkit toolkit)
	{
		return GetAndHandleToolkitRecord(toolkit, delegate(IMediaData mediaData, string decoderImplementation)
		{
			byte[] mediaBody = mediaData.GetMediaBody();
			if (mediaBody != null)
			{
				VideoOS.Platform.Data.JPEGData jPEGData = new VideoOS.Platform.Data.JPEGData
				{
					Bytes = mediaBody,
					HardwareDecodingStatus = decoderImplementation
				};
				if (_sessionCustomDataDictionary.ContainsKey(toolkit))
				{
					jPEGData.CustomData = _sessionCustomDataDictionary[toolkit];
				}
				mediaData.Dispose();
				return jPEGData;
			}
			return (VideoOS.Platform.Data.JPEGData)null;
		}, (VideoOS.Platform.Data.JPEGData jpegData) => new VideoSourceDataOnJpegDataProxy(jpegData));
	}

	private static BitmapData GetAndHandleToolkitBitmap(IPlaybackSourceToolkit toolkit)
	{
		return GetAndHandleToolkitRecord(toolkit, (IImage image, string decoderImplementation) => new PlaybackBitmapData(image)
		{
			ObjectToDispose = image,
			HardwareDecodingStatus = decoderImplementation
		}, (PlaybackBitmapData bitmapData) => new VideoSourceDataOnBitmapDataProxy(bitmapData));
	}

	private RawVideoSourceDataList GetAndHandleToolkitRaw(IPlaybackSourceToolkit toolkit, bool withHeader)
	{
		bool hasMotion = false;
		long sequence = 0L;
		byte[] signature = null;
		MediaSessionInfo sessionInfo = null;
		return InternalCommandServiceCommon.GetAndHandleToolkitMultiRecord(toolkit, delegate(IData record)
		{
			lock (_sync)
			{
				if (!_mediaSessionInfos.TryGetValue(toolkit, out sessionInfo))
				{
					throw new MIPException("GetAndHandleToolkitRaw session is not registered");
				}
			}
			UpdatePrivacyMask(record.GetProperty("Privacy_Mask") as IPrivacyMaskProperty, sessionInfo);
			if (record.GetProperty("Recorded_Signatures") is ByteArrayProperty byteArrayProperty)
			{
				signature = byteArrayProperty.Value;
			}
			if (record.GetProperty("Has_Motion") is BooleanProperty booleanProperty)
			{
				hasMotion = booleanProperty.Value;
			}
		}, delegate(IMediaData data)
		{
			if (data.GetProperty("Has_Motion") is BooleanProperty booleanProperty)
			{
				hasMotion = booleanProperty.Value;
			}
			if (data.GetProperty("Sequence_Number") is Int64Property int64Property)
			{
				sequence = int64Property.Value;
			}
			RawVideoSourceData result = new RawVideoSourceData
			{
				Content = (withHeader ? data.GetData() : data.GetMediaBody()),
				PrivacyMask = sessionInfo.LastPrivacyMask,
				HasMotion = hasMotion,
				SequenceNumber = sequence
			};
			data.Dispose();
			return result;
		}, (IEnumerable<RawVideoSourceData> dataList) => new RawVideoSourceDataList
		{
			List = new List<RawVideoSourceData>(dataList),
			ObjectToDispose = null,
			Signature = signature
		}, (RawVideoSourceData rawVideoSourceData) => new SourceDataOnRawDataProxy(rawVideoSourceData));
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
			stringBuilder.Append(gridSize);
			stringBuilder.Append('x');
			stringBuilder.Append(gridSize);
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

	private static string ConvertToDecoderDimension(int dim)
	{
		if (dim != 0)
		{
			return dim.ToString();
		}
		return "-1";
	}

	private static string GetDatabasePath(FQID fqid)
	{
		string text = fqid.ServerId.ServerHostname;
		if (text.ToUpper().EndsWith("SCP"))
		{
			IDeviceInFile obj = ((((SDKConfiguration)fqid.ServerId.UserContext.Configuration).FindSystemInfo(fqid.ServerId) ?? throw new MIPException("Unable to find system for device with ObjectId:" + fqid.ObjectId.ToString())).GetDevice(fqid) as IDeviceInFile) ?? throw new MIPException("Unable to find device with ObjectId:" + fqid.ObjectId.ToString());
			if (string.IsNullOrEmpty(obj.DatabasePath))
			{
				throw new MIPException("No export database path found for device with ObjectId:" + fqid.ObjectId.ToString());
			}
			text = obj.DatabasePath;
		}
		return text;
	}

	protected override string BuildToolkitXPCODbXml(FQID fqid, int width, int height, string media, bool keepAspectRatio, bool fillSpace, bool allowUpscaling)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			string databasePath = GetDatabasePath(fqid);
			if (!Directory.Exists(databasePath))
			{
				throw new MIPException("Directory not found:" + databasePath);
			}
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			if ("RAW".Equals(media))
			{
				xmlWriter.WriteStartElement("toolkit");
				xmlWriter.WriteAttributeString("type", "source");
				xmlWriter.WriteElementString("provider", "mpms");
				xmlWriter.WriteStartElement("config");
				xmlWriter.WriteStartElement("table");
				xmlWriter.WriteAttributeString("include_archives", "yes");
				xmlWriter.WriteElementString("name", string.IsNullOrEmpty(fqid.ObjectIdString) ? fqid.ObjectId.ToString() : fqid.ObjectIdString);
				xmlWriter.WriteStartElement("bank");
				xmlWriter.WriteElementString("path", databasePath);
				LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(fqid);
				if (loginSettings != null && !string.IsNullOrEmpty(loginSettings.NetworkCredential.Password))
				{
					xmlWriter.WriteStartElement("authorization");
					xmlWriter.WriteAttributeString("method", "anonymous");
					xmlWriter.WriteElementString("password", loginSettings.NetworkCredential.Password);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
			}
			else
			{
				xmlWriter.WriteStartElement("toolkit");
				xmlWriter.WriteAttributeString("type", "source");
				xmlWriter.WriteElementString("provider", "mmp");
				xmlWriter.WriteStartElement("config");
				xmlWriter.WriteStartElement("jpeg_encoder");
				xmlWriter.WriteAttributeString("quality_update_key", "quality_key");
				WriteResizeTransform(xmlWriter, keepAspectRatio, fillSpace, width, height, allowUpscaling);
				xmlWriter.WriteStartElement("image_crop");
				xmlWriter.WriteAttributeString("offset_x", "0.0");
				xmlWriter.WriteAttributeString("offset_x_update_key", "cropx_key");
				xmlWriter.WriteAttributeString("offset_y", "0.0");
				xmlWriter.WriteAttributeString("offset_y_update_key", "cropy_key");
				xmlWriter.WriteAttributeString("width", "1.0");
				xmlWriter.WriteAttributeString("width_update_key", "cropwidth_key");
				xmlWriter.WriteAttributeString("height", "1.0");
				xmlWriter.WriteAttributeString("height_update_key", "cropheight_key");
				xmlWriter.WriteStartElement("video_decoder");
				string decoderImplementation = MediaCommonUtility.GetDecoderImplementation();
				xmlWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
				xmlWriter.WriteAttributeString("number_of_threads", MediaCommonUtility.GetDecoderThreads(_activeSessions.Count));
				xmlWriter.WriteAttributeString("removable_masks_lifted", fqid.ServerId.UserContext.PrivacyMaskLifted ? "true" : "false");
				xmlWriter.WriteAttributeString("normalize_pixel_aspect_ratio", "true");
				xmlWriter.WriteAttributeString("target_resolution_width", ConvertToDecoderDimension(width));
				xmlWriter.WriteAttributeString("target_resolution_width_update_key", "decode_width_key");
				xmlWriter.WriteAttributeString("target_resolution_height", ConvertToDecoderDimension(height));
				xmlWriter.WriteAttributeString("target_resolution_height_update_key", "decode_height_key");
				xmlWriter.WriteStartElement("toolkit");
				xmlWriter.WriteAttributeString("type", "source");
				xmlWriter.WriteElementString("provider", "mpms");
				xmlWriter.WriteStartElement("config");
				xmlWriter.WriteStartElement("table");
				xmlWriter.WriteAttributeString("include_archives", "yes");
				xmlWriter.WriteElementString("name", string.IsNullOrEmpty(fqid.ObjectIdString) ? fqid.ObjectId.ToString() : fqid.ObjectIdString);
				xmlWriter.WriteStartElement("bank");
				xmlWriter.WriteElementString("path", databasePath);
				LoginSettings loginSettings2 = LoginSettingsCache.GetLoginSettings(fqid);
				if (loginSettings2 != null && !string.IsNullOrEmpty(loginSettings2.NetworkCredential.Password))
				{
					xmlWriter.WriteStartElement("authorization");
					xmlWriter.WriteAttributeString("method", "anonymous");
					xmlWriter.WriteElementString("password", loginSettings2.NetworkCredential.Password);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				if (width != 0 && height != 0)
				{
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
			}
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	protected override string BuildToolkitIsXml(FQID fqid, int width, int height, string media, bool keepAspectRatio, bool fillSpace, bool allowUpscaling)
	{
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(fqid);
		if (loginSettings == null)
		{
			throw new MIPException("LoginSettings not found");
		}
		Uri uri = new UriBuilder(fqid.ServerId.ServerScheme, fqid.ServerId.ServerHostname, fqid.ServerId.Serverport).Uri;
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
			xmlWriter.WriteAttributeString("quality_update_key", "quality_key");
			WriteResizeTransform(xmlWriter, keepAspectRatio, fillSpace, width, height, allowUpscaling);
			xmlWriter.WriteStartElement("image_crop");
			xmlWriter.WriteAttributeString("offset_x", "0.0");
			xmlWriter.WriteAttributeString("offset_x_update_key", "cropx_key");
			xmlWriter.WriteAttributeString("offset_y", "0.0");
			xmlWriter.WriteAttributeString("offset_y_update_key", "cropy_key");
			xmlWriter.WriteAttributeString("width", "1.0");
			xmlWriter.WriteAttributeString("width_update_key", "cropwidth_key");
			xmlWriter.WriteAttributeString("height", "1.0");
			xmlWriter.WriteAttributeString("height_update_key", "cropheight_key");
			xmlWriter.WriteStartElement("video_decoder");
			string decoderImplementation = MediaCommonUtility.GetDecoderImplementation();
			xmlWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
			xmlWriter.WriteAttributeString("number_of_threads", MediaCommonUtility.GetDecoderThreads(_activeSessions.Count));
			xmlWriter.WriteAttributeString("removable_masks_lifted", fqid.ServerId.UserContext.PrivacyMaskLifted ? "true" : "false");
			xmlWriter.WriteAttributeString("removable_masks_lifted_update_key", "removable_masks_lifted_key");
			xmlWriter.WriteAttributeString("target_resolution_width", ConvertToDecoderDimension(width));
			xmlWriter.WriteAttributeString("target_resolution_width_update_key", "decode_width_key");
			xmlWriter.WriteAttributeString("target_resolution_height", ConvertToDecoderDimension(height));
			xmlWriter.WriteAttributeString("target_resolution_height_update_key", "decode_height_key");
			InternalCommandServiceCommon.WriteIsSource(xmlWriter, uri, fqid, loginSettings, media, DateTime.MinValue, DateTime.MinValue, Guid.Empty);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			if (width != 0 && height != 0)
			{
				xmlWriter.WriteEndElement();
			}
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

	private static string BuildExportToolkitMkv(string fullPath, DateTime startTime, DateTime endTime, string cameraSourceString, List<string> audioSourceStringList, bool removableMaskLifted)
	{
		string value = TimeConvert.ToGmtString(startTime, includeMilliSeconds: true);
		string value2 = TimeConvert.ToGmtString(endTime, includeMilliSeconds: true);
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "export");
			xmlWriter.WriteElementString("provider", "mkv");
			xmlWriter.WriteStartElement("config");
			xmlWriter.WriteElementString("file", fullPath);
			xmlWriter.WriteStartElement("span");
			xmlWriter.WriteAttributeString("begin", value);
			xmlWriter.WriteAttributeString("end", value2);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteElementString("removable_masks_lifted", removableMaskLifted ? "true" : "false");
			if (cameraSourceString != null)
			{
				xmlWriter.WriteStartElement("track");
				xmlWriter.WriteAttributeString("type", "video");
				xmlWriter.WriteRaw(ConfigUtil.StripXmlHeader(cameraSourceString));
				xmlWriter.WriteEndElement();
			}
			foreach (string audioSourceString in audioSourceStringList)
			{
				xmlWriter.WriteStartElement("track");
				xmlWriter.WriteAttributeString("type", "audio");
				xmlWriter.WriteRaw(ConfigUtil.StripXmlHeader(audioSourceString));
				xmlWriter.WriteEndElement();
			}
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

	private static string BuildExportToolkitXpco(Item exportItem, string fullPath, DateTime startTime, DateTime endTime, string sourceString, string privacyMask, bool reExportEnabled, bool encryptionEnabled, EncryptionStrength encryptionStrength, string encryptionPassword, bool signData, Guid trackId, bool removableMaskLifted, bool failOnInvalidSignature)
	{
		string value = TimeConvert.ToGmtString(startTime, includeMilliSeconds: true);
		string value2 = TimeConvert.ToGmtString(endTime, includeMilliSeconds: true);
		sourceString = ConfigUtil.StripXmlHeader(sourceString);
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlTextWriter.WriteStartElement("toolkit");
			xmlTextWriter.WriteAttributeString("type", "export");
			xmlTextWriter.WriteElementString("provider", "mpms");
			xmlTextWriter.WriteStartElement("config");
			xmlTextWriter.WriteStartElement("span");
			xmlTextWriter.WriteAttributeString("begin", value);
			xmlTextWriter.WriteAttributeString("end", value2);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteElementString("removable_masks_lifted", removableMaskLifted ? "true" : "false");
			xmlTextWriter.WriteStartElement("track");
			if (!string.IsNullOrWhiteSpace(privacyMask) && exportItem.FQID.Kind == Kind.Camera)
			{
				xmlTextWriter.WriteStartElement("toolkit");
				xmlTextWriter.WriteAttributeString("type", "source");
				xmlTextWriter.WriteElementString("provider", "mmp");
				xmlTextWriter.WriteStartElement("config");
				xmlTextWriter.WriteStartElement("jpeg_encoder");
				xmlTextWriter.WriteStartElement("apply_privacy_mask");
				xmlTextWriter.WriteAttributeString("mask", privacyMask);
				xmlTextWriter.WriteStartElement("video_decoder");
				string decoderImplementation = MediaCommonUtility.GetDecoderImplementation();
				xmlTextWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
				xmlTextWriter.WriteAttributeString("number_of_threads", MediaCommonUtility.GetDecoderThreads(_activeSessions.Count));
				xmlTextWriter.WriteAttributeString("removable_masks_lifted", exportItem.FQID.ServerId.UserContext.PrivacyMaskLifted ? "true" : "false");
				xmlTextWriter.WriteRaw(sourceString);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
			}
			else
			{
				xmlTextWriter.WriteRaw(sourceString);
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteElementString("bank_path", fullPath);
			xmlTextWriter.WriteStartElement("table");
			xmlTextWriter.WriteAttributeString("reexport_enabled", reExportEnabled ? "true" : "false");
			xmlTextWriter.WriteElementString("name", trackId.ToString());
			xmlTextWriter.WriteElementString("description", SecurityElement.Escape(exportItem.Name));
			xmlTextWriter.WriteStartElement("meta_data");
			xmlTextWriter.WriteStartElement("entry");
			xmlTextWriter.WriteAttributeString("key", "device_type");
			if (exportItem.FQID.Kind == Kind.Camera)
			{
				xmlTextWriter.WriteValue("Camera");
			}
			else if (exportItem.FQID.Kind == Kind.Microphone)
			{
				xmlTextWriter.WriteValue("Microphone");
			}
			else if (exportItem.FQID.Kind == Kind.Speaker)
			{
				xmlTextWriter.WriteValue("Speaker");
			}
			else if (exportItem.FQID.Kind == Kind.Metadata)
			{
				xmlTextWriter.WriteValue("Metadata");
			}
			xmlTextWriter.WriteEndElement();
			if (exportItem.FQID.Kind == Kind.Camera)
			{
				xmlTextWriter.WriteRaw(ConfigUtil.StripXmlHeader(GetPanoramicInfoForXpcoDb(exportItem.FQID)));
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			if (encryptionEnabled)
			{
				xmlTextWriter.WriteStartElement("authorization");
				xmlTextWriter.WriteAttributeString("method", "anonymous");
				xmlTextWriter.WriteStartElement("password");
				xmlTextWriter.WriteValue(encryptionPassword);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("encryption");
				switch (encryptionStrength)
				{
				case EncryptionStrength.DES56:
					xmlTextWriter.WriteAttributeString("algorithm", "DES");
					xmlTextWriter.WriteAttributeString("strength", "56");
					break;
				case EncryptionStrength.AES128:
					xmlTextWriter.WriteAttributeString("algorithm", "AES");
					xmlTextWriter.WriteAttributeString("strength", "128");
					break;
				case EncryptionStrength.AES192:
					xmlTextWriter.WriteAttributeString("algorithm", "AES");
					xmlTextWriter.WriteAttributeString("strength", "192");
					break;
				case EncryptionStrength.AES256:
					xmlTextWriter.WriteAttributeString("algorithm", "AES");
					xmlTextWriter.WriteAttributeString("strength", "256");
					break;
				}
				xmlTextWriter.WriteElementString("password", encryptionPassword);
				xmlTextWriter.WriteEndElement();
			}
			if (signData)
			{
				xmlTextWriter.WriteStartElement("signing");
				xmlTextWriter.WriteAttributeString("recorded_signatures", "include");
				xmlTextWriter.WriteAttributeString("exported_signatures", "generate");
				xmlTextWriter.WriteAttributeString("fail_on_invalid_signature", failOnInvalidSignature ? "true" : "false");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private static string GetPanoramicInfoForXpcoDb(FQID deviceFQID)
	{
		try
		{
			return (((SDKConfiguration)deviceFQID.ServerId.UserContext.Configuration).FindSystemInfo(deviceFQID.ServerId, populate: true) ?? throw new MIPException("Unable to find system for camera with ObjectId:" + deviceFQID.ObjectId.ToString())).Cameras[deviceFQID.ObjectId].GetPanoramicLensDataForXpco();
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("GetPanoramicInfoForXpcoDb", exception);
			return "";
		}
	}

	protected override string BuildToolkitXPCODbXml(FQID fqid, int width, int height, BitmapFormat bitmapFormat, bool keepAspectRatio, bool fillSpace)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			string databasePath = GetDatabasePath(fqid);
			if (!Directory.Exists(databasePath))
			{
				throw new MIPException("Directory not found:" + databasePath);
			}
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "source");
			xmlWriter.WriteElementString("provider", "mmp");
			xmlWriter.WriteStartElement("config");
			if (bitmapFormat != BitmapFormat.Unknown)
			{
				xmlWriter.WriteStartElement("image_color_format_convert");
				xmlWriter.WriteAttributeString("format", bitmapFormat.ToString());
			}
			WriteResizeTransform(xmlWriter, keepAspectRatio, fillSpace, width, height, allowUpscaling: true);
			xmlWriter.WriteStartElement("image_crop");
			xmlWriter.WriteAttributeString("offset_x", "0.0");
			xmlWriter.WriteAttributeString("offset_x_update_key", "cropx_key");
			xmlWriter.WriteAttributeString("offset_y", "0.0");
			xmlWriter.WriteAttributeString("offset_y_update_key", "cropy_key");
			xmlWriter.WriteAttributeString("width", "1.0");
			xmlWriter.WriteAttributeString("width_update_key", "cropwidth_key");
			xmlWriter.WriteAttributeString("height", "1.0");
			xmlWriter.WriteAttributeString("height_update_key", "cropheight_key");
			xmlWriter.WriteStartElement("video_decoder");
			string decoderImplementation = MediaCommonUtility.GetDecoderImplementation();
			xmlWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
			xmlWriter.WriteAttributeString("number_of_threads", MediaCommonUtility.GetDecoderThreads(_activeSessions.Count));
			xmlWriter.WriteAttributeString("removable_masks_lifted", fqid.ServerId.UserContext.PrivacyMaskLifted ? "true" : "false");
			xmlWriter.WriteAttributeString("target_resolution_width", ConvertToDecoderDimension(width));
			xmlWriter.WriteAttributeString("target_resolution_width_update_key", "decode_width_key");
			xmlWriter.WriteAttributeString("target_resolution_height", ConvertToDecoderDimension(height));
			xmlWriter.WriteAttributeString("target_resolution_height_update_key", "decode_height_key");
			xmlWriter.WriteStartElement("toolkit");
			xmlWriter.WriteAttributeString("type", "source");
			xmlWriter.WriteElementString("provider", "mpms");
			xmlWriter.WriteStartElement("config");
			xmlWriter.WriteStartElement("table");
			xmlWriter.WriteAttributeString("include_archives", "yes");
			xmlWriter.WriteElementString("name", string.IsNullOrEmpty(fqid.ObjectIdString) ? fqid.ObjectId.ToString() : fqid.ObjectIdString);
			xmlWriter.WriteStartElement("bank");
			xmlWriter.WriteElementString("path", databasePath);
			LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(fqid);
			if (loginSettings != null && !string.IsNullOrEmpty(loginSettings.NetworkCredential.Password))
			{
				xmlWriter.WriteStartElement("authorization");
				xmlWriter.WriteAttributeString("method", "anonymous");
				xmlWriter.WriteElementString("password", loginSettings.NetworkCredential.Password);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			if (width != 0 && height != 0)
			{
				xmlWriter.WriteEndElement();
			}
			if (bitmapFormat != BitmapFormat.Unknown)
			{
				xmlWriter.WriteEndElement();
			}
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

	protected override string BuildToolkitIsXml(FQID fqid, int width, int height, BitmapFormat bitmapFormat, bool keepAspectRatio, bool fillSpace)
	{
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(fqid);
		if (loginSettings == null)
		{
			throw new MIPException("LoginSettings not found");
		}
		Uri uri = new UriBuilder(fqid.ServerId.ServerScheme, fqid.ServerId.ServerHostname, fqid.ServerId.Serverport).Uri;
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
			xmlWriter.WriteAttributeString("format", bitmapFormat.ToString());
			WriteResizeTransform(xmlWriter, keepAspectRatio, fillSpace, width, height, allowUpscaling: true);
			xmlWriter.WriteStartElement("image_crop");
			xmlWriter.WriteAttributeString("offset_x", "0.0");
			xmlWriter.WriteAttributeString("offset_x_update_key", "cropx_key");
			xmlWriter.WriteAttributeString("offset_y", "0.0");
			xmlWriter.WriteAttributeString("offset_y_update_key", "cropy_key");
			xmlWriter.WriteAttributeString("width", "1.0");
			xmlWriter.WriteAttributeString("width_update_key", "cropwidth_key");
			xmlWriter.WriteAttributeString("height", "1.0");
			xmlWriter.WriteAttributeString("height_update_key", "cropheight_key");
			xmlWriter.WriteStartElement("video_decoder");
			string decoderImplementation = MediaCommonUtility.GetDecoderImplementation();
			xmlWriter.WriteAttributeString("decoder_implementation", decoderImplementation);
			xmlWriter.WriteAttributeString("number_of_threads", MediaCommonUtility.GetDecoderThreads(_activeSessions.Count));
			xmlWriter.WriteAttributeString("removable_masks_lifted", fqid.ServerId.UserContext.PrivacyMaskLifted ? "true" : "false");
			xmlWriter.WriteAttributeString("removable_masks_lifted_update_key", "removable_masks_lifted_key");
			xmlWriter.WriteAttributeString("target_resolution_width", ConvertToDecoderDimension(width));
			xmlWriter.WriteAttributeString("target_resolution_width_update_key", "decode_width_key");
			xmlWriter.WriteAttributeString("target_resolution_height", ConvertToDecoderDimension(height));
			xmlWriter.WriteAttributeString("target_resolution_height_update_key", "decode_height_key");
			InternalCommandServiceCommon.WriteIsSource(xmlWriter, uri, fqid, loginSettings, "VIDEO", DateTime.MinValue, DateTime.MinValue, Guid.Empty);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			if (width != 0 && height != 0)
			{
				xmlWriter.WriteEndElement();
			}
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

	private static void WriteResizeTransform(XmlWriter xw, bool keepAspectRatio, bool fillSpace, int width, int height, bool allowUpscaling)
	{
		if (width != 0 && height != 0)
		{
			if (keepAspectRatio && fillSpace)
			{
				xw.WriteStartElement("image_filled_resize");
			}
			else if (keepAspectRatio)
			{
				xw.WriteStartElement("image_boxed_resize");
			}
			else
			{
				xw.WriteStartElement("image_resize");
			}
			xw.WriteAttributeString("width", width.ToString() ?? "");
			xw.WriteAttributeString("width_update_key", "width_key");
			xw.WriteAttributeString("height", height.ToString() ?? "");
			xw.WriteAttributeString("height_update_key", "height_key");
			xw.WriteAttributeString("allow_upscaling", allowUpscaling ? "true" : "false");
			xw.WriteAttributeString("allow_upscaling_update_key", "allow_upscaling_key");
		}
	}

	protected override string BuildToolkitRawXPCODbXml(FQID fqid)
	{
		string databasePath = GetDatabasePath(fqid);
		if (!Directory.Exists(databasePath))
		{
			throw new MIPException("Directory not found:" + databasePath);
		}
		StringBuilder stringBuilder = new StringBuilder();
		using StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
		xmlTextWriter.WriteStartElement("toolkit");
		xmlTextWriter.WriteAttributeString("type", "source");
		xmlTextWriter.WriteElementString("provider", "mpms");
		xmlTextWriter.WriteStartElement("config");
		xmlTextWriter.WriteStartElement("table");
		xmlTextWriter.WriteAttributeString("include_archives", "yes");
		xmlTextWriter.WriteElementString("name", string.IsNullOrEmpty(fqid.ObjectIdString) ? fqid.ObjectId.ToString() : fqid.ObjectIdString);
		xmlTextWriter.WriteStartElement("bank");
		xmlTextWriter.WriteElementString("path", databasePath);
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(fqid);
		if (loginSettings != null && !string.IsNullOrEmpty(loginSettings.NetworkCredential.Password))
		{
			xmlTextWriter.WriteStartElement("authorization");
			xmlTextWriter.WriteAttributeString("method", "anonymous");
			xmlTextWriter.WriteElementString("password", loginSettings.NetworkCredential.Password);
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Flush();
		return stringBuilder.ToString();
	}
}
