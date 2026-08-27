using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using VideoOS.LPR.Common.UI.LPRServiceStatus;
using VideoOS.LPR.Common.UI.Resources;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.LPR.Plugin.ClientResources;

namespace VideoOS.LPR.Common.UI;

public class LPRServiceStatusHelper
{
	public string StatusText { get; private set; }

	public bool Error { get; private set; }

	public bool Warning { get; private set; }

	public string UptimeText { get; private set; }

	public string CpuUsageText { get; private set; }

	public string MemoryAvailableText { get; private set; }

	public string LicensePlatesDetectedText { get; private set; }

	public ReadOnlyCollection<VideoSourceStatusModel> VideoSourceStatusModels { get; private set; }

	public LPRServiceStatusHelper(GetStatusResp statusResponse, CultureInfo culture)
	{
		StatusText = string.Empty;
		Error = false;
		Warning = false;
		UptimeText = string.Empty;
		CpuUsageText = string.Empty;
		MemoryAvailableText = string.Empty;
		LicensePlatesDetectedText = string.Empty;
		if (statusResponse == null)
		{
			Error = true;
			StatusText = ClientStrings.ResourceManager.GetString("ServiceStatusWebServiceNotRespondingError", culture);
			return;
		}
		VideoSourceStatus[] videoSourceStatuses;
		if (statusResponse.Errors != ServiceErrors.None)
		{
			Error = true;
			if (statusResponse.Errors.HasFlag(ServiceErrors.SurveillanceSystemConnectionError))
			{
				StatusText = ClientStrings.ResourceManager.GetString("ServiceStatusSurveillanceSystemConnectionError", culture);
			}
			else if (statusResponse.Errors.HasFlag(ServiceErrors.WebServiceError))
			{
				StatusText = ClientStrings.ResourceManager.GetString("ServiceStatusWebServiceError", culture);
			}
			else if (statusResponse.Errors.HasFlag(ServiceErrors.LicensePlatesPollError))
			{
				StatusText = ClientStrings.ResourceManager.GetString("ServiceStatusLicensePlatesPollError", culture);
			}
			else if (statusResponse.Errors.HasFlag(ServiceErrors.RunningOnOtherSystem))
			{
				StatusText = ClientStrings.ResourceManager.GetString("ServiceStatusLPRServerRegisteredOnAnotherSystem", culture);
			}
			else
			{
				StatusText = ClientStrings.ResourceManager.GetString("ServiceStatusUnknownError", culture);
			}
		}
		else
		{
			int num = 0;
			int num2 = 0;
			if (statusResponse.VideoSourceStatuses != null)
			{
				videoSourceStatuses = statusResponse.VideoSourceStatuses;
				for (int i = 0; i < videoSourceStatuses.Length; i++)
				{
					if (videoSourceStatuses[i].Errors.ToString().StartsWith("Error", StringComparison.Ordinal))
					{
						num2++;
					}
					else
					{
						num++;
					}
				}
			}
			if (num2 > 0)
			{
				Warning = true;
				StatusText = string.Format(CultureInfo.CurrentCulture, ClientStrings.ResourceManager.GetString("ServiceStatusVideoSourcesRunning", culture), num, num2 + num);
			}
			else if (num > 0)
			{
				StatusText = ClientStrings.ResourceManager.GetString("ServiceStatusOK", culture);
			}
			else
			{
				StatusText = ClientStrings.ResourceManager.GetString("ServiceStatusOKNoVideoSources", culture);
			}
		}
		if (statusResponse.Status == null)
		{
			return;
		}
		TimeSpan timeSpan = DateTime.UtcNow - statusResponse.Status.ServiceStartTime.ToUniversalTime();
		UptimeText = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds).ToString("c", CultureInfo.CurrentCulture);
		CpuUsageText = string.Format(CultureInfo.CurrentCulture, Strings.ServiceStatusCpuUsageText, statusResponse.Status.MachineCpuUsed);
		MemoryAvailableText = string.Format(CultureInfo.CurrentCulture, Strings.ServiceStatusMemoryAvailableText, statusResponse.Status.MachineMemoryAvailable / 1048576.0);
		LicensePlatesDetectedText = statusResponse.Status.TotalDetectedLicensePlates.ToString(CultureInfo.CurrentCulture);
		List<VideoSourceStatusModel> list = new List<VideoSourceStatusModel>();
		videoSourceStatuses = statusResponse.VideoSourceStatuses;
		foreach (VideoSourceStatus videoSourceStatus in videoSourceStatuses)
		{
			VideoSourceStatusModel videoSourceStatusModel = new VideoSourceStatusModel
			{
				Text = videoSourceStatus.Name,
				ErrorText = string.Empty
			};
			switch (videoSourceStatus.Errors)
			{
			default:
			{
				videoSourceStatusModel.State = VideoSourceStatusModelState.Ok;
				string format = ClientStrings.ResourceManager.GetString("VideoSourceStatusFPS", culture);
				videoSourceStatusModel.ErrorText = "- " + string.Format(CultureInfo.CurrentCulture, format, videoSourceStatus.FrameRate);
				break;
			}
			case VideoSourceError.ErrorUnspecified:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorUnspecified", culture);
				break;
			case VideoSourceError.WarningUnspecified:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Warning;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusWarningUnspecified", culture);
				break;
			case VideoSourceError.WarningIntradaProcessWarning:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Warning;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusWarningIntradaProcessWarning", culture);
				break;
			case VideoSourceError.ErrorCameraNotAvailable:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorCameraNotAvailable", culture);
				break;
			case VideoSourceError.ErrorVideoSourceLicenseInvalid:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorVideoSourceLicenseInvalid", culture);
				break;
			case VideoSourceError.ErrorVideoSourceLicenseExpired:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorVideoSourceLicenseExpired", culture);
				break;
			case VideoSourceError.ErrorCountryModuleLicenseInvalid:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorCountryModuleLicenseInvalid", culture);
				break;
			case VideoSourceError.ErrorCountryModuleLicenseExpired:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorCountryModuleLicenseExpired", culture);
				break;
			case VideoSourceError.ErrorFailedToStartIntradaProcess:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorFailedToStartIntradaProcess", culture);
				break;
			case VideoSourceError.ErrorFailedToStartIntradaProcessNoCountry:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorFailedToStartIntradaProcessNoCountry", culture);
				break;
			case VideoSourceError.ErrorIntradaProcessError:
				videoSourceStatusModel.State = VideoSourceStatusModelState.Error;
				videoSourceStatusModel.ErrorText = "- " + ClientStrings.ResourceManager.GetString("VideoSourceStatusErrorIntradaProcessError", culture);
				break;
			}
			list.Add(videoSourceStatusModel);
		}
		VideoSourceStatusModels = new ReadOnlyCollection<VideoSourceStatusModel>(list);
	}

	public static bool AreStatusErrorsEqual(GetStatusResp statusResponse1, GetStatusResp statusResponse2)
	{
		if (statusResponse1 == statusResponse2)
		{
			return true;
		}
		if (statusResponse1 == null || statusResponse2 == null || statusResponse1.Errors != statusResponse2.Errors)
		{
			return false;
		}
		if (statusResponse1.VideoSourceStatuses == statusResponse2.VideoSourceStatuses)
		{
			return true;
		}
		if (statusResponse1.VideoSourceStatuses == null || statusResponse2.VideoSourceStatuses == null || statusResponse1.VideoSourceStatuses.Length != statusResponse2.VideoSourceStatuses.Length)
		{
			return false;
		}
		Dictionary<Guid, VideoSourceStatus> dictionary = new Dictionary<Guid, VideoSourceStatus>();
		VideoSourceStatus[] videoSourceStatuses = statusResponse1.VideoSourceStatuses;
		foreach (VideoSourceStatus videoSourceStatus in videoSourceStatuses)
		{
			dictionary.Add(videoSourceStatus.Id, videoSourceStatus);
		}
		videoSourceStatuses = statusResponse2.VideoSourceStatuses;
		foreach (VideoSourceStatus videoSourceStatus2 in videoSourceStatuses)
		{
			if (!dictionary.TryGetValue(videoSourceStatus2.Id, out var value) || value.Errors != videoSourceStatus2.Errors)
			{
				return false;
			}
		}
		return true;
	}
}
