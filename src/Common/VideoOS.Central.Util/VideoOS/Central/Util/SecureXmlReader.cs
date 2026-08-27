using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Central.Util;

public static class SecureXmlReader
{
	public static XmlReader Create(string serializedXmlData)
	{
		return XmlReader.Create(new MemoryStream(Encoding.Unicode.GetBytes(serializedXmlData)), SecureXmlReaderSettings.Settings);
	}
}
