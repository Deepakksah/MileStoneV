using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace VideoOS.Common.Integration.LicenseInfo;

public class LicenseOwnerInformation
{
	private static class XPathExpressionCache
	{
		private static Dictionary<string, XPathExpression> _expressions = new Dictionary<string, XPathExpression>();

		public static XPathExpression CustomerName => GetExpression("CustomerName", "Customer/Name");

		public static XPathExpression ContactName => GetExpression("ContactName", "Customer/Contact/Name");

		public static XPathExpression ContactEmail => GetExpression("ContactEmail", "Customer/Contact/Email");

		public static XPathExpression CustomerStreet => GetExpression("CustomerStreet", "Customer/Street");

		public static XPathExpression CustomerCity => GetExpression("CustomerCity", "Customer/City");

		public static XPathExpression CustomerZip => GetExpression("CustomerZip", "Customer/Zip");

		public static XPathExpression CustomerState => GetExpression("CustomerState", "Customer/State");

		public static XPathExpression CustomerCountry => GetExpression("CustomerCountry", "Customer/Country");

		public static XPathExpression ContactPhone => GetExpression("ContactPhone", "Customer/Contact/Phone");

		public static XPathExpression Pma => GetExpression("Pma", "Customer/PMA/Expire");

		public static XPathExpression Premium => GetExpression("Premium", "Customer/PMA/PremiumExpire");

		public static XPathExpression CareId => GetExpression("CareId", "Customer/PMA/Id");

		public static XPathExpression CareLevel => GetExpression("CareLevel", "Customer/PMA/Level");

		public static XPathExpression EditOwnerLink => GetExpression("EditOwnerLink", "Customer/EditOwnerLink");

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

	public string CustomerErrorMessage { get; private set; }

	public string CustomerName { get; private set; }

	public string ContactName { get; private set; }

	public string ContactAddress { get; private set; }

	public string ContactEmail { get; private set; }

	public string ContactPhone { get; private set; }

	public DateTime SupportPlanDate { get; private set; }

	public DateTime CarePremiumExpiryDate { get; private set; }

	public string CareId { get; private set; }

	public string CareLevel { get; private set; }

	public string EditOwnerLink { get; private set; }

	public bool HasData { get; private set; }

	public LicenseOwnerInformation()
	{
		SetDefault();
		CustomerErrorMessage = string.Empty;
		xml = string.Empty;
	}

	public static LicenseOwnerInformation FromXml(string ownerXml)
	{
		LicenseOwnerInformation licenseOwnerInformation = new LicenseOwnerInformation();
		if (string.IsNullOrEmpty(ownerXml))
		{
			return licenseOwnerInformation;
		}
		try
		{
			using StringReader textReader = new StringReader(ownerXml);
			XPathNavigator rootNavigator = new XPathDocument(textReader).CreateNavigator();
			licenseOwnerInformation.ContactName = GetContactName(rootNavigator);
			licenseOwnerInformation.CustomerName = GetCustomerName(rootNavigator);
			licenseOwnerInformation.ContactAddress = GetCustomerAddress(rootNavigator);
			licenseOwnerInformation.ContactEmail = GetContactEmail(rootNavigator);
			licenseOwnerInformation.ContactPhone = GetContactPhone(rootNavigator);
			licenseOwnerInformation.EditOwnerLink = GetEditOwnerLink(rootNavigator);
			licenseOwnerInformation.SupportPlanDate = GetPma(rootNavigator);
			licenseOwnerInformation.CarePremiumExpiryDate = GetPremiumExpiry(rootNavigator);
			licenseOwnerInformation.CareId = GetCareId(rootNavigator);
			licenseOwnerInformation.CareLevel = GetCareLevel(rootNavigator);
			licenseOwnerInformation.HasData = true;
			licenseOwnerInformation.xml = ownerXml;
		}
		catch (XmlException ex)
		{
			licenseOwnerInformation.CustomerErrorMessage = ex.Message;
		}
		catch (XPathException ex2)
		{
			licenseOwnerInformation.CustomerErrorMessage = ex2.Message;
		}
		catch (ArgumentException ex3)
		{
			licenseOwnerInformation.CustomerErrorMessage = ex3.Message;
		}
		return licenseOwnerInformation;
	}

	public string ToXml()
	{
		if (xml != null)
		{
			return xml;
		}
		return string.Empty;
	}

	private void SetDefault()
	{
		ContactName = "N/A";
		CustomerName = "N/A";
		ContactAddress = "N/A";
		ContactEmail = "N/A";
		ContactPhone = "N/A";
		SupportPlanDate = DateTime.MaxValue;
		CarePremiumExpiryDate = DateTime.MaxValue;
		CareId = "N/A";
		CareLevel = "N/A";
	}

	private static DateTime GetPma(XPathNavigator rootNavigator)
	{
		return GetExpiryDate(rootNavigator, XPathExpressionCache.Pma);
	}

	private static DateTime GetPremiumExpiry(XPathNavigator rootNavigator)
	{
		return GetExpiryDate(rootNavigator, XPathExpressionCache.Premium);
	}

	private static DateTime GetExpiryDate(XPathNavigator rootNavigator, XPathExpression expiryDateTypeExpression)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(expiryDateTypeExpression);
		if (xPathNavigator != null && DateTime.TryParse(xPathNavigator.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
		{
			return DateTime.SpecifyKind(result, DateTimeKind.Utc);
		}
		return DateTime.MaxValue;
	}

	private static string GetEditOwnerLink(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.EditOwnerLink);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetContactPhone(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.ContactPhone);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetContactEmail(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.ContactEmail);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetCustomerAddress(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CustomerStreet);
		string text = ((xPathNavigator != null) ? xPathNavigator.Value : string.Empty);
		xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CustomerZip);
		string text2 = ((xPathNavigator != null) ? xPathNavigator.Value : string.Empty);
		xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CustomerCity);
		string text3 = ((xPathNavigator != null) ? xPathNavigator.Value : string.Empty);
		xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CustomerState);
		string text4 = ((xPathNavigator != null) ? xPathNavigator.Value : string.Empty);
		xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CustomerCountry);
		string text5 = ((xPathNavigator != null) ? xPathNavigator.Value : string.Empty);
		string[] value = new string[3]
		{
			"{0}",
			(!string.IsNullOrEmpty(text2)) ? "{1} {2}" : "{1}{2}",
			(!string.IsNullOrEmpty(text4)) ? "{3} {4}" : "{3}{4}"
		};
		return string.Format(CultureInfo.CurrentCulture, string.Join(Environment.NewLine, value), text, text2, text3, text4, text5);
	}

	private static string GetCustomerName(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CustomerName);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetContactName(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.ContactName);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetCareId(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CareId);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}

	private static string GetCareLevel(XPathNavigator rootNavigator)
	{
		XPathNavigator xPathNavigator = rootNavigator.SelectSingleNode(XPathExpressionCache.CareLevel);
		if (xPathNavigator == null)
		{
			return string.Empty;
		}
		return xPathNavigator.Value;
	}
}
