using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public class LicenseInformation
{
	public LicenseType LicenseType
	{
		get
		{
			bool flag = Features.Where((FeatureInformation f) => f.Name == "HardwareLicensing").Count() > 0;
			bool flag2 = Features.Where((FeatureInformation f) => f.Name == "ChannelLicensing").Count() > 0;
			if (flag && !flag2)
			{
				return LicenseType.Hardware;
			}
			if (flag2 && !flag)
			{
				return LicenseType.Channel;
			}
			return LicenseType.Unknown;
		}
	}

	public LicenseState LicenseState { get; set; }

	public string LicenseErrorMessage { get; set; }

	public string ProductName { get; set; }

	public string Slc { get; set; }

	public string EncryptedSlc { get; set; }

	public LicenseOwnerInformation Owner { get; set; }

	public LicenseReportInformation LicenseReport { get; private set; }

	public SitesLicenseXml SitesLicenseXml { get; set; }

	public DateTime LicenseExpirationDate { get; set; }

	public DateTime LicenseActivationDate { get; set; }

	public Collection<FeatureInformation> Features { get; private set; }

	public ITotalLicense TotalActivated { get; private set; }

	public RecorderLicenseInformation RecorderLicenses { get; private set; }

	public HardwareLicenseInformation HardwareLicenses { get; private set; }

	public HardwareLicenseInformation ArculesHardwareLicenses { get; private set; }

	public Collection<FeedLicenseInformation> FeedLicenses { get; private set; }

	public Collection<FeedLicenseInformation> ICPFeedLicenses { get; private set; }

	public bool UpdateRequired
	{
		get
		{
			if ((long)HardwareLicenses.Missing <= 0L && (long)ArculesHardwareLicenses.Missing <= 0L && !HasMissingFeedLicenses(FeedLicenses))
			{
				return HasMissingFeedLicenses(ICPFeedLicenses);
			}
			return true;
		}
	}

	public LicenseInformation()
	{
		LicenseState = LicenseState.Unknown;
		LicenseErrorMessage = string.Empty;
		ProductName = string.Empty;
		Slc = string.Empty;
		EncryptedSlc = string.Empty;
		Owner = new LicenseOwnerInformation();
		SitesLicenseXml = new SitesLicenseXml();
		LicenseExpirationDate = DateTime.MaxValue;
		LicenseExpirationDate = DateTime.MinValue;
		Features = new Collection<FeatureInformation>();
		TotalActivated = LicenseFactory.CreateNewTotalLicense(0, 0, 0, 0);
		RecorderLicenses = new RecorderLicenseInformation();
		HardwareLicenses = new HardwareLicenseInformation();
		ArculesHardwareLicenses = new HardwareLicenseInformation();
		FeedLicenses = new Collection<FeedLicenseInformation>();
		ICPFeedLicenses = new Collection<FeedLicenseInformation>();
		LicenseReport = new LicenseReportInformation();
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = null;
		try
		{
			stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("LicenseInformation");
			xmlTextWriter.WriteElementString("State", LicenseState.ToString());
			xmlTextWriter.WriteElementString("ErrorMessage", LicenseErrorMessage ?? string.Empty);
			xmlTextWriter.WriteElementString("ExpirationDate", LicenseExpirationDate.ToString("o", CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("ActivationDate", LicenseActivationDate.ToString("o", CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString("SLC", Slc ?? string.Empty);
			xmlTextWriter.WriteElementString("ProductName", ProductName ?? string.Empty);
			xmlTextWriter.WriteElementString("EncryptedSlc", EncryptedSlc ?? string.Empty);
			xmlTextWriter.WriteElementString("OwnerXml", Owner.ToXml());
			xmlTextWriter.WriteElementString("SitesInformation", SitesLicenseXml.ToXml());
			xmlTextWriter.WriteStartElement("Features");
			foreach (FeatureInformation feature in Features)
			{
				xmlTextWriter.WriteStartElement("Feature");
				xmlTextWriter.WriteAttributeString("Name", feature.Name ?? string.Empty);
				xmlTextWriter.WriteAttributeString("ExpirationDate", feature.ExpiryDate.ToString("o", CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Licensed", feature.Licensed.ToString());
				xmlTextWriter.WriteAttributeString("Visible", feature.Visible.ToString());
				if (feature.Parameters.Any())
				{
					xmlTextWriter.WriteStartElement("Parameters");
					foreach (FeatureParameterInformation parameter in feature.Parameters)
					{
						xmlTextWriter.WriteStartElement("Parameter");
						xmlTextWriter.WriteAttributeString("Name", parameter.Name ?? string.Empty);
						xmlTextWriter.WriteAttributeString("Value", parameter.Value ?? string.Empty);
						xmlTextWriter.WriteEndElement();
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("RecorderLicenses");
			xmlTextWriter.WriteAttributeString("Available", RecorderLicenses.Available.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Used", RecorderLicenses.Used.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Missing", RecorderLicenses.Missing.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("HardwareLicenses");
			xmlTextWriter.WriteAttributeString("Available", HardwareLicenses.Available.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Used", HardwareLicenses.Used.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Temporary", HardwareLicenses.Temporary.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Expired", HardwareLicenses.Expired.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Replaced", HardwareLicenses.Replaced.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("TotalReplacementLimit", HardwareLicenses.TotalReplacementLimit.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Missing", HardwareLicenses.Missing.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("FirstGraceExpirationDate", HardwareLicenses.FirstGraceExpirationDate.ToString("o", CultureInfo.InvariantCulture));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("ArculesHardwareLicenses");
			xmlTextWriter.WriteAttributeString("Available", ArculesHardwareLicenses.Available.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Used", ArculesHardwareLicenses.Used.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Temporary", ArculesHardwareLicenses.Temporary.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Expired", ArculesHardwareLicenses.Expired.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Replaced", ArculesHardwareLicenses.Replaced.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("TotalReplacementLimit", ArculesHardwareLicenses.TotalReplacementLimit.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("Missing", ArculesHardwareLicenses.Missing.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("FirstGraceExpirationDate", ArculesHardwareLicenses.FirstGraceExpirationDate.ToString("o", CultureInfo.InvariantCulture));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("TotalActivated");
			xmlTextWriter.WriteAttributeString("HardwareCount", TotalActivated.HardwareCount.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("CameraCount", TotalActivated.CameraCount.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("ICPCameraCount", TotalActivated.ICPCameraCount.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteAttributeString("ArculesCameraCount", TotalActivated.ArculesCameraCount.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("FeedLicenses");
			foreach (FeedLicenseInformation feedLicense in FeedLicenses)
			{
				xmlTextWriter.WriteStartElement("FeedLicense");
				xmlTextWriter.WriteAttributeString("Name", feedLicense.TypeName);
				xmlTextWriter.WriteAttributeString("Available", feedLicense.Available.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Used", feedLicense.Used.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Temporary", feedLicense.Temporary.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Expired", feedLicense.Expired.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Missing", feedLicense.Missing.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("ICPFeedLicenses");
			foreach (FeedLicenseInformation iCPFeedLicense in ICPFeedLicenses)
			{
				xmlTextWriter.WriteStartElement("ICPFeedLicense");
				xmlTextWriter.WriteAttributeString("Name", iCPFeedLicense.TypeName);
				xmlTextWriter.WriteAttributeString("Available", iCPFeedLicense.Available.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Used", iCPFeedLicense.Used.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Temporary", iCPFeedLicense.Temporary.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Expired", iCPFeedLicense.Expired.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteAttributeString("Missing", iCPFeedLicense.Missing.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("LicenseReport");
			xmlTextWriter.WriteAttributeString("MissingLicenses", LicenseReport.MissingLicenses.ToString());
			xmlTextWriter.WriteAttributeString("MultiLicenseHardware", LicenseReport.MultiLicenseHardware.ToString());
			foreach (ILicenseCalculationResult licenseCalculationResult in LicenseReport.LicenseCalculationResults)
			{
				xmlTextWriter.WriteStartElement("Hardware");
				xmlTextWriter.WriteAttributeString("HardwareName", licenseCalculationResult.HardwareName);
				xmlTextWriter.WriteAttributeString("ModelName", licenseCalculationResult.ModelName);
				xmlTextWriter.WriteAttributeString("DriverName", licenseCalculationResult.DriverName);
				xmlTextWriter.WriteAttributeString("MacAddress", licenseCalculationResult.MacAddress);
				xmlTextWriter.WriteAttributeString("ChannelLicenses", licenseCalculationResult.ChannelLicenses.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
		}
		finally
		{
			stringWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}

	public static LicenseInformation FromXml(string xml, Dictionary<string, string> displayNames)
	{
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		if (displayNames == null)
		{
			throw new ArgumentNullException("displayNames");
		}
		LicenseInformation licenseInformation = new LicenseInformation();
		try
		{
			using StringReader textReader = new StringReader(xml);
			XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator();
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("LicenseInformation/State");
			licenseInformation.LicenseState = (LicenseState)Enum.Parse(typeof(LicenseState), xPathNavigator2.Value);
			licenseInformation.LicenseErrorMessage = xPathNavigator.SelectSingleNode("LicenseInformation/ErrorMessage").Value;
			XPathNavigator xPathNavigator3 = xPathNavigator.SelectSingleNode("LicenseInformation/ExpirationDate");
			licenseInformation.LicenseExpirationDate = DateTime.ParseExact(xPathNavigator3.Value, "o", CultureInfo.InvariantCulture);
			XPathNavigator xPathNavigator4 = xPathNavigator.SelectSingleNode("LicenseInformation/ActivationDate");
			if (xPathNavigator4 != null)
			{
				licenseInformation.LicenseActivationDate = DateTime.ParseExact(xPathNavigator4.Value, "o", CultureInfo.InvariantCulture);
			}
			licenseInformation.Slc = xPathNavigator.SelectSingleNode("LicenseInformation/SLC").Value;
			licenseInformation.ProductName = xPathNavigator.SelectSingleNode("LicenseInformation/ProductName").Value;
			licenseInformation.EncryptedSlc = xPathNavigator.SelectSingleNode("LicenseInformation/EncryptedSlc").Value;
			XPathNavigator xPathNavigator5 = xPathNavigator.SelectSingleNode("LicenseInformation/OwnerXml");
			if (xPathNavigator5 != null)
			{
				string value = xPathNavigator5.Value;
				licenseInformation.Owner = LicenseOwnerInformation.FromXml(value);
			}
			xPathNavigator5 = xPathNavigator.SelectSingleNode("LicenseInformation/SitesInformation");
			if (xPathNavigator5 != null)
			{
				string value2 = xPathNavigator5.Value;
				licenseInformation.SitesLicenseXml = SitesLicenseXml.FromXml(value2);
			}
			else
			{
				licenseInformation.SitesLicenseXml = new SitesLicenseXml();
			}
			foreach (XPathNavigator item3 in xPathNavigator.Select("LicenseInformation/Features/Feature"))
			{
				string attributeValueAsString = item3.GetAttributeValueAsString("Name");
				DateTime attributeValueAsDateTime = item3.GetAttributeValueAsDateTime("ExpirationDate");
				bool attributeValueAsBoolean = item3.GetAttributeValueAsBoolean("Licensed");
				bool attributeValueAsBoolean2 = item3.GetAttributeValueAsBoolean("Visible");
				string displayName = attributeValueAsString;
				if (displayNames.ContainsKey(LicenseDisplayNameKeys.ApplicationNameKey(attributeValueAsString)))
				{
					displayName = displayNames[LicenseDisplayNameKeys.ApplicationNameKey(attributeValueAsString)];
				}
				FeatureInformation featureInformation = new FeatureInformation(attributeValueAsString, displayName, attributeValueAsBoolean, attributeValueAsDateTime, attributeValueAsBoolean2);
				licenseInformation.Features.Add(featureInformation);
				foreach (XPathNavigator item4 in item3.Select("Parameters/Parameter"))
				{
					string attributeValueAsString2 = item4.GetAttributeValueAsString("Name");
					string attributeValueAsString3 = item4.GetAttributeValueAsString("Value");
					string displayName2 = attributeValueAsString2;
					if (string.Equals(attributeValueAsString2, "SLC", StringComparison.Ordinal) && displayNames.ContainsKey(LicenseDisplayNameKeys.SLCKey(attributeValueAsString)))
					{
						displayName2 = displayNames[LicenseDisplayNameKeys.SLCKey(attributeValueAsString)];
					}
					FeatureParameterInformation item = new FeatureParameterInformation(attributeValueAsString2, displayName2, attributeValueAsString3);
					featureInformation.Parameters.Add(item);
				}
			}
			XPathNavigator navigator2 = xPathNavigator.SelectSingleNode("LicenseInformation/RecorderLicenses");
			licenseInformation.RecorderLicenses.Available = navigator2.GetAttributeValueAsInt32("Available");
			licenseInformation.RecorderLicenses.Used = navigator2.GetAttributeValueAsInt32("Used");
			licenseInformation.RecorderLicenses.Missing = navigator2.GetAttributeValueAsInt32("Missing");
			XPathNavigator navigator3 = xPathNavigator.SelectSingleNode("LicenseInformation/HardwareLicenses");
			licenseInformation.HardwareLicenses.Available = navigator3.GetAttributeValueAsInt32("Available");
			licenseInformation.HardwareLicenses.Used = navigator3.GetAttributeValueAsInt32("Used");
			licenseInformation.HardwareLicenses.Temporary = navigator3.GetAttributeValueAsInt32("Temporary");
			licenseInformation.HardwareLicenses.Expired = navigator3.GetAttributeValueAsInt32("Expired");
			licenseInformation.HardwareLicenses.Replaced = navigator3.GetAttributeValueAsInt32("Replaced");
			licenseInformation.HardwareLicenses.TotalReplacementLimit = navigator3.GetAttributeValueAsInt32("TotalReplacementLimit");
			licenseInformation.HardwareLicenses.Missing = navigator3.GetAttributeValueAsInt32("Missing");
			licenseInformation.HardwareLicenses.FirstGraceExpirationDate = navigator3.GetAttributeValueAsDateTime("FirstGraceExpirationDate");
			XPathNavigator navigator4 = xPathNavigator.SelectSingleNode("LicenseInformation/ArculesHardwareLicenses");
			licenseInformation.ArculesHardwareLicenses.Available = navigator4.GetAttributeValueAsInt32("Available");
			licenseInformation.ArculesHardwareLicenses.Used = navigator4.GetAttributeValueAsInt32("Used");
			licenseInformation.ArculesHardwareLicenses.Temporary = navigator4.GetAttributeValueAsInt32("Temporary");
			licenseInformation.ArculesHardwareLicenses.Expired = navigator4.GetAttributeValueAsInt32("Expired");
			licenseInformation.ArculesHardwareLicenses.Replaced = navigator4.GetAttributeValueAsInt32("Replaced");
			licenseInformation.ArculesHardwareLicenses.TotalReplacementLimit = navigator4.GetAttributeValueAsInt32("TotalReplacementLimit");
			licenseInformation.ArculesHardwareLicenses.Missing = navigator4.GetAttributeValueAsInt32("Missing");
			licenseInformation.ArculesHardwareLicenses.FirstGraceExpirationDate = navigator4.GetAttributeValueAsDateTime("FirstGraceExpirationDate");
			XPathNavigator navigator5 = xPathNavigator.SelectSingleNode("LicenseInformation/TotalActivated");
			licenseInformation.TotalActivated.HardwareCount = navigator5.GetAttributeValueAsInt32("HardwareCount");
			licenseInformation.TotalActivated.CameraCount = navigator5.GetAttributeValueAsInt32("CameraCount");
			licenseInformation.TotalActivated.ICPCameraCount = navigator5.GetAttributeValueAsInt32("ICPCameraCount");
			licenseInformation.TotalActivated.ArculesCameraCount = navigator5.GetAttributeValueAsInt32("ArculesCameraCount");
			FromXml_ReadFeedLicenses(xPathNavigator.Select("LicenseInformation/FeedLicenses/FeedLicense"), licenseInformation.FeedLicenses);
			FromXml_ReadFeedLicenses(xPathNavigator.Select("LicenseInformation/ICPFeedLicenses/ICPFeedLicense"), licenseInformation.ICPFeedLicenses);
			XPathNavigator xPathNavigator6 = xPathNavigator.SelectSingleNode("LicenseInformation/LicenseReport");
			if (xPathNavigator6 != null)
			{
				licenseInformation.LicenseReport.MissingLicenses = xPathNavigator6.GetAttributeValueAsBoolean("MissingLicenses");
				licenseInformation.LicenseReport.MultiLicenseHardware = xPathNavigator6.GetAttributeValueAsBoolean("MultiLicenseHardware");
				foreach (XPathNavigator item5 in xPathNavigator.Select("LicenseInformation/LicenseReport/Hardware"))
				{
					string attributeValueAsString4 = item5.GetAttributeValueAsString("MacAddress");
					string attributeValueAsString5 = item5.GetAttributeValueAsString("ModelName");
					string attributeValueAsString6 = item5.GetAttributeValueAsString("DriverName");
					string attributeValueAsString7 = item5.GetAttributeValueAsString("HardwareName");
					int attributeValueAsInt = item5.GetAttributeValueAsInt32("ChannelLicenses");
					ILicenseCalculationResult item2 = LicenseFactory.CreateLicenseCalculationResult(attributeValueAsString4, attributeValueAsString5, attributeValueAsString6, attributeValueAsString7, attributeValueAsInt);
					licenseInformation.LicenseReport.LicenseCalculationResults.Add(item2);
				}
			}
		}
		catch (XmlException ex)
		{
			licenseInformation.LicenseState = LicenseState.ParseError;
			licenseInformation.LicenseErrorMessage = ex.Message;
		}
		catch (XPathException ex2)
		{
			licenseInformation.LicenseState = LicenseState.ParseError;
			licenseInformation.LicenseErrorMessage = ex2.Message;
		}
		catch (ArgumentException ex3)
		{
			licenseInformation.LicenseState = LicenseState.ParseError;
			licenseInformation.LicenseErrorMessage = ex3.Message;
		}
		return licenseInformation;
	}

	private static void FromXml_ReadFeedLicenses(XPathNodeIterator feedLicenseNodes, Collection<FeedLicenseInformation> feedLicensesCollection)
	{
		foreach (XPathNavigator feedLicenseNode in feedLicenseNodes)
		{
			FeedLicenseInformation item = new FeedLicenseInformation(feedLicenseNode.GetAttributeValueAsString("Name"))
			{
				Available = feedLicenseNode.GetAttributeValueAsInt32("Available"),
				Used = feedLicenseNode.GetAttributeValueAsInt32("Used"),
				Temporary = feedLicenseNode.GetAttributeValueAsInt32("Temporary"),
				Expired = feedLicenseNode.GetAttributeValueAsInt32("Expired"),
				Missing = feedLicenseNode.GetAttributeValueAsInt32("Missing")
			};
			feedLicensesCollection.Add(item);
		}
	}

	private static bool HasMissingFeedLicenses(IEnumerable<FeedLicenseInformation> feedLicenses)
	{
		foreach (FeedLicenseInformation feedLicense in feedLicenses)
		{
			if ((long)feedLicense.Expired > 0L || (long)feedLicense.Missing > 0L)
			{
				return true;
			}
		}
		return false;
	}
}
