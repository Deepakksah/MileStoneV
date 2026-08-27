using System.Collections.Generic;
using VideoOS.LogServer.Communication.Legacy.Registration.Data;
using VideoOS.LogServer.Communication.Legacy.Registration.Data.Serialization;
using VideoOS.LogServer.Communication.LogWriterServiceReference;

namespace VideoOS.LogServer.Communication.Extensions;

public static class LegacyMessageXmlExtensions
{
	public static IEnumerable<LegacyLogMessage> ToLogMessages(this LegacyMessageXml value)
	{
		foreach (Entry entry in value.Entries)
		{
			yield return new LegacyLogMessage
			{
				Id = entry.UName,
				LogType = entry.LogType,
				SourceType = entry.SourceType,
				Category = entry.Category,
				MessageTexts = new LogMessageText[1]
				{
					new LogMessageText
					{
						LanguageCode = value.Culture,
						MessageText = entry.Text
					}
				}
			};
		}
	}

	public static IEnumerable<LogMessageCategory> ToCategories(this LegacyMessageXml value)
	{
		foreach (LegacyLogMessageEnumEntry category in value.Categories)
		{
			yield return new LogMessageCategory
			{
				Id = category.Id,
				LogMessageCategoryTexts = new LogMessageCategoryText[1]
				{
					new LogMessageCategoryText
					{
						LanguageCode = value.Culture,
						Text = category.Value
					}
				}
			};
		}
	}

	public static IEnumerable<LogMessageSourceType> ToSourceTypes(this LegacyMessageXml value)
	{
		foreach (LegacyLogMessageEnumEntry sourceType in value.SourceTypes)
		{
			yield return new LogMessageSourceType
			{
				Id = sourceType.Id,
				LogMessageSourceTypeTexts = new LogMessageSourceTypeText[1]
				{
					new LogMessageSourceTypeText
					{
						LanguageCode = value.Culture,
						Text = sourceType.Value
					}
				}
			};
		}
	}
}
