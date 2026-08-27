using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents;
using VideoOS.Server.Properties;
using VideoOS.Utilities.Telemetry;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server;

public sealed class NotificationManager
{
	private class NotificationData
	{
		public SendNotificationCommand cmd;

		public DateTime nextRun;

		public string Key => cmd.IDNotificationProfile.ToString() + cmd.RuleId.ToString() + cmd.TriggerId.ToString() + cmd.EventTypeId.ToString() + cmd.CameraId.ToString();

		public NotificationData(SendNotificationCommand cmd)
		{
			this.cmd = cmd;
		}
	}

	private Server _server;

	private Hashtable _notifications;

	public static readonly ILogger Logger = Telemetry.GetILogger("NotificationManager");

	public NotificationManager(Server server)
	{
		_server = server;
		_notifications = new Hashtable();
	}

	public void SendNotification(SendNotificationCommand cmd)
	{
		Logger.LogInformation("New notification received from Device '{CameraId}'", cmd.CameraId);
		SecurityToken localSecurityToken = _server.GetLocalSecurityToken(ServerConfig.Instance.NotificationTokenTimeout);
		MailProcessor mailProcessor = new MailProcessor(localSecurityToken);
		try
		{
			ApplyConfiguration(mailProcessor, cmd);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			return;
		}
		if (!NotificationIntervalCheck(cmd, mailProcessor.TimeBetweenMails))
		{
			return;
		}
		try
		{
			mailProcessor.BuildAndSendMail(cmd);
		}
		catch (WebException ex3)
		{
			Logger.LogError("Send mail exception: {Exception}", ex3);
			mailProcessor.Recipients = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", mailProcessor.SMTPServer, mailProcessor.Recipients);
			string text = FindRecorderNameFromDevice(cmd.CameraId);
			ServerLogManager.Instance.AddLogEntry(LogEntryId.EMailErrorRecorderUnavailable, mailProcessor.Recipients, text);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex5)
		{
			Logger.LogError("Send mail exception: {Exception}", ex5);
			mailProcessor.Recipients = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", mailProcessor.SMTPServer, mailProcessor.Recipients);
			string text2 = ((ex5.InnerException == null) ? ex5.Message : (". " + ex5.InnerException.Message));
			ServerLogManager.Instance.AddLogEntry(LogEntryId.EMailError, mailProcessor.Recipients + text2, text2);
		}
	}

