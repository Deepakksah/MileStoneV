using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace VideoOS.Utilities.OemResource;

[CLSCompliant(true)]
public abstract class OemResourceReaderBase
{
	private readonly OemResourcePackage _oemResourcePackage;

	public OemResourceReaderBase(IResourcesFolderLocator resourcesFolderLocator)
	{
		if (resourcesFolderLocator == null)
		{
			throw new ArgumentNullException("resourcesFolderLocator");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(resourcesFolderLocator.ResourceFilePath);
		_oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!_oemResourcePackage.ValidateSignature())
		{
			throw new InvalidOemResourceException();
		}
	}

	public string GetResourceString([CallerMemberName] string name = "")
	{
		return (_oemResourcePackage.GetResourceItem(name)?.ResourceValueAsObject as string) ?? throw new MissingOemResourceException();
	}
}
