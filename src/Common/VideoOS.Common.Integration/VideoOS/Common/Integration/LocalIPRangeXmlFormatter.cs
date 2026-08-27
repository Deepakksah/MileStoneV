using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Utilities.Network;

namespace VideoOS.Common.Integration;

public class LocalIPRangeXmlFormatter
{
	public class IPRange
	{
		private IPAddress _rangeFrom;

		private IPAddress _rangeTo;

		public IPAddress RangeFrom => _rangeFrom;

		public IPAddress RangeTo => _rangeTo;

		public IPRange(IPAddress rangeFrom, IPAddress rangeTo)
		{
			_rangeFrom = rangeFrom;
			_rangeTo = rangeTo;
		}
	}

	private int _counter;

	private SortedDictionary<int, IPRange> _rangeList = new SortedDictionary<int, IPRange>();

	public SortedDictionary<int, IPRange> IPRanges => _rangeList;

	public void AddRange(IPAddress rangeFrom, IPAddress rangeTo)
	{
		_rangeList.Add(_counter, new IPRange(rangeFrom, rangeTo));
		_counter++;
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
		{
			xmlWriter.WriteStartDocument(standalone: true);
			xmlWriter.WriteStartElement("LocalIPRanges");
			for (int i = 0; i < _rangeList.Count; i++)
			{
				xmlWriter.WriteStartElement("Range");
				xmlWriter.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));
				IPRange iPRange = _rangeList[i];
				xmlWriter.WriteAttributeString("From", iPRange.RangeFrom.ToString());
				xmlWriter.WriteAttributeString("To", iPRange.RangeTo.ToString());
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
		}
		return stringBuilder.ToString();
	}

	public static LocalIPRangeXmlFormatter FromXml(string rangeXml)
	{
		LocalIPRangeXmlFormatter localIPRangeXmlFormatter = new LocalIPRangeXmlFormatter();
		if (string.IsNullOrEmpty(rangeXml))
		{
			throw new ArgumentNullException("rangeXml", "Invalid range xml");
		}
		using StringReader input = new StringReader(rangeXml);
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.IgnoreComments = true;
		using XmlReader reader = XmlReader.Create(input, xmlReaderSettings);
		foreach (XPathNavigator item in new XPathDocument(reader).CreateNavigator().SelectSingleNode("LocalIPRanges").SelectChildren(XPathNodeType.Element))
		{
			item.MoveToFirstAttribute();
			_ = item.ValueAsInt;
			item.MoveToNextAttribute();
			string value = item.Value;
			item.MoveToNextAttribute();
			string value2 = item.Value;
			localIPRangeXmlFormatter.AddRange(IPAddress.Parse(value), IPAddress.Parse(value2));
		}
		return localIPRangeXmlFormatter;
	}

	public static bool IsInRange(string address, string localIPRanges)
	{
		if (!string.IsNullOrEmpty(localIPRanges) && IPAddress.TryParse(address, out var address2))
		{
			foreach (IPRange value in FromXml(localIPRanges).IPRanges.Values)
			{
				if (value.RangeFrom.AddressFamily == address2.AddressFamily && IPHelper.IPAddressCompare(value.RangeFrom, address2) != 1 && IPHelper.IPAddressCompare(address2, value.RangeTo) != 1)
				{
					return true;
				}
			}
		}
		return false;
	}
}
