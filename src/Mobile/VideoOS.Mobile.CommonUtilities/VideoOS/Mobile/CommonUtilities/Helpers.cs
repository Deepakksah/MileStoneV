using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security;
using System.Xml;
using VideoOS.Utilities.OemResource;
using VideoOS.Utilities.Security;

namespace VideoOS.Mobile.CommonUtilities;

public static class Helpers
{
	public static Dictionary<string, OemResourceItemBase> GetResourcePackage(string embeddedResourceName, Assembly assemblyToReadFrom)
	{
		StreamReader txtReader = new StreamReader(assemblyToReadFrom.GetManifestResourceStream(embeddedResourceName));
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(txtReader);
		OemResourcePackage oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!oemResourcePackage.ValidateSignature())
		{
			throw new SecurityException("The OEM resource file is signed with an invalid signature.");
		}
		return oemResourcePackage.OemResourceItems as Dictionary<string, OemResourceItemBase>;
	}

	public static bool ValidatePassword(string username, string password)
	{
		return Password.ValidatePassword(username, new NetworkCredential("", password).SecurePassword);
	}
}
