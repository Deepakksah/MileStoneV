using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace VideoOS.Platform.Live;

public class LiveSourceStatus
{
	private readonly Dictionary<int, string> _currectStatus = new Dictionary<int, string>();

	public Collection<int> Keys => new Collection<int>(_currectStatus.Keys.ToArray());

	public LiveSourceStatus(string liveStatus)
	{
		if (string.IsNullOrEmpty(liveStatus))
		{
			return;
		}
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(liveStatus);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("livepackage/status/statusitem");
			if (xmlNodeList == null)
			{
				return;
			}
			foreach (XmlNode item in xmlNodeList)
			{
				string innerText = item.Attributes["id"].InnerText;
				string innerText2 = item.Attributes["value"].InnerText;
				int result = 0;
				int.TryParse(innerText, out result);
				_currectStatus[result] = innerText2;
			}
		}
		catch (Exception)
		{
		}
	}

	public string Value(int key)
	{
		if (_currectStatus.Keys.Contains(key))
		{
			return _currectStatus[key];
		}
		return "";
	}
}
