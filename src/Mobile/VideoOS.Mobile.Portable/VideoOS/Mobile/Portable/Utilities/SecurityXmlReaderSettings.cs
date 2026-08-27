using System.Xml;

namespace VideoOS.Mobile.Portable.Utilities;

public static class SecurityXmlReaderSettings
{
	public static XmlReaderSettings Settings => new XmlReaderSettings
	{
		CheckCharacters = true,
		DtdProcessing = DtdProcessing.Prohibit,
		MaxCharactersFromEntities = 1024L
	};
}
