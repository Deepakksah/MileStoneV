using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.LPR.Common.Communication.VideoSources;

public class GetVideoSourcesParams
{
	private static XmlSerializer _xmlSerializer = new XmlSerializer(typeof(GetVideoSourcesParams));

	public Guid SiteId { get; set; }

	public string Serialize()
	{
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		_xmlSerializer.Serialize(stringWriter, this);
		return stringWriter.ToString();
	}

	public static GetVideoSourcesParams Deserialize(string value)
	{
		GetVideoSourcesParams result = null;
		try
		{
			using StringReader input = new StringReader(value);
			using XmlTextReader xmlReader = new XmlTextReader(input)
			{
				DtdProcessing = DtdProcessing.Prohibit,
				XmlResolver = null
			};
			result = (GetVideoSourcesParams)_xmlSerializer.Deserialize(xmlReader);
		}
		catch (Exception)
		{
		}
		return result;
	}
}
