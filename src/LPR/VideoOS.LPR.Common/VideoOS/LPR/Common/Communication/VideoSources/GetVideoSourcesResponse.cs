using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.LPR.Common.Communication.Info;

namespace VideoOS.LPR.Common.Communication.VideoSources;

public class GetVideoSourcesResponse
{
	private static XmlSerializer _xmlSerializer = new XmlSerializer(typeof(GetVideoSourcesResponse), new Type[1] { typeof(VideoSourceInfo) });

	public List<VideoSourceInfo> VideoSourceInfos { get; set; }

	public bool RetrievalInProgress { get; set; }

	public string Serialize()
	{
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		_xmlSerializer.Serialize(stringWriter, this);
		return stringWriter.ToString();
	}

	public static GetVideoSourcesResponse Deserialize(string value)
	{
		GetVideoSourcesResponse result = null;
		try
		{
			using StringReader input = new StringReader(value);
			using XmlTextReader xmlReader = new XmlTextReader(input)
			{
				DtdProcessing = DtdProcessing.Prohibit,
				XmlResolver = null
			};
			result = (GetVideoSourcesResponse)_xmlSerializer.Deserialize(xmlReader);
		}
		catch (Exception)
		{
		}
		return result;
	}
}
