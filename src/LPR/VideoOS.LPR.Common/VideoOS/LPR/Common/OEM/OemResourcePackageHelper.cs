using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.LPR.Common.OEM;

public class OemResourcePackageHelper
{
	private const string ResourceFilename = "resources.xml";

	private readonly string _resourceFilePath;

	private readonly Lazy<OemResourcePackage> _oemResourcePackage;

	public bool IsLoadedOemResourcePackageValid => _oemResourcePackage.Value.ValidateSignature();

	public OemResourcePackageHelper(bool useGlobalResourceFile)
		: this(new ResourcesFolderLocator("resources.xml", useGlobalResourceFile))
	{
	}

	public OemResourcePackageHelper(IResourcesFolderLocator resourcesFolderLocator)
	{
		if (resourcesFolderLocator == null)
		{
			throw new ArgumentNullException("resourcesFolderLocator");
		}
		_resourceFilePath = resourcesFolderLocator.ResourceFilePath;
		_oemResourcePackage = new Lazy<OemResourcePackage>(delegate
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.XmlResolver = null;
			XmlReader reader = XmlReader.Create(new StringReader(_resourceFilePath).ReadToEnd(), new XmlReaderSettings
			{
				XmlResolver = null
			});
			xmlDocument.Load(reader);
			return new OemResourcePackage(xmlDocument);
		});
	}

	public string GetResourceString([CallerMemberName] string name = "")
	{
		OemResourceItemBase resourceItem = _oemResourcePackage.Value.GetResourceItem(name);
		if (resourceItem == null)
		{
			return "[" + name + "]";
		}
		return resourceItem.ResourceValueAsObject as string;
	}
}
