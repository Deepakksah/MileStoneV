using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Localization;

namespace VideoOS.IdentityServer.Localization;

public static class LocalizationContants
{
	public static readonly RequestCulture DefaultCulture = new RequestCulture("en-US");

	public static readonly IList<CultureInfo> SupportedCultures = new string[34]
	{
		"en-US", "ar-SA", "hr-HR", "cs-CZ", "nl-NL", "fi-FI", "he-IL", "hi-IN", "hu-HU", "is-IS",
		"nb-no", "pl-PL", "sr-Latn-RS", "sk-SK", "sv-SE", "th-TH", "tr-TR", "bg-BG", "zh-CN", "zh-TW",
		"da-DK", "fr-FR", "de-DE", "it-IT", "ja-JP", "ko-KR", "pt-BR", "ru-RU", "es-ES", "fa-IR",
		"uk-UA", "vi-VN", "el-GR", "ro-RO"
	}.Select((string el) => new CultureInfo(el)).ToList();
}
