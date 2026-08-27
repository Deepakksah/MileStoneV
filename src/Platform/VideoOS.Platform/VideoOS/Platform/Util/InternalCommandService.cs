using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Util;

public class InternalCommandService
{
	private static InternalCommandService _instance = new InternalCommandService();

	public static InternalCommandService Instance
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

	public virtual SequenceType[] SequencesGetTypes(FQID deviceFQID)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual void AsyncSequencesGetTypes(FQID deviceFQID, object asyncState, AsyncCallbackHandler resultCallbackHandler)
	{
		HandleNotImplemented();
	}

	public virtual SequenceEntry[] SequencesGetAroundWithSpan(FQID deviceFQID, Guid sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual SequenceEntry[] SequencesGetAroundWithSpan(FQID deviceFQID, Guid sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, bool throwException)
	{
		return SequencesGetAroundWithSpan(deviceFQID, sequenceType, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	public virtual void AsyncSequencesGetAroundWithSpan(FQID deviceFQID, object asyncState, Guid sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, AsyncCallbackHandler resultCallbackHandler)
	{
		HandleNotImplemented();
	}

	public virtual object ImageExporterConnect(FQID deviceFQID)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object ImageExporterConnect(FQID deviceFQID, int width, int height)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object ImageExporterConnect(FQID deviceFQID, int width, int height, BitmapFormat format)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object ImageExporterConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace)
	{
		return Instance.ImageExporterConnect(deviceFQID, width, height);
	}

	public virtual object ImageExporterConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, bool allowUspcaling)
	{
		return Instance.ImageExporterConnect(deviceFQID, width, height, keepAspectRatio, fillSpace);
	}

	public virtual object ImageExporterConnect(FQID deviceFQID, int width, int height, bool keepAspectRatio, bool fillSpace, BitmapFormat format)
	{
		return Instance.ImageExporterConnect(deviceFQID, width, height, format);
	}

	public virtual object ImageExporterConnect(FQID deviceFQID, string media, bool requestSignatures)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual object PcmAudioConnect(InternalMediaService.PcmAudioConnectParams pcmAudioConnectParameters)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual void ImageExporterDisconnect(object sessionId)
	{
		HandleNotImplemented();
	}

	public virtual JPEGData ImageExporterJPEGGetAt(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual JPEGData ImageExporterJPEGGetNearest(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual JPEGData ImageExporterJPEGGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual JPEGData ImageExporterCommand(object sessionId, string command)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual void ImageExporterGoTo(object sessionId, DateTime time, string direction)
	{
		HandleNotImplemented();
	}

	public virtual bool ImageExporterGoTo2(object sessionId, DateTime time, string direction)
	{
		Instance.ImageExporterGoTo(sessionId, time, direction);
		return true;
	}

	public virtual BitmapData ImageExporterBitmapGetAt(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual BitmapData ImageExporterBitmapGetNearest(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual BitmapData ImageExporterBitmapGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual BitmapData ImageExporterBitmapCommand(object sessionId, string command)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetAt(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetAt(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetNearest(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetNearest(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawCommand(object sessionId, string command)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawCommand(object sessionId, string command, bool withHeader)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		Instance.ImageExporterRawGetAt(sessionId, deviceFQID, time);
		return Instance.ImageExporterRawCommand(sessionId, "Previous");
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetBefore(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		Instance.ImageExporterRawGetAt(sessionId, deviceFQID, time, withHeader);
		return Instance.ImageExporterRawCommand(sessionId, "Previous", withHeader);
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetAfter(object sessionId, FQID deviceFQID, DateTime time)
	{
		Instance.ImageExporterRawGetAt(sessionId, deviceFQID, time);
		return Instance.ImageExporterRawCommand(sessionId, "Next");
	}

	public virtual RawVideoSourceDataList ImageExporterRawGetAfter(object sessionId, FQID deviceFQID, DateTime time, bool withHeader)
	{
		Instance.ImageExporterRawGetAt(sessionId, deviceFQID, time, withHeader);
		return Instance.ImageExporterRawCommand(sessionId, "Next", withHeader);
	}

	public virtual PcmAudioDataList PcmAudioGetAt(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual PcmAudioDataList PcmAudioGetNearest(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual PcmAudioDataList PcmAudioGetAtOrBefore(object sessionId, FQID deviceFQID, DateTime time)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual PcmAudioDataList PcmAudioCommand(object sessionId, string command)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual void SetNewToken(object sessionId, string token)
	{
		HandleNotImplemented();
	}

	public virtual void SetRemovableMasksLifted(object sessionId, bool lifted)
	{
	}

	public virtual void SetWidthHeight(object sessionId, int width, int height)
	{
		HandleNotImplemented();
	}

	public virtual void SetWidthHeight(object sessionId, int width, int height, bool allowUpscaling)
	{
		Instance.SetWidthHeight(sessionId, width, height);
	}

	public virtual void SetCropRectangle(object sessionId, double x, double y, double width, double height)
	{
		HandleNotImplemented();
	}

	public virtual void SetCompression(object sessionId, int quality)
	{
		HandleNotImplemented();
	}

	public virtual void SetCustomData(object sessionId, object customData)
	{
	}

	public virtual void SetDecoderWidthHeight(object sessionId, int width, int height)
	{
	}

	public virtual IDBExporter CreateDBExporter()
	{
		HandleNotImplemented();
		return null;
	}

	public virtual IDBExporter CreateDBExporter(bool blkFormat)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual IAVIExporter CreateAVIExporter()
	{
		HandleNotImplemented();
		return null;
	}

	public virtual IWAVExporter CreateWAVExporter()
	{
		HandleNotImplemented();
		return null;
	}

	public virtual IMKVExporter CreateMKVExporter()
	{
		HandleNotImplemented();
		return null;
	}

	public virtual ViewAndLayoutItem CreateWorkSpaceViewAndLayoutItem(Guid workSpacePluginId)
	{
		return null;
	}

	public virtual Guid SmartSearchStart(FQID deviceFQID, DateTime searchBeginTime, DateTime searchEndTime, int sensitivity, TimeSpan searchInterval, SmartSearchMask searchMask, bool ignoreInitialMotion, SmartSearchSize resultMotionMaskSize, bool throwExceptions)
	{
		HandleNotImplemented();
		return Guid.Empty;
	}

	public virtual void SmartSearchCancel(FQID deviceFQID, Guid smartSearchId, bool throwExceptions)
	{
		HandleNotImplemented();
	}

	public virtual SmartSearchStatus SmartSearchGetStatus(FQID deviceFQID, Guid smartSearchId, bool throwExceptions)
	{
		HandleNotImplemented();
		return null;
	}

	public virtual SmartSearchResult SmartSearchGetResult(FQID deviceFQID, Guid smartSearchId, bool continueSearch, bool throwExceptions)
	{
		HandleNotImplemented();
		return null;
	}

	private static void HandleNotImplemented()
	{
		if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Standalone)
		{
			EnvironmentManager.Instance.Log(error: true, "TryLoad", "VideoOS.Platform.SDK.Export.Environment.Initialize() not called.", null);
			throw new MIPException("VideoOS.Platform.SDK.Export.Environment.Initialize() not called.");
		}
		throw new MIPException("Method not supported for this Environment");
	}
}
