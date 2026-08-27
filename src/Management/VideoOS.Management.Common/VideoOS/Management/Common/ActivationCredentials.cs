using System;
using System.Xml.Linq;

namespace VideoOS.Management.Common;

public class ActivationCredentials
{
	public string UserName { get; set; }

	public string Password { get; set; }

	public string Xml
	{
		get
		{
			XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("ActivationCredentials", new XElement("UserName", UserName), new XElement("Password", Password)));
			return xDocument.Declaration.ToString() + xDocument.ToString();
		}
	}

	public ActivationCredentials()
	{
	}

	public static ActivationCredentials Parse(string xml)
	{
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		if (string.IsNullOrWhiteSpace(xml))
		{
			throw new ArgumentException("The value of ActivationCredentials cannot be empty!");
		}
		XElement root = XDocument.Parse(xml).Root;
		if (root.Element("UserName") == null || root.Element("Password") == null)
		{
			throw new FormatException("XML value does not have the correct format!");
		}
		ActivationCredentials obj = new ActivationCredentials
		{
			UserName = root.Element("UserName").Value,
			Password = root.Element("Password").Value
		};
		if (string.IsNullOrEmpty(obj.UserName))
		{
			throw new ArgumentException("Username cannot be null.");
		}
		return obj;
	}

	public ActivationCredentials(string userName, string password)
	{
		UserName = userName;
		Password = password;
	}
}
