using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Security;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server;

public sealed class MailProcessor
{
	private SecurityToken _securityToken;

	private MailGenerator _mailGenerator;

	public int TimeBetweenMails;

	public bool IncludeImages;

	public int NumberOfImages;

	public bool EmbedImages;

	public int TimeBetweenImages;

	public bool IncludeAvi;

	public int TimeBeforeEvent;

	public int TimeAfterEvent;

	public double Framerate;

	public string TriggeringName;

	public string Recipients;

	public string Subject;

	public string CustomMessage;

	public string MediaDeviceName;

	public string SMTPServer;

	public string SMTPLogon;

	public string SMTPUsername;

	public string SMTPPassword;

	public string SMTPSender;

	public bool SMTPUseSsl;

	public int SMTPPort;

	public string AVICodec;

	public bool ValidAVIInfo;

	public int CompressorHandler;

	public int CompressorFlags;

	public int CompressorQuality;

	public int CompressorDataRate;

	public int CompressorKeyFrames;

	public byte[] CompressorConfigData;

	private SendNotificationCommand _cmd;

	public static readonly ILogger Logger = Telemetry.GetILogger("NotificationManager");

	public MailProcessor(SecurityToken securityToken)
	{
		_securityToken = securityToken;
	}

	public void BuildAndSendMail(SendNotificationCommand cmd)
	{
		_cmd = cmd;
		Exception ex = null;
		AviConfigData configData = null;
		try
		{
			InitializeMailGenerator();
			if (cmd.CameraId != SpecialActionTargets.NoneDevice && cmd.CameraId != Guid.Empty && (IncludeImages || IncludeAvi))
			{
				try
				{
					configData = GetConfigDataMail();
					RunAviGenMail(configData, cmd.CameraId);
					ProcessResultsMail(configData);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					ex = ex3;
				}
			}
			if (ex != null)
			{
				if (ex is WebException)
				{
					MailGenerator mailGenerator = _mailGenerator;
					mailGenerator.CustomMessage = mailGenerator.CustomMessage + "\n\nImage not added due to error: " + LogEntryId.EMailErrorRecorderUnavailable;
				}
				else
				{
					MailGenerator mailGenerator2 = _mailGenerator;
					mailGenerator2.CustomMessage = mailGenerator2.CustomMessage + "\n\nImage not added due to error: " + LogEntryId.EMailError;
				}
			}
			_mailGenerator.SendMail();
		}
		finally
		{
			Cleanup(configData);
		}
		if (ex != null)
		{
			throw ex;
		}
	}

