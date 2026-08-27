using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Services.Protocols;
using VideoOS.RecorderWebservice.Shared.Status2;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Plugin.Communication;

public static class RecorderStatusService
{
	public static List<DeviceStatus> GetDeviceStatus(string token, DeviceOverview[] deviceOverviewList, IServiceLogger serviceLogger)
	{
		Dictionary<string, Dictionary<Guid, DeviceOverview>> dictionary = new Dictionary<string, Dictionary<Guid, DeviceOverview>>();
		DeviceOverview[] array = deviceOverviewList;
		foreach (DeviceOverview deviceOverview in array)
		{
			if (!dictionary.ContainsKey(deviceOverview.RecorderWebserverUri))
			{
				dictionary.Add(deviceOverview.RecorderWebserverUri, new Dictionary<Guid, DeviceOverview>());
			}
			dictionary[deviceOverview.RecorderWebserverUri].Add(deviceOverview.DeviceId, deviceOverview);
		}
		Dictionary<Guid, DeviceStatus> dictionary2 = new Dictionary<Guid, DeviceStatus>();
		IRecorderStatusService2 recorderStatusService = null;
		Unit[] units = new Unit[4]
		{
			Unit.Bytes,
			Unit.KB,
			Unit.MB,
			Unit.GB
		};
		foreach (string key in dictionary.Keys)
		{
			if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(key))
			{
				dictionary2 = CreateUnknownStatus(dictionary, key);
				continue;
			}
			try
			{
				recorderStatusService = GetRecorderStatusServiceProxy(key);
				VideoDeviceStatistics[] videoDeviceStatistics = recorderStatusService.GetVideoDeviceStatistics(token, dictionary[key].Keys.ToArray());
				Dictionary<Guid, VideoDeviceStatistics> dictionary3 = new Dictionary<Guid, VideoDeviceStatistics>();
				VideoDeviceStatistics[] array2 = videoDeviceStatistics;
				foreach (VideoDeviceStatistics videoDeviceStatistics2 in array2)
				{
					dictionary3.Add(videoDeviceStatistics2.DeviceId, videoDeviceStatistics2);
				}
				CameraDeviceStatus[] cameraDeviceStatusArray = recorderStatusService.GetCurrentDeviceStatus(token, dictionary[key].Keys.ToArray()).CameraDeviceStatusArray;
				foreach (CameraDeviceStatus cameraDeviceStatus in cameraDeviceStatusArray)
				{
					DeviceStatus deviceStatus = new DeviceStatus(cameraDeviceStatus.DeviceId);
					deviceStatus.DeviceName = dictionary[key][cameraDeviceStatus.DeviceId].DeviceName;
					deviceStatus.DeviceHostName = dictionary[key][cameraDeviceStatus.DeviceId].DeviceHostName;
					deviceStatus.RecordingServerName = dictionary[key][cameraDeviceStatus.DeviceId].RecordingServerName;
					deviceStatus.RecordingStorageName = dictionary[key][cameraDeviceStatus.DeviceId].RecordingStorageName;
					deviceStatus.DataCollectorId = dictionary[key][cameraDeviceStatus.DeviceId].DataCollectorId;
					deviceStatus.RecorderId = dictionary[key][cameraDeviceStatus.DeviceId].RecorderId;
					deviceStatus.Enabled = cameraDeviceStatus.Enabled;
					deviceStatus.ErrorState = GetErrorState(cameraDeviceStatus);
					deviceStatus.DbMoveInProgress = cameraDeviceStatus.DbMoveInProgress;
					deviceStatus.DbRepairInProgress = cameraDeviceStatus.DbRepairInProgress;
					deviceStatus.Motion = cameraDeviceStatus.Motion;
					deviceStatus.Recording = cameraDeviceStatus.Recording;
					deviceStatus.Started = cameraDeviceStatus.Started;
					VideoDeviceStatistics value = null;
					if (dictionary3.TryGetValue(cameraDeviceStatus.DeviceId, out value))
					{
						(float, Unit) calculatedUnit = DataUnitUtility.GetCalculatedUnit(value.UsedSpaceInBytes, units, 999uL, 1024u);
						deviceStatus.UsedSpace = calculatedUnit.Item1;
						deviceStatus.UsedSpaceUnit = calculatedUnit.Item2;
						deviceStatus.UsedSpaceInBytesRaw = value.UsedSpaceInBytes;
						ulong num = 0uL;
						double defaultLiveStreamFpsValue = 0.0;
						double recordingStreamFpsValue = 0.0;
						VideoFormat defaultLiveStreamFormat = VideoFormat.VideoFormatUnknown;
						VideoFormat recordingStreamFormat = VideoFormat.VideoFormatUnknown;
						VideoStreamStatistics[] videoStreamStatisticsArray = value.VideoStreamStatisticsArray;
						foreach (VideoStreamStatistics videoStreamStatistics in videoStreamStatisticsArray)
						{
							VideoFormat videoFormat = VideoFormat.VideoFormatUnknown;
							switch (videoStreamStatistics.VideoFormat)
							{
							case "JPEG":
								videoFormat = VideoFormat.VideoFormatJpeg;
								break;
							case "MPEG":
								videoFormat = VideoFormat.VideoFormatMpeg;
								break;
							case "MPEG4":
								videoFormat = VideoFormat.VideoFormatMpeg4;
								break;
							case "VC1":
								videoFormat = VideoFormat.VideoFormatVC1;
								break;
							case "H263":
								videoFormat = VideoFormat.VideoFormatH263;
								break;
							case "H264":
								videoFormat = VideoFormat.VideoFormatH264;
								break;
							case "H265":
								videoFormat = VideoFormat.VideoFormatH265;
								break;
							}
							num += videoStreamStatistics.BPS;
							if (videoStreamStatistics.LiveStreamDefault)
							{
								defaultLiveStreamFpsValue = videoStreamStatistics.FPS;
								defaultLiveStreamFormat = videoFormat;
							}
							if (videoStreamStatistics.RecordingStream)
							{
								recordingStreamFpsValue = videoStreamStatistics.FPS;
								recordingStreamFormat = videoFormat;
							}
						}
						if (deviceStatus.ErrorState != ErrorState.StatusUnknown && deviceStatus.Enabled && deviceStatus.Started)
						{
							deviceStatus.KbpsValue = (float)(num / 1024);
							deviceStatus.DefaultLiveStreamFormat = defaultLiveStreamFormat;
							deviceStatus.DefaultLiveStreamFpsValue = defaultLiveStreamFpsValue;
							deviceStatus.RecordingStreamFormat = recordingStreamFormat;
							deviceStatus.RecordingStreamFpsValue = recordingStreamFpsValue;
						}
					}
					dictionary2.Add(deviceStatus.DeviceId, deviceStatus);
				}
			}
			catch (Exception ex)
			{
				dictionary2 = CreateUnknownStatus(dictionary, key);
				recorderStatusService?.Abort();
				serviceLogger.LogToServerLog(ex, "", LogType.Error);
			}
		}
		array = deviceOverviewList;
		foreach (DeviceOverview deviceOverview2 in array)
		{
			if (!dictionary2.ContainsKey(deviceOverview2.DeviceId))
			{
				dictionary2.Add(deviceOverview2.DeviceId, CreateUnknownStatusElement(deviceOverview2));
			}
		}
		return dictionary2.Values.ToList();
	}

	private static ErrorState GetErrorState(CameraDeviceStatus cameraStatus)
	{
		if (!cameraStatus.Enabled)
		{
			return ErrorState.StatusDisabled;
		}
		if (cameraStatus.Error)
		{
			return ErrorState.StatusError;
		}
		if (!cameraStatus.Started)
		{
			return ErrorState.StatusStopped;
		}
		return ErrorState.StatusOk;
	}

	public static RecorderStatus GetRecorderStatus(string token, string recorderUri, IServiceLogger serviceLogger)
	{
		RecorderStatus statusUnknown = RecorderStatus.StatusUnknown;
		if (string.IsNullOrEmpty(token))
		{
			return statusUnknown;
		}
		if (string.IsNullOrEmpty(recorderUri))
		{
			return RecorderStatus.StatusServerUnavailable;
		}
		IRecorderStatusService2 recorderStatusService = null;
		try
		{
			recorderStatusService = GetRecorderStatusServiceProxy(recorderUri);
			AttachAndConnectionState recorderStatus = recorderStatusService.GetRecorderStatus(token);
			if (!Enum.TryParse<RecorderConnectionState>(recorderStatus.ConnectionState, out var result))
			{
				result = RecorderConnectionState.Unknown;
			}
			if (!Enum.TryParse<RecorderAttachState>(recorderStatus.AttachState, out var result2))
			{
				result2 = RecorderAttachState.Unknown;
			}
			statusUnknown = new RecorderStatus(result2, result);
		}
		catch (Exception ex)
		{
			recorderStatusService?.Abort();
			statusUnknown = RecorderStatus.StatusServerUnavailable;
			if (ex is WebException)
			{
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "GetRecorderStatus for '{0}' failed: {1}", recorderUri, ex.Message), LogType.Error);
			}
			else
			{
				serviceLogger.LogToServerLog(ex, null, LogType.Error);
				if (ex is SoapException && ex.Message == "Token invalid")
				{
					serviceLogger.LogToServerLog("Invalid token response received in GetRecorderStatus(), refreshing token now...", LogType.Information);
					ServerCommandServiceTokenAndUser.RefreshAppPoolSecurityToken(serviceLogger);
				}
			}
		}
		return statusUnknown;
	}

	private static Dictionary<Guid, DeviceStatus> CreateUnknownStatus(Dictionary<string, Dictionary<Guid, DeviceOverview>> recorderDeviceDic, string recorderUri)
	{
		Dictionary<Guid, DeviceStatus> dictionary = new Dictionary<Guid, DeviceStatus>();
		foreach (Guid key in recorderDeviceDic[recorderUri].Keys)
		{
			DeviceStatus value = CreateUnknownStatusElement(recorderDeviceDic[recorderUri][key]);
			dictionary.Add(key, value);
		}
		return dictionary;
	}

	private static DeviceStatus CreateUnknownStatusElement(DeviceOverview devInfo)
	{
		return new DeviceStatus(devInfo.DeviceId)
		{
			DeviceName = devInfo.DeviceName,
			DeviceHostName = devInfo.DeviceHostName,
			RecordingServerName = devInfo.RecordingServerName,
			RecordingStorageName = devInfo.RecordingStorageName,
			ErrorState = ErrorState.StatusUnknown,
			RecorderId = devInfo.RecorderId,
			DataCollectorId = devInfo.DataCollectorId
		};
	}

	private static IRecorderStatusService2 GetRecorderStatusServiceProxy(string webServerUrl)
	{
		return new RecorderStatusServiceProxy(webServerUrl);
	}
}
