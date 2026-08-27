using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.LPR.Common.Communication.Info;

namespace VideoOS.LPR.Common.Communication.Sites;

public class GetSitesResponse
{
	private static XmlSerializer _xmlSerializer = new XmlSerializer(typeof(GetSitesResponse), new Type[1] { typeof(SiteInfo) });

	public List<SiteInfo> SiteInfos { get; set; }

	public string Serialize()
	{
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		_xmlSerializer.Serialize(stringWriter, this);
		return stringWriter.ToString();
	}

	public static GetSitesResponse Deserialize(string value)
	{
		GetSitesResponse result = null;
		try
		{
			using StringReader input = new StringReader(value);
			using XmlTextReader xmlReader = new XmlTextReader(input)
			{
				DtdProcessing = DtdProcessing.Prohibit,
				XmlResolver = null
			};
			result = (GetSitesResponse)_xmlSerializer.Deserialize(xmlReader);
		}
		catch (Exception)
		{
		}
		return result;
	}
}
