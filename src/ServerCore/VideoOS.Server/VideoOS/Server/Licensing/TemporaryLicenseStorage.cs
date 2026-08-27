using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Licensing;

public sealed class TemporaryLicenseStorage : ITemporaryLicenseStorage, ITemporaryChangesWithoutActivation
{
	private bool _hasChanges;

	private Dictionary<string, HashSet<TemporaryDeviceLicense>> _deviceLicensesByMacAddress;

	private bool _isInitialized;

	public Guid HomeSiteId { get; private set; }

	public Guid Id { get; private set; }

	public string RSA { get; private set; }

	public Dictionary<string, TemporaryLicense> HardwareLicenses { get; private set; }

	public HashSet<TemporaryDeviceLicense> DeviceLicenses { get; private set; }

	public void LoadData(IDataComponentFactory dataComponentFactory)
	{
		string text = ReadPersistedStorageData(dataComponentFactory);
		if (string.IsNullOrEmpty(text))
		{
			throw new LicenseException("temporary license repository is empty.", LicenseErrorNumber.TemporaryLicenseStorageNoData);
		}
		try
		{
			SetData(text, dataComponentFactory);
		}
		catch (LicenseException)
		{
			throw;
		}
		try
		{
			Site site = Site.QueryHomeSite(dataComponentFactory);
			HomeSiteId = site.Id;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new LicenseException("Failed to read confirmation data for temporary license repository.", LicenseErrorNumber.TemporaryLicenseStorageNoHomeSite, inner);
		}
		_isInitialized = true;
	}

	private static string ReadPersistedStorageData(IDataComponentFactory dataComponentFactory)
	{
		try
		{
			using IConfigurationDataComponent configurationDataComponent = dataComponentFactory.CreateConfigurationDataComponent();
			return configurationDataComponent.QueryTemporaryLicenseStorageData();
		}
		catch (DatabaseException inner)
		{
			throw new LicenseException("Temporary license repository not found.", LicenseErrorNumber.TemporaryLicenseStorageNotFound, inner);
		}
	}