	public void SendTestEmailNotification(MailGeneratorData mailGeneratorData)
	{
		MailGenerator mailGenerator;
		using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_server.Data)))
		{
			string username = FindSystemSetting(configurationManager, SettingList.SmtpUserName);
			string password = FindSystemSetting(configurationManager, SettingList.SmtpPassword);
			mailGeneratorData.SetLoginMode(username, password);
			mailGenerator = new MailGenerator(mailGeneratorData);
			SystemSetting systemSetting = configurationManager.SystemSettings[SettingList.AVICodec];
			if (systemSetting == null)
			{
				mailGenerator.SetError(MailMediaError.MailMediaErrorNoCodecSelected);
			}
		}
		mailGenerator.SendMail();
	}

	private void ApplyConfiguration(MailProcessor mailProcessor, SendNotificationCommand cmd)
	{
		_server.Data.ConfigurationDataSetCacheLock.EnterReadLock();
		try
		{
			using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_server.Data));
			NotificationSetting notificationSetting = FindNotificationSetting(configurationManager, cmd.IDNotificationProfile);
			if (notificationSetting == null)
			{
				throw new Exception("Could not find notification profile");
			}
			mailProcessor.TimeBetweenMails = Convert.ToInt32(notificationSetting.Settings.Get("TimeBetweenMails"), CultureInfo.InvariantCulture);
			mailProcessor.IncludeImages = Convert.ToBoolean(notificationSetting.Settings.Get("IncludeImages"), CultureInfo.InvariantCulture);
			mailProcessor.NumberOfImages = Convert.ToInt32(notificationSetting.Settings.Get("NumberOfImages"), CultureInfo.InvariantCulture);
			mailProcessor.EmbedImages = Convert.ToBoolean(notificationSetting.Settings.Get("EmbedImages"), CultureInfo.InvariantCulture);
			mailProcessor.TimeBetweenImages = Convert.ToInt32(notificationSetting.Settings.Get("TimeBetweenImages"), CultureInfo.InvariantCulture);
			mailProcessor.IncludeAvi = Convert.ToBoolean(notificationSetting.Settings.Get("IncludeAVI"), CultureInfo.InvariantCulture);
			mailProcessor.TimeBeforeEvent = Convert.ToInt32(notificationSetting.Settings.Get("TimeBeforeEvent"), CultureInfo.InvariantCulture);
			mailProcessor.TimeAfterEvent = Convert.ToInt32(notificationSetting.Settings.Get("TimeAfterEvent"), CultureInfo.InvariantCulture);
			mailProcessor.Framerate = Convert.ToDouble(notificationSetting.Settings.Get("FrameRate"), CultureInfo.InvariantCulture);
			mailProcessor.TriggeringName = FindTriggeringName(cmd.EventTypeId, cmd.TriggerId);
			mailProcessor.Recipients = notificationSetting.Settings.Get("Recipients");
			SourceType sourceType = FindEventSourceTypeName(cmd.EventTypeId);
			string empty = string.Empty;
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			string newValue = string.Empty;
			string newValue2 = FindRuleName(cmd.RuleId);
			switch (sourceType)
			{
			case SourceType.Hardware:
				empty = FindHardwareName(cmd.TriggerId);
				empty2 = FindDeviceName(cmd.CameraId);
				empty3 = FindRecorderNameFromDevice(cmd.CameraId);
				break;
			case SourceType.Device:
				empty = FindHardwareNameFromDevice(cmd.TriggerId);
				empty2 = FindDeviceName(cmd.TriggerId);
				empty3 = FindRecorderNameFromDevice(cmd.TriggerId);
				break;
			default:
				empty = FindHardwareNameFromDevice(cmd.CameraId);
				empty2 = mailProcessor.TriggeringName;
				empty3 = FindRecorderNameFromDevice(cmd.CameraId);
				newValue = FindAcUnitName(cmd.TriggerId);
				break;
			}
			if (string.IsNullOrEmpty(empty3))
			{
				empty3 = FindRecorderNameFromRecoderId(cmd.RecorderId);
			}
			if (string.IsNullOrEmpty(empty3))
			{
				empty3 = FindRecorderNameFromRecoderId(cmd.TriggerId);
			}
			mailProcessor.Subject = notificationSetting.Settings.Get("Subject");
			mailProcessor.Subject = mailProcessor.Subject.Replace("$RecorderName$", empty3);
			mailProcessor.Subject = mailProcessor.Subject.Replace("$HardwareName$", empty);
			mailProcessor.Subject = mailProcessor.Subject.Replace("$DeviceName$", empty2);
			mailProcessor.Subject = mailProcessor.Subject.Replace("$RuleName$", newValue2);
			mailProcessor.Subject = mailProcessor.Subject.Replace("$AccessUnitName$", newValue);
			mailProcessor.Subject = ReplaceTriggerTimeWithFormattedDate(mailProcessor.Subject, cmd.Time);
			mailProcessor.CustomMessage = notificationSetting.Settings.Get("Text");
			mailProcessor.CustomMessage = mailProcessor.CustomMessage.Replace("$RecorderName$", empty3);
			mailProcessor.CustomMessage = mailProcessor.CustomMessage.Replace("$HardwareName$", empty);
			mailProcessor.CustomMessage = mailProcessor.CustomMessage.Replace("$DeviceName$", empty2);
			mailProcessor.CustomMessage = mailProcessor.CustomMessage.Replace("$RuleName$", newValue2);
			mailProcessor.CustomMessage = mailProcessor.CustomMessage.Replace("$AccessUnitName$", newValue);
			mailProcessor.CustomMessage = ReplaceTriggerTimeWithFormattedDate(mailProcessor.CustomMessage, cmd.Time);
			mailProcessor.MediaDeviceName = FindDeviceName(cmd.CameraId);
			if (string.IsNullOrEmpty(mailProcessor.MediaDeviceName))
			{
				mailProcessor.MediaDeviceName = "-";
			}
			mailProcessor.SMTPServer = FindSystemSetting(configurationManager, SettingList.SmtpServer);
			mailProcessor.SMTPLogon = FindSystemSetting(configurationManager, SettingList.SmtpLogOn);
			mailProcessor.SMTPUsername = FindSystemSetting(configurationManager, SettingList.SmtpUserName);
			mailProcessor.SMTPPassword = FindSystemSetting(configurationManager, SettingList.SmtpPassword);
			mailProcessor.SMTPSender = FindSystemSetting(configurationManager, SettingList.SmtpSender);
			mailProcessor.SMTPUseSsl = FindSystemSettingBool(configurationManager, SettingList.SmtpUseSsl);
			mailProcessor.SMTPPort = FindSystemSettingInt(configurationManager, SettingList.SmtpPort);
			if (!mailProcessor.IncludeAvi)
			{
				return;
			}
			mailProcessor.AVICodec = FindSystemSetting(configurationManager, SettingList.AVICodec);
			if (!string.IsNullOrEmpty(mailProcessor.AVICodec))
			{
				mailProcessor.CompressorHandler = Convert.ToInt32(mailProcessor.AVICodec, CultureInfo.InvariantCulture);
				Compressor compressor = FindCompressor(configurationManager, mailProcessor.CompressorHandler);
				if (compressor != null)
				{
					mailProcessor.CompressorFlags = compressor.Flags;
					mailProcessor.CompressorQuality = compressor.Quality;
					mailProcessor.CompressorDataRate = compressor.DataRate;
					mailProcessor.CompressorKeyFrames = compressor.KeyFrames;
					mailProcessor.CompressorConfigData = compressor.ConfigData;
					mailProcessor.ValidAVIInfo = true;
				}
			}
		}
		finally
		{
			_server.Data.ConfigurationDataSetCacheLock.ExitReadLock();
		}
	}

	private static string ReplaceTriggerTimeWithFormattedDate(string textToFormat, DateTime dateTime)
	{
		string pattern = "\\$\\#(.*?)\\#\\$";
		MatchCollection matchCollection = Regex.Matches(textToFormat, pattern);
		foreach (Match item in matchCollection)
		{
			try
			{
				string newValue = dateTime.ToLocalTime().ToString(item.Groups[1].Value, CultureInfo.CurrentCulture);
				textToFormat = textToFormat.Replace(item.Value, newValue);
			}
			catch (FormatException ex)
			{
				textToFormat = textToFormat.Replace(item.Value, dateTime.ToLocalTime().ToString(CultureInfo.CurrentCulture));
				Logger.LogError("TriggerTime format is incorrect. {Exception}", ex);
			}
		}
		textToFormat = textToFormat.Replace("$TriggerTime$", dateTime.ToLocalTime().ToString(CultureInfo.CurrentCulture));
		return textToFormat;
	}

	private bool NotificationIntervalCheck(SendNotificationCommand cmd, int timeBetweenMails)
	{
		NotificationData notificationData = new NotificationData(cmd);
		lock (_notifications)
		{
			if (_notifications.ContainsKey(notificationData.Key))
			{
				Logger.LogDebug("Notification for device '{CameraId}' already registered", cmd.CameraId);
				notificationData = (NotificationData)_notifications[notificationData.Key];
				notificationData.cmd = cmd;
			}
			else
			{
				Logger.LogDebug("Notification registered for device '{CameraId}'", cmd.CameraId);
				notificationData.nextRun = cmd.Time;
				_notifications[notificationData.Key] = notificationData;
			}
			if (notificationData.nextRun > cmd.Time)
			{
				Logger.LogDebug("Returning, notification not yet allowed for device '{CameraId}'", cmd.CameraId);
				return false;
			}
		}
		notificationData.nextRun = cmd.Time.AddSeconds(timeBetweenMails);
		Logger.LogDebug("Running notification for device '{CameraId}', scheduling next run: {NextRun}", cmd.CameraId, notificationData.nextRun.ToLongTimeString());
		return true;
	}

	private string FindTriggeringName(Guid eventTypeId, Guid triggerId)
	{
		try
		{
			if (triggerId == SourceGuid.External)
			{
				string text = FindEventTypeName(eventTypeId);
				if (string.IsNullOrEmpty(text))
				{
					return string.Empty;
				}
				return string.Format(CultureInfo.InvariantCulture, Resources.NotificationManager_CustomEvent, text);
			}
			if (triggerId == SourceGuid.Timer)
			{
				return string.Format(CultureInfo.InvariantCulture, Resources.NotificationManager_TimesEvent);
			}
			string text2 = FindDeviceName(triggerId);
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			string text3 = FindServerName(triggerId);
			if (!string.IsNullOrEmpty(text3))
			{
				return text3;
			}
			string text4 = FindHardwareName(triggerId);
			if (!string.IsNullOrEmpty(text4))
			{
				return text4;
			}
			string text5 = FindAcUnitName(triggerId);
			if (!string.IsNullOrEmpty(text5))
			{
				return text5;
			}
			string text6 = FindEventTypeName(eventTypeId);
			if (!string.IsNullOrEmpty(text6))
			{
				return string.Format(CultureInfo.InvariantCulture, Resources.NotificationManager_SystemEvent, text6);
			}
			return string.Empty;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			return "-";
		}
	}

	private static string FindServerName(Guid triggerId)
	{
		string text = null;
		try
		{
			text = ConfigurationDataManager.FindRecorderNameByRecorderId(triggerId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			text = string.Empty;
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		try
		{
			text = ConfigurationDataManager.FindFailoverServerNameById(triggerId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			text = string.Empty;
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		try
		{
			text = ConfigurationDataManager.FindManagementServerNameById(triggerId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			text = string.Empty;
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		try
		{
			text = ConfigurationDataManager.FindRegisteredServerNameById(triggerId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			text = string.Empty;
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return string.Empty;
	}

	private string FindEventTypeName(Guid eventTypeId)
	{
		return _server.ConfigurationLookup.FindEventDisplayNameFromEventTypeID(eventTypeId, string.Empty);
	}

	private SourceType FindEventSourceTypeName(Guid eventTypeId)
	{
		return _server.ConfigurationLookup.FindEventSourceTypeFromEventTypeID(eventTypeId);
	}

	private static Compressor FindCompressor(ConfigurationManager configurationManager, int handler)
	{
		Compressor compressor = configurationManager.Compressors[handler];
		if (compressor == null)
		{
			return null;
		}
		return compressor;
	}

	private static NotificationSetting FindNotificationSetting(ConfigurationManager configurationManager, Guid profileId)
	{
		Guid guid = new Guid("74E649D7-D6F6-47DB-8828-9A6CCFE95204");
		foreach (NotificationSetting value in configurationManager.NotificationSettings.Values)
		{
			if (value.IDNotificationProfile == profileId && value.IDNotificationType == guid)
			{
				return value;
			}
		}
		return null;
	}

	private static string FindRuleName(Guid ruleId)
	{
		try
		{
			return ConfigurationDataManager.FindRuleNameByRuleId(ruleId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	private static string FindRecorderNameFromDevice(Guid deviceId)
	{
		try
		{
			return ConfigurationDataManager.FindRecorderNameByDeviceId(deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	private string FindRecorderNameFromRecoderId(Guid recorderId)
	{
		try
		{
			return ConfigurationDataManager.FindRecorderNameByRecorderId(recorderId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	private static string FindHardwareNameFromDevice(Guid deviceId)
	{
		try
		{
			return ConfigurationDataManager.FindHardwareNameByDeviceId(deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	private static string FindHardwareName(Guid hardwareId)
	{
		try
		{
			return ConfigurationDataManager.FindHardwareNameByHardwareId(hardwareId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	private static string FindDeviceName(Guid deviceId)
	{
		try
		{
			return ConfigurationDataManager.FindDeviceNameByDeviceId(deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	private static string FindAcUnitName(Guid mipObjectId)
	{
		try
		{
			return ConfigurationDataManager.FindAcUnitNameByMipObjectId(mipObjectId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	public static string FindSystemSetting(ConfigurationManager configurationManager, SettingList settingName)
	{
		return configurationManager.SystemSettings[settingName]?.Value;
	}

	public static bool FindSystemSettingBool(ConfigurationManager configurationManager, SettingList settingName)
	{
		SystemSetting systemSetting = configurationManager.SystemSettings[settingName];
		if (systemSetting == null)
		{
			throw new ArgumentNullException(settingName.ToString(), "Chosen parameter is null.");
		}
		return systemSetting.ValueAsBool;
	}

	public static int FindSystemSettingInt(ConfigurationManager configurationManager, SettingList settingName)
	{
		SystemSetting systemSetting = configurationManager.SystemSettings[settingName];
		if (systemSetting == null)
		{
			throw new ArgumentNullException(settingName.ToString(), "Chosen parameter is null.");
		}
		return systemSetting.ValueAsInt32;
	}
}