	private void InitializeMailGenerator()
	{
		MailGenerator mailGenerator = new MailGenerator(SMTPServer, SMTPSender, Recipients, SMTPUseSsl, SMTPPort);
		if (SMTPLogon.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
		{
			mailGenerator.SetLoginMode(SMTPUsername, SMTPPassword);
		}
		mailGenerator.Subject = Subject;
		mailGenerator.TriggeringDeviceName = TriggeringName;
		mailGenerator.CustomMessage = CustomMessage;
		mailGenerator.MediaDeviceName = MediaDeviceName;
		mailGenerator.EmbedImages = EmbedImages;
		_mailGenerator = mailGenerator;
	}

	private AviConfigData GetConfigDataMail()
	{
		AviConfigData configDataInitial = GetConfigDataInitial(getCodecs: false);
		string text = FindRecorderUriFromDevice(_cmd.CameraId);
		if (!string.IsNullOrEmpty(text))
		{
			configDataInitial.MediaSources.AddVideoSource(_cmd.CameraId, text);
		}
		Guid[] audioIds = _cmd.AudioIds;
		foreach (Guid deviceId in audioIds)
		{
			text = FindRecorderUriFromDevice(deviceId);
			if (!string.IsNullOrEmpty(text))
			{
				configDataInitial.MediaSources.AddAudioSource(deviceId, text);
			}
		}
		if (IncludeImages)
		{
			configDataInitial.Jpg.StartTimeUtc = _cmd.Time.ToUniversalTime();
			configDataInitial.Jpg.NoOfImages = NumberOfImages;
			configDataInitial.Jpg.TimeBtwMs = TimeBetweenImages;
		}
		if (IncludeAvi)
		{
			configDataInitial.Avi.Compressor.Handler = CompressorHandler;
			configDataInitial.Avi.Compressor.Flags = CompressorFlags;
			configDataInitial.Avi.Compressor.DataRate = CompressorDataRate;
			configDataInitial.Avi.Compressor.KeyFrames = CompressorKeyFrames;
			configDataInitial.Avi.Compressor.Quality = CompressorQuality;
			configDataInitial.Avi.Compressor.ConfigData = CompressorConfigData;
			configDataInitial.Avi.Framerate = Framerate;
			configDataInitial.Avi.StartTimeUtc = _cmd.Time - new TimeSpan(0, 0, 0, TimeBeforeEvent, 0);
			configDataInitial.Avi.EndTimeUtc = _cmd.Time + new TimeSpan(0, 0, 0, TimeAfterEvent, 0);
			string str = new string(Path.GetInvalidFileNameChars());
			Regex regex = new Regex(string.Format(CultureInfo.InvariantCulture, "[{0}]", Regex.Escape(str)));
			string arg = regex.Replace(MediaDeviceName, "");
			configDataInitial.Avi.OutputFile = string.Format(CultureInfo.InvariantCulture, "{1} - {0:dd.MM.yyyy - HH.mm.ss}.avi", _cmd.Time.ToLocalTime(), arg);
		}
		return configDataInitial;
	}

	private static string FindRecorderUriFromDevice(Guid deviceId)
	{
		try
		{
			return ConfigurationDataManager.FindRecorderWebServerUriByDevice(deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return null;
		}
	}

	private void RunAviGenMail(AviConfigData configData, Guid deviceID)
	{
		try
		{
			int num = RunAviGenProc(configData, _securityToken);
			switch (num)
			{
			case 1:
				Logger.LogError("Error creating AVI: MailMediaNoConfiguration: Camera ID: {DeviceId}", deviceID);
				_mailGenerator.SetError(MailMediaError.MailMediaNoConfiguration);
				break;
			case 2:
				Logger.LogError("Error creating AVI: MailMediaErrorInConfiguration: Camera ID: {DeviceId}", deviceID);
				_mailGenerator.SetError(MailMediaError.MailMediaErrorInConfiguration);
				break;
			case 3:
				Logger.LogError("Error creating AVI: MailMediaErrorInConfiguration: Camera ID: {DeviceId}", deviceID);
				_mailGenerator.SetError(MailMediaError.MailMediaErrorInConfiguration);
				break;
			case 10:
				Logger.LogError("Error creating AVI: MailMediaErrorCodecDoesNotSupportThisMedia: Camera ID: {DeviceId}", deviceID);
				_mailGenerator.SetError(MailMediaError.MailMediaErrorCodecDoesNotSupportThisMedia);
				break;
			case 11:
				Logger.LogError("Error creating AVI: MailMediaErrorNoImagesAvailable: Camera ID: {DeviceId}", deviceID);
				_mailGenerator.SetError(MailMediaError.MailMediaErrorNoImagesAvailable);
				break;
			case 12:
				Logger.LogError("Error creating AVI: MailMediaErrorConnectionBroken");
				_mailGenerator.SetError(MailMediaError.MailMediaErrorConnectionBroken);
				break;
			case 99:
				Logger.LogError("avigen.exe crashed in an unexpected way. Probably due to a codec error. Camera ID: {DeviceId}", deviceID);
				_mailGenerator.SetError(MailMediaError.MailMediaErrorUnknownError);
				break;
			default:
				Logger.LogError("Error creating AVI: MailMediaErrorUnknownError no. {ErrorLevel} CameraID {DeviceId}", num, deviceID);
				_mailGenerator.SetError(MailMediaError.MailMediaErrorUnknownError);
				break;
			case 0:
				break;
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("Error creating AVI: CameraID {DeviceId} Exception: {Exception}", deviceID, ex2);
			_mailGenerator.SetError(MailMediaError.MailMediaErrorUnknownError);
			throw;
		}
	}

	private void ProcessResultsMail(AviConfigData configData)
	{
		List<string> resultFileNames = GetResultFileNames(configData);
		foreach (string item in resultFileNames)
		{
			if (item.EndsWith(".Jpg", StringComparison.OrdinalIgnoreCase))
			{
				_mailGenerator.AddImage(Path.Combine(configData.TempDirectory, item));
			}
			else if (item.EndsWith(".Avi", StringComparison.OrdinalIgnoreCase))
			{
				_mailGenerator.AddVideo(Path.Combine(configData.TempDirectory, item));
			}
		}
	}

	public static AviCodec64[] GetInstalledCodecs()
	{
		AviConfigData aviConfigData = null;
		AviCodec64[] result = null;
		try
		{
			aviConfigData = GetConfigDataInitial(getCodecs: true);
			int num = RunAviGenProc(aviConfigData, null);
			switch (num)
			{
			case 1:
				Logger.LogError("Error retrieving codecs: CodecNoConfiguration");
				break;
			case 2:
				Logger.LogError("Error retrieving codecs: CodecErrorInConfiguration");
				break;
			case 3:
				Logger.LogError("Error retrieving codecs: CodecErrorInConfiguration");
				break;
			case 99:
				Logger.LogError("Error retrieving codecs: avigen.exe crashed in an unexpected way.");
				break;
			default:
				Logger.LogError("Error retrieving codecs: CodecErrorUnknownError no. {ErrorLevel}", num);
				break;
			case 0:
				break;
			}
			if (num == 0)
			{
				List<string> resultFileNames = GetResultFileNames(aviConfigData);
				Stream stream = null;
				try
				{
					stream = File.Open(aviConfigData.TempDirectory + "\\Codecs.bin", FileMode.Open);
					BinaryFormatter binaryFormatter = new BinaryFormatter().Wrap(typeof(AviCodec64));
					result = (AviCodec64[])binaryFormatter.Deserialize(stream);
				}
				finally
				{
					stream?.Close();
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("Error retrieving codecs: {Exception}", ex2);
		}
		finally
		{
			Cleanup(aviConfigData);
		}
		return result;
	}

	private static AviConfigData GetConfigDataInitial(bool getCodecs)
	{
		AviConfigData aviConfigData = new AviConfigData();
		aviConfigData.TempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
		aviConfigData.GetCodec = getCodecs;
		return aviConfigData;
	}

	private static int RunAviGenProc(AviConfigData configData, SecurityToken securityToken)
	{
		string text = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
		try
		{
			string contents = configData.ToXml();
			File.WriteAllText(text, contents);
			string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "AviGen");
			string text2 = Path.Combine(path, "avigen.exe");
			bool flag = false;
			if (!File.Exists(text2))
			{
				path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).ToUpperInvariant();
				text2 = Path.Combine(path, "AVIGEN.EXE");
				text2 = text2.Replace("VIDEOOS.SERVER.SERVICE", "VIDEOOS.SERVER.AVIGEN");
				flag = true;
			}
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = text2;
			processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "\"{0}\" {1}", text, (securityToken != null) ? securityToken.Value : "NoTokenNeeded");
			processStartInfo.WindowStyle = ((!flag) ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal);
			using Process process = Process.Start(processStartInfo);
			process.WaitForExit();
			return process.ExitCode;
		}
		finally
		{
			try
			{
				File.Delete(text);
			}
			catch
			{
			}
		}
	}

	private static List<string> GetResultFileNames(AviConfigData configData)
	{
		string[] files = Directory.GetFiles(configData.TempDirectory);
		List<string> list = new List<string>(files);
		list.Sort();
		return list;
	}

	private static void Cleanup(AviConfigData configData)
	{
		if (configData == null)
		{
			return;
		}
		try
		{
			if (Directory.Exists(configData.TempDirectory))
			{
				Directory.Delete(configData.TempDirectory, recursive: true);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}
}
