using System;
using System.Globalization;
using System.Web.Services.Protocols;
using System.Xml;

namespace VideoOS.Common.Integration.ServerAPI;

public static class SoapDeserialize
{
	public static Tuple<int, int> DeserializeErrorDetails(SoapException ex)
	{
		if (ex == null)
		{
			throw new ArgumentNullException("ex");
		}
		int result = 0;
		int result2 = 0;
		XmlDocument xmlDocument = new XmlDocument();
		if (ex.Detail != null)
		{
			XmlReader reader = ex.Detail.CreateNavigator().ReadSubtree();
			XmlNode firstChild = xmlDocument.ReadNode(reader).FirstChild;
			if (!int.TryParse(firstChild.InnerText, out result))
			{
				throw new FormatException(string.Format(CultureInfo.InvariantCulture, "{0} not in the correct format", firstChild.Name));
			}
			XmlNode nextSibling = firstChild.NextSibling;
			if (nextSibling != null && !int.TryParse(nextSibling.InnerText, out result2))
			{
				throw new FormatException(string.Format(CultureInfo.InvariantCulture, "{0} not in the correct format", nextSibling.Name));
			}
		}
		return new Tuple<int, int>(result, result2);
	}
}
