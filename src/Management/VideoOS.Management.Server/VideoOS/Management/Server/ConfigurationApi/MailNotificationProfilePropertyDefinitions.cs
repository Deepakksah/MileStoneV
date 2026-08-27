using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MailNotificationProfilePropertyDefinitions
{
	public const string SubjectKey = "Subject";

	public const string MessageTextKey = "MessageText";

	public const string RecipientsKey = "Recipients";

	public const string TimeBetweenMailsKey = "TimeBetweenMails";

	public const string IncludeImagesKey = "IncludeImages";

	public const string NumberOfImagesKey = "NumberOfImages";

	public const string EmbedImagesKey = "EmbedImages";

	public const string TimeBetweenImagesKey = "TimeBetweenImages";

	public const string IncludeAVIKey = "IncludeAVI";

	public const string TimeBeforeEventKey = "TimeBeforeEvent";

	public const string TimeAfterEventKey = "TimeAfterEvent";

	public const string FrameRateKey = "FrameRate";

	public static Property Subject(string subject)
	{
		return new Property("Subject", subject)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationSubject,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationSubject"
		};
	}

	public static Property MessageText(string text)
	{
		return new Property("MessageText", text)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationText,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationText",
			ToolTipTranslationId = "MailNotificationTextTooltip"
		};
	}

	public static Property Recipients(string mailList)
	{
		Property property = new Property("Recipients", "");
		property.ValueType = "Array";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.MailNotificationRecipients;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "MailNotificationRecipients";
		property.ValueArray = mailList.Split(';');
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}

	public static Property IncludeImages(string value)
	{
		return new Property("IncludeImages", value)
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationIncludeImages,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationIncludeImages"
		};
	}

	public static Property EmbedImages(string value)
	{
		return new Property("EmbedImages", value)
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationEmbedImages,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationEmbedImages"
		};
	}

	public static Property IncludeAVI(string value)
	{
		return new Property("IncludeAVI", value)
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationIncludeAVI,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationIncludeAVI"
		};
	}

	public static Property TimeBetweenMails(string value)
	{
		return new Property("TimeBetweenMails", value)
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationTimeBetweenMails,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationTimeBetweenMails",
			ToolTipTranslationId = "MailNotificationTimeBetweenMailsTooltip"
		};
	}

	public static Property NumberOfImages(string value)
	{
		return new Property("NumberOfImages", value)
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationNumberOfImages,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationNumberOfImages"
		};
	}

	public static Property TimeBetweenImages(string value)
	{
		return new Property("TimeBetweenImages", value)
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationTimeBetweenImages,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationTimeBetweenImages"
		};
	}

	public static Property TimeBeforeEvent(string value)
	{
		return new Property("TimeBeforeEvent", value)
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationTimeBeforeEvent,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationTimeBeforeEvent"
		};
	}

	public static Property TimeAfterEvent(string value)
	{
		return new Property("TimeAfterEvent", value)
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationTimeAfterEvent,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationTimeAfterEvent"
		};
	}

	public static Property FrameRate(string value)
	{
		return new Property("FrameRate", value)
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.MailNotificationFrameRate,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "MailNotificationFrameRate"
		};
	}
}
