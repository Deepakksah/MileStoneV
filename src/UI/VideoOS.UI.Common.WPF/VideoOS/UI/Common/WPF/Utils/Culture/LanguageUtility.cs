using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Culture;

public static class LanguageUtility
{
	public static XmlLanguage SelectedLanguageName { get; set; } = XmlLanguage.GetLanguage("en-US");
}
