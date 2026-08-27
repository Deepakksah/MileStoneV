using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.LPR.Common.Communication.Sites;

public class GetAllSitesParams
{
	private static XmlSerializer _xmlSerializer = new XmlSerializer(typeof(GetAllSitesParams));

	public Guid SiteId { get; set; }

	public string Serialize()
	{
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		_xmlSerializer.Serialize(stringWriter, this);
		return stringWriter.ToString();
	}

	public static GetAllSitesParams Deserialize(string value)
	{
		GetAllSitesParams result = null;
		try
		{
			using StringReader input = new StringReader(value);
			using XmlTextReader xmlReader = new XmlTextReader(input)
			{
				DtdProcessing = DtdProcessing.Prohibit,
				XmlResolver = null
			};
			result = (GetAllSitesParams)_xmlSerializer.Deserialize(xmlReader);
		}
		catch (Exception)
		{
		}
		return result;
	}
}
