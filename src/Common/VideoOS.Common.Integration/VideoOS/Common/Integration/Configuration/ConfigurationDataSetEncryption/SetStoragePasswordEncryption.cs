using System;
using System.Linq;
using System.Xml.Linq;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetEncryption;

public class SetStoragePasswordEncryption : IXmlTransformation
{
	public void Transform(XDocument doc, Func<string, byte[], string> transform)
	{
		foreach (XElement item in from d in doc.Descendants()
			where d.Name.LocalName == "encryptionPassword"
			select d)
		{
			item.Value = transform(item.Value, null);
		}
	}
}
