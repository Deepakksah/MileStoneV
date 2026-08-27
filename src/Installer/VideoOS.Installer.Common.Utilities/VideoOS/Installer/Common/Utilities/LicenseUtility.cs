using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public class LicenseUtility : ILicenseUtility
{
	private IGlobalData _globalData;

	public LicenseUtility(IGlobalData globalData)
	{
		if (globalData == null)
		{
			throw new ArgumentNullException("globalData");
		}
		_globalData = globalData;
	}

	public bool HasEvidenceLockFeature(bool valueIfError)
	{
		if (!_globalData.TryGetValue("LicenseInformation", out var value))
		{
			return valueIfError;
		}
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(value);
		}
		catch (Exception exception)
		{
			LogManager.Write("Loading license info xml failed.", TraceEventType.Error);
			LogManager.Write(exception);
			return valueIfError;
		}
		string xpath = FormattableString.Invariant(FormattableStringFactory.Create("LicenseInfo/Features/Feature[@name='{0}']", "EvidenceLock"));
		return xmlDocument.SelectSingleNode(xpath) != null;
	}
}
