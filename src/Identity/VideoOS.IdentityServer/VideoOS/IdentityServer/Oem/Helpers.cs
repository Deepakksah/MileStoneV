using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.IdentityServer.Oem;

public static class Helpers
{
	public static Dictionary<string, OemResourceItemBase> GetResourcePackage(string embeddedResourceName, Assembly assemblyToReadFrom)
	{
		Stream manifestResourceStream = assemblyToReadFrom.GetManifestResourceStream(embeddedResourceName);
		StreamReader txtReader = new StreamReader(manifestResourceStream);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(txtReader);
		OemResourcePackage oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!oemResourcePackage.ValidateSignature())
		{
			throw new SecurityException("The OEM resource file is signed with an invalid signature.");
		}
		return oemResourcePackage.OemResourceItems as Dictionary<string, OemResourceItemBase>;
	}
}
