using System;
using System.Globalization;
using System.IO;
using CSCore;
using CSCore.Codecs.WAV;
using VideoOS.Management.Server.Log;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public sealed class AudioMessage : IAudioMessageItem
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime LastModified { get; set; }

	public string OriginalFileName { get; set; }

	public string ContentBase64 { get; set; }

	public AudioMessage()
	{
	}

	public AudioMessage(AudioMessage other)
	{
		Id = other.Id;
		Name = other.Name;
		Description = other.Description;
		LastModified = other.LastModified;
		OriginalFileName = other.OriginalFileName;
		ContentBase64 = other.ContentBase64;
	}

	public static string[] QueryAudioMessageIds(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent();
		return audioMessageComponent.QueryAudioMessageIds();
	}

	public static AudioMessage[] QueryAudioMessages(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent();
		return audioMessageComponent.QueryAudioMessages();
	}

	public static AudioMessage QueryAudioMessageById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent();
		return audioMessageComponent.QueryAudioMessageById(id);
	}

	public static AudioMessage[] QueryAudioMessagesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent();
		return audioMessageComponent.QueryAudioMessagesByIds(ids);
	}

	public static AudioMessage CreateAudioMessage(IGeminiRequestContext requestContext, string id, string name, string description, string originalFileName, string contentBase64)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		id = GeminiResourceComponent.GetGuid(id);
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "description");
		SetMessageContentValidation(requestContext, id, originalFileName, contentBase64, out var content);
		int maxMessageCount = QueryMaxMessageCount(requestContext);
		AudioMessage audioMessage;
		using (IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent())
		{
			audioMessageComponent.CreateAudioMessage(id, name, description, originalFileName, content, maxMessageCount);
			audioMessage = audioMessageComponent.QueryAudioMessageById(id);
		}
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditAudioMessageCreated(audioMessage.Id, audioMessage.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "AudioMessage", audioMessage.Id, EventAction.Added, audioMessage.Name);
		return audioMessage;
	}

	public static AudioMessage UpdateAudioMessage(IGeminiRequestContext requestContext, string id, string name, string description)
	{
		UpdateAudioMessageValidation(requestContext, id, name, description);
		AudioMessage result;
		using (IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent())
		{
			audioMessageComponent.UpdateAudioMessage(id, name, description);
			result = audioMessageComponent.QueryAudioMessageById(id);
		}
		Integration.TryNotifyServerAsync();
		Integration.FireConfigurationChanged(requestContext, "AudioMessage", id, EventAction.Updated, name);
		return result;
	}

	public static void UpdateAudioMessageValidation(IGeminiRequestContext requestContext, string id, string name, string description)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "description", allowNull: false);
	}

	public static void DeleteAudioMessage(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		using (IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent())
		{
			audioMessageComponent.DeleteAudioMessage(id, sessionId);
		}
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "AudioMessage", id, EventAction.Deleted, string.Empty);
	}

	public static void SetMessageContent(IGeminiRequestContext requestContext, string id, string originalFileName, string contentBase64)
	{
		SetMessageContentValidation(requestContext, id, originalFileName, contentBase64, out var content);
		Guid sessionId = Guid.NewGuid();
		using (IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent())
		{
			audioMessageComponent.UpdateAudioMessageContent(id, originalFileName, content, sessionId);
		}
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "AudioMessage", id, EventAction.Updated, string.Empty);
	}

	public static void SetMessageContentValidation(IGeminiRequestContext requestContext, string id, string originalFileName, string contentBase64, out byte[] content)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Validation.CheckName(originalFileName, "originalFileName");
		int num = QueryMaxMessageSize(requestContext);
		Validation.CheckObjectReference(contentBase64, "contentBase64");
		int num2 = (num + 2 - (num + 2) % 3) / 3 * 4;
		if (contentBase64.Length > num2)
		{
			throw VmoExceptionFactory.VMO60317_VmoServerServiceException();
		}
		try
		{
			content = Convert.FromBase64String(contentBase64);
		}
		catch (FormatException)
		{
			throw VmoExceptionFactory.VMO60318_VmoServerServiceException();
		}
		if (content.Length > num)
		{
			throw VmoExceptionFactory.VMO60317_VmoServerServiceException();
		}
		if (content.Length < 32)
		{
			throw VmoExceptionFactory.VMO60318_VmoServerServiceException();
		}
		using MemoryStream stream = new MemoryStream(content);
		WaveFileReader waveFileReader = null;
		try
		{
			try
			{
				waveFileReader = new WaveFileReader(stream);
			}
			catch (Exception)
			{
				throw VmoExceptionFactory.VMO60318_VmoServerServiceException();
			}
			if (waveFileReader.WaveFormat.WaveFormatTag != AudioEncoding.Pcm || waveFileReader.WaveFormat.SampleRate != 16000 || waveFileReader.WaveFormat.BitsPerSample != 16 || waveFileReader.WaveFormat.Channels != 1)
			{
				throw VmoExceptionFactory.VMO60319_VmoServerServiceException();
			}
		}
		finally
		{
			waveFileReader?.Dispose();
			waveFileReader = null;
		}
	}

	public static string GetMessageContent(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IAudioMessageComponent<AudioMessage> audioMessageComponent = requestContext.CreateAudioMessageComponent();
		return Convert.ToBase64String(audioMessageComponent.QueryAudioMessageContentById(id));
	}

	private static int QueryMaxMessageSize(IGeminiRequestContext requestContext)
	{
		using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
		return Convert.ToInt32(managementServerComponent.GetSystemSetting(SettingList.AudioMessageMaxBytes), CultureInfo.InvariantCulture);
	}

	private static int QueryMaxMessageCount(IGeminiRequestContext requestContext)
	{
		using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
		return Convert.ToInt32(managementServerComponent.GetSystemSetting(SettingList.AudioMessageMaxCount), CultureInfo.InvariantCulture);
	}
}
