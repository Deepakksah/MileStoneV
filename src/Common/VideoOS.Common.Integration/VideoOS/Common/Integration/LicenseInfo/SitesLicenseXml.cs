using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace VideoOS.Common.Integration.LicenseInfo;

public class SitesLicenseXml
{
	private static class XPathExpressionCache
	{
		private static Dictionary<string, XPathExpression> _expressions = new Dictionary<string, XPathExpression>();

		public static XPathExpression Name => GetExpression("Name", "Name");

		public static XPathExpression Sites => GetExpression("Sites", "Sites");

		public static XPathExpression HardwareCount => GetExpression("HardwareCount", "HardwareActivated");

		public static XPathExpression ICPCount => GetExpression("ICPCount", "ICPCameraFeedsActivated");

		public static XPathExpression CameraCount => GetExpression("CameraCount", "CameraFeedsActivated");

		public static XPathExpression Site => GetExpression("Site", "Site");

		private static XPathExpression GetExpression(string key, string xpath)
		{
			lock (_expressions)
			{
				if (!_expressions.TryGetValue(key, out var value))
				{
					value = XPathExpression.Compile(xpath);
					_expressions.Add(key, value);
				}
				return value;
			}
		}
	}

	private string xml;

	public Collection<SiteLicense> Sites { get; private set; }

	public bool HasData { get; private set; }

	public SitesLicenseXml()
	{
		Sites = new Collection<SiteLicense>();
		HasData = false;
		xml = string.Empty;
	}

	public static SitesLicenseXml FromXml(string sitesInfoXml)
	{
		SitesLicenseXml sitesLicenseXml = new SitesLicenseXml();
		if (string.IsNullOrEmpty(sitesInfoXml))
		{
			return sitesLicenseXml;
		}
		sitesLicenseXml.xml = sitesInfoXml;
		try
		{
			using StringReader textReader = new StringReader(sitesInfoXml);
			XPathNavigator rootNavigator = new XPathDocument(textReader).CreateNavigator();
			sitesLicenseXml.Sites = GetSitesLicenses(rootNavigator);
			sitesLicenseXml.HasData = true;
		}
		catch (XmlException)
		{
			sitesLicenseXml.HasData = false;
		}
		catch (XPathException)
		{
			sitesLicenseXml.HasData = false;
		}
		catch (ArgumentException)
		{
			sitesLicenseXml.HasData = false;
		}
		return sitesLicenseXml;
	}

	private static string GetSiteName(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.Name);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetHardwareCount(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.HardwareCount);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetICPCameraCount(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.ICPCount);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetCameraCount(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CameraCount);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static Collection<SiteLicense> GetSitesLicenses(XPathNavigator rootNavigator)
	{
		List<SiteLicense> list = new List<SiteLicense>();
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.Sites);
		if (xPathNavigator != null)
		{
			foreach (XPathNavigator item in xPathNavigator.Select(XPathExpressionCache.Site))
			{
				string attribute = item.GetAttribute("Id", string.Empty);
				string siteName = GetSiteName(item);
				if (!int.TryParse(GetHardwareCount(item), out var result))
				{
					result = int.MaxValue;
				}
				if (!int.TryParse(GetICPCameraCount(item), out var result2))
				{
					result2 = int.MaxValue;
				}
				if (!int.TryParse(GetCameraCount(item), out var result3))
				{
					result3 = int.MaxValue;
				}
				list.Add(new SiteLicense(attribute, siteName, result, result2, result3));
			}
		}
		return new Collection<SiteLicense>(list);
	}

	public string ToXml()
	{
		if (xml != null)
		{
			return xml;
		}
		return string.Empty;
	}
}