	private void SetData(string tlsData, IDataComponentFactory dataComponentFactory)
	{
		Guid id = Guid.Empty;
		string text = null;
		Dictionary<string, TemporaryLicense> hardwareLicenses = new Dictionary<string, TemporaryLicense>();
		List<TemporaryDeviceLicense> list = new List<TemporaryDeviceLicense>();
		try
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.XmlResolver = null;
			using StringReader input = new StringReader(tlsData);
			using XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
			if (!xmlReader.IsStartElement())
			{
				throw new LicenseException("Unable to move to start element.", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
			}
			if (!string.Equals(xmlReader.Name, "TLS", StringComparison.Ordinal))
			{
				throw new LicenseException("Root element name invalid.", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
			}
			if (!xmlReader.MoveToAttribute("Id"))
			{
				throw new LicenseException("Root element attribute missing. 'Id'", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
			}
			if (!xmlReader.Value.IsGuid())
			{
				throw new LicenseException("Root element attribute value invalid. 'Id'", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
			}
			id = new Guid(xmlReader.Value);
			xmlReader.MoveToElement();
			while (!xmlReader.EOF)
			{
				XmlNodeType nodeType = xmlReader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					if (string.Equals(xmlReader.Name, "HardwareLicenses", StringComparison.Ordinal))
					{
						hardwareLicenses = ReadHardwareLicenses(xmlReader);
					}
					else if (string.Equals(xmlReader.Name, "DeviceLicenses", StringComparison.Ordinal))
					{
						list = ReadDeviceLicenses(xmlReader, dataComponentFactory);
					}
					else if (string.Equals(xmlReader.Name, "RSAKeyValue", StringComparison.Ordinal))
					{
						text = xmlReader.ReadOuterXml();
					}
					else
					{
						xmlReader.Read();
					}
				}
				else
				{
					xmlReader.Read();
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new LicenseException("Failed to parse temporary license repository data.", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid, inner);
		}
		if (string.IsNullOrEmpty(text))
		{
			throw new LicenseException("RSA information not available", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
		}
		Id = id;
		RSA = text;
		HardwareLicenses = hardwareLicenses;
		DeviceLicenses = new HashSet<TemporaryDeviceLicense>();
		_deviceLicensesByMacAddress = new Dictionary<string, HashSet<TemporaryDeviceLicense>>();
		foreach (TemporaryDeviceLicense item in list)
		{
			AddDeviceLicenseInternal(item);
		}
	}

	private static List<TemporaryDeviceLicense> ReadDeviceLicenses(XmlReader reader, IDataComponentFactory dataComponentFactory)
	{
		List<TemporaryDeviceLicense> list = new List<TemporaryDeviceLicense>();
		if (reader.IsEmptyElement)
		{
			reader.Read();
			return list;
		}
		while (reader.Read())
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Element:
			{
				if (!string.Equals(reader.Name, "Device", StringComparison.Ordinal))
				{
					break;
				}
				string text = ReadAttributeValue(reader, "MacAddress");
				string changesWithoutActivationToken = ReadAttributeValue(reader, "Token", optional: true) ?? string.Empty;
				string text2 = ReadAttributeValue(reader, "UniqueId", optional: true) ?? string.Empty;
				Guid originalDeviceId = GetOriginalDeviceId(reader, dataComponentFactory, text);
				if (string.IsNullOrEmpty(text2))
				{
					if (originalDeviceId != Guid.Empty)
					{
						DateTime expiryDateUtc = ReadAttributeValueAsDateTime(reader, "Expires");
						using (IConfigurationDataComponent configurationDataComponent = dataComponentFactory.CreateConfigurationDataComponent())
						{
							text2 = configurationDataComponent.QueryUniqueIdByMacAddressAndOriginalId(text, originalDeviceId.ToString());
						}
						if (string.IsNullOrWhiteSpace(text2))
						{
							text2 = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", text, originalDeviceId);
						}
						TemporaryDeviceLicense temporaryDeviceLicense = new TemporaryDeviceLicense(text, text2, expiryDateUtc, changesWithoutActivationToken);
						temporaryDeviceLicense.OriginalDeviceId = originalDeviceId;
						list.Add(temporaryDeviceLicense);
					}
				}
				else
				{
					DateTime expiryDateUtc2 = ReadAttributeValueAsDateTime(reader, "Expires");
					TemporaryDeviceLicense temporaryDeviceLicense2 = new TemporaryDeviceLicense(text, text2, expiryDateUtc2, changesWithoutActivationToken);
					temporaryDeviceLicense2.OriginalDeviceId = originalDeviceId;
					list.Add(temporaryDeviceLicense2);
				}
				break;
			}
			case XmlNodeType.EndElement:
				if (string.Equals(reader.Name, "DeviceLicenses", StringComparison.Ordinal))
				{
					return list;
				}
				break;
			}
		}
		return list;
	}

	private static Guid GetOriginalDeviceId(XmlReader reader, IDataComponentFactory dataComponentFactory, string macAddress)
	{
		Guid result;
		try
		{
			ReadAttributeValueAsGuid(reader, "Index", out result);
		}
		catch (LicenseException)
		{
			ReadAttributeValueAsInt32(reader, "Index", out var result2);
			using IConfigurationDataComponent configurationDataComponent = dataComponentFactory.CreateConfigurationDataComponent();
			return configurationDataComponent.QueryOriginalIdByMacAddressDeviceIndex(macAddress, result2);
		}
		return result;
	}

	private static Dictionary<string, TemporaryLicense> ReadHardwareLicenses(XmlReader reader)
	{
		Dictionary<string, TemporaryLicense> dictionary = new Dictionary<string, TemporaryLicense>();
		if (reader.IsEmptyElement)
		{
			reader.Read();
			return dictionary;
		}
		while (reader.Read())
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Element:
				if (string.Equals(reader.Name, "HardwareLicense", StringComparison.Ordinal))
				{
					string text = ReadAttributeValue(reader, "MacAddress");
					DateTime dateTime = ReadAttributeValueAsDateTime(reader, "Expires");
					string replacementToken = ReadAttributeValue(reader, "ChangesWithoutActivationToken", optional: true);
					if (dateTime != DateTime.MaxValue && dateTime != DateTime.MaxValue.Date)
					{
						TemporaryLicense value = new TemporaryLicense(text, dateTime, replacementToken);
						dictionary.Add(text, value);
					}
				}
				break;
			case XmlNodeType.EndElement:
				if (string.Equals(reader.Name, "HardwareLicenses", StringComparison.Ordinal))
				{
					return dictionary;
				}
				break;
			}
		}
		return dictionary;
	}

	private static string ReadAttributeValue(XmlReader reader, string attributeName, bool optional = false)
	{
		string result = "";
		if (!reader.MoveToAttribute(attributeName))
		{
			if (!optional)
			{
				throw new LicenseException("Attribute missing. '" + attributeName + "'", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
			}
		}
		else
		{
			result = reader.Value;
		}
		return result;
	}

	private static void ReadAttributeValueAsGuid(XmlReader reader, string attributeName, out Guid result)
	{
		string input = ReadAttributeValue(reader, attributeName);
		result = Guid.Empty;
		if (!Guid.TryParse(input, out result))
		{
			throw new LicenseException("Attribute value invalid. '" + attributeName + "'", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
		}
	}

	private static void ReadAttributeValueAsInt32(XmlReader reader, string attributeName, out int result)
	{
		result = -1;
		string s = ReadAttributeValue(reader, attributeName);
		if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
		{
			throw new LicenseException("Attribute value invalid. '" + attributeName + "'", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
		}
	}

	private static DateTime ReadAttributeValueAsDateTime(XmlReader reader, string attributeName)
	{
		string s = ReadAttributeValue(reader, attributeName);
		if (!DateTime.TryParseExact(s, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var result))
		{
			if (!long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
			{
				throw new LicenseException("Attribute value invalid. '" + attributeName + "'", LicenseErrorNumber.TemporaryLicenseStorageFormatInvalid);
			}
			result = new DateTime(result2);
		}
		return result;
	}

	public bool PersistData(IDataComponentFactory dataComponentFactory)
	{
		ThrowIfNotInitialized();
		if (!_hasChanges)
		{
			return false;
		}
		string dataAsXml = GetDataAsXml();
		WritePersistedStorageData(dataComponentFactory, dataAsXml);
		_hasChanges = false;
		return true;
	}

	private static void WritePersistedStorageData(IDataComponentFactory dataComponentFactory, string data)
	{
		using IConfigurationDataComponent configurationDataComponent = dataComponentFactory.CreateConfigurationDataComponent();
		configurationDataComponent.WriteTemporaryLicenseStorageData(data);
	}

	private string GetDataAsXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		xmlWriterSettings.Encoding = Encoding.UTF8;
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
		{
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("TLS");
			xmlWriter.WriteAttributeString("Id", Id.ToString().ToUpperInvariant());
			xmlWriter.WriteRaw(RSA);
			xmlWriter.WriteStartElement("HardwareLicenses");
			foreach (TemporaryLicense value in HardwareLicenses.Values)
			{
				xmlWriter.WriteStartElement("HardwareLicense");
				xmlWriter.WriteAttributeString("MacAddress", value.MacAddress);
				xmlWriter.WriteAttributeString("Expires", value.ExpiryDateUtc.ToString("o", CultureInfo.InvariantCulture));
				xmlWriter.WriteAttributeString("ChangesWithoutActivationToken", value.ChangesWithoutActivationToken);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("DeviceLicenses");
			foreach (TemporaryDeviceLicense deviceLicense in DeviceLicenses)
			{
				xmlWriter.WriteStartElement("Device");
				xmlWriter.WriteAttributeString("MacAddress", deviceLicense.MacAddress);
				xmlWriter.WriteAttributeString("Index", deviceLicense.OriginalDeviceId.ToString());
				xmlWriter.WriteAttributeString("UniqueId", deviceLicense.UniqueIdentifier);
				xmlWriter.WriteAttributeString("Expires", deviceLicense.ExpiryDateUtc.ToString("o", CultureInfo.InvariantCulture));
				xmlWriter.WriteAttributeString("Token", deviceLicense.ChangesWithoutActivationToken);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
		}
		return stringBuilder.ToString();
	}

	public void Verify()
	{
		ThrowIfNotInitialized();
		if (HardwareLicenses == null)
		{
			throw new LicenseException("Temporary licenses not initialized", LicenseErrorNumber.TemporaryLicenseStorageNotInitialized);
		}
		if (HomeSiteId != Id)
		{
			throw new LicenseException("Temporary license repository not confirmed", LicenseErrorNumber.TemporaryLicenseStorageNotConfirmed);
		}
	}

	public void AddLicense(TemporaryLicense temporaryLicense)
	{
		ThrowIfNotInitialized();
		HardwareLicenses[temporaryLicense.MacAddress] = temporaryLicense;
		_hasChanges = true;
	}

	public void RemoveHardwareLicense(string macAddress)
	{
		ThrowIfNotInitialized();
		HardwareLicenses.Remove(macAddress);
		_hasChanges = true;
	}

	public TemporaryLicense FindLicense(string macAddress)
	{
		ThrowIfNotInitialized();
		if (HardwareLicenses.TryGetValue(macAddress, out var value))
		{
			return value;
		}
		return null;
	}

	public void AddDeviceLicense(TemporaryDeviceLicense temporaryDeviceLicense)
	{
		ThrowIfNotInitialized();
		if (temporaryDeviceLicense == null)
		{
			throw new ArgumentNullException("temporaryDeviceLicense");
		}
		AddDeviceLicenseInternal(temporaryDeviceLicense);
	}

	public void ClearChangesWithoutActivationTokens()
	{
		foreach (TemporaryLicense value in HardwareLicenses.Values)
		{
			value.ClearChangesWithoutActivationToken();
		}
		foreach (TemporaryDeviceLicense deviceLicense in DeviceLicenses)
		{
			deviceLicense.ClearChangesWithoutActivationToken();
		}
	}

	public Collection<string> FindHardwareWithPreviousYearTokens(DateTime activated)
	{
		Collection<string> collection = new Collection<string>();
		foreach (TemporaryLicense value in HardwareLicenses.Values)
		{
			if (value.IsChangedWithoutActivation() && !value.HasCurrentYearToken(activated))
			{
				collection.Add(value.MacAddress);
			}
		}
		return collection;
	}

	public Collection<IHardwareDeviceItem> FindHardwareDevicesWithPreviousYearTokens(DateTime activated)
	{
		Collection<IHardwareDeviceItem> collection = new Collection<IHardwareDeviceItem>();
		foreach (TemporaryDeviceLicense deviceLicense in DeviceLicenses)
		{
			if (!deviceLicense.HasCurrentYearToken(activated))
			{
				collection.Add(LicenseFactory.CreateHardwareDeviceItem(deviceLicense.MacAddress, deviceLicense.UniqueIdentifier.ToString()));
			}
		}
		return collection;
	}

	private void AddDeviceLicenseInternal(TemporaryDeviceLicense temporaryDeviceLicense)
	{
		DeviceLicenses.Add(temporaryDeviceLicense);
		string macAddress = temporaryDeviceLicense.MacAddress;
		if (!_deviceLicensesByMacAddress.ContainsKey(macAddress))
		{
			_deviceLicensesByMacAddress[macAddress] = new HashSet<TemporaryDeviceLicense>();
		}
		_deviceLicensesByMacAddress[temporaryDeviceLicense.MacAddress].Add(temporaryDeviceLicense);
		_hasChanges = true;
	}

	public TemporaryDeviceLicense FindDeviceLicense(string macAddress, string uniqueDeviceId)
	{
		ThrowIfNotInitialized();
		if (!_deviceLicensesByMacAddress.ContainsKey(macAddress))
		{
			return null;
		}
		HashSet<TemporaryDeviceLicense> source = _deviceLicensesByMacAddress[macAddress];
		return source.OrderByDescending((TemporaryDeviceLicense o) => o.ExpiryDateUtc).FirstOrDefault((TemporaryDeviceLicense t) => t.UniqueIdentifier == uniqueDeviceId);
	}

	public void RemoveDeviceLicense(string macAddress, string uniqueDeviceId)
	{
		ThrowIfNotInitialized();
		if (_deviceLicensesByMacAddress.ContainsKey(macAddress))
		{
			HashSet<TemporaryDeviceLicense> hashSet = _deviceLicensesByMacAddress[macAddress];
			TemporaryDeviceLicense temporaryDeviceLicense = hashSet.FirstOrDefault((TemporaryDeviceLicense l) => l.UniqueIdentifier == uniqueDeviceId);
			if (temporaryDeviceLicense != null)
			{
				DeviceLicenses.Remove(temporaryDeviceLicense);
				hashSet.Remove(temporaryDeviceLicense);
				_hasChanges = true;
			}
		}
	}

	public HashSet<string> LoadTemporaryHardwareChangesWithoutActivationIdentifiers(DateTime activatedDate)
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (HardwareLicenses != null)
		{
			foreach (TemporaryLicense value in HardwareLicenses.Values)
			{
				if (value.IsChangedWithoutActivation() && value.HasCurrentYearToken(activatedDate))
				{
					hashSet.Add(value.MacAddress);
				}
			}
		}
		return hashSet;
	}

	public HashSet<string> LoadTemporaryDeviceChangesWithoutActivationIdentifiers(DateTime activatedDate)
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (DeviceLicenses != null)
		{
			foreach (TemporaryDeviceLicense deviceLicense in DeviceLicenses)
			{
				if (deviceLicense.IsChangedWithoutActivation() && deviceLicense.HasCurrentYearToken(activatedDate))
				{
					hashSet.Add(deviceLicense.MacAddress + "-" + deviceLicense.UniqueIdentifier);
				}
			}
		}
		return hashSet;
	}

	private void ThrowIfNotInitialized()
	{
		if (!_isInitialized)
		{
			throw new LicenseException("Temporary licenses not initialized", LicenseErrorNumber.TemporaryLicenseStorageNotInitialized);
		}
	}
}
