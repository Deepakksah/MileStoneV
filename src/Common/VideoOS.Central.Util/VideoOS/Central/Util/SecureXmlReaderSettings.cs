using System.Xml;

namespace VideoOS.Central.Util;

public static class SecureXmlReaderSettings
{
	public static XmlReaderSettings Settings => new XmlReaderSettings
	{
		CheckCharacters = true,
		DtdProcessing = DtdProcessing.Prohibit,
		MaxCharactersFromEntities = 1024L
	};
}
