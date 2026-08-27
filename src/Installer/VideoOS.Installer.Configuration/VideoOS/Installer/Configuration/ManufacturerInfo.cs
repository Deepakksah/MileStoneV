using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace VideoOS.Installer.Configuration;

[Serializable]
[XmlRoot(ElementName = "ManufacturerInformation")]
public class ManufacturerInfo
{
	private const string ManufacturerNameInRegistry = "ManufacturerName";

	private const string SKUInRegistry = "SKU";

	private const string PartnerGuidInRegistry = "PartnerGuid";

	private static readonly List<char> InvalidChars = new List<char> { '<', '>', '*', '%', ':', '?', '\\' };

	[XmlElement(ElementName = "PartnerGUID")]
	public Guid PartnerGuid { get; set; }

	[XmlElement(ElementName = "ManufacturerName")]
	public string ManufacturerName { get; set; }

	[XmlElement(ElementName = "SKU")]
	public string Sku { get; set; }

	private bool HasContentEmpty
	{
		get
		{
			if (!(PartnerGuid == Guid.Empty) && !string.IsNullOrEmpty(ManufacturerName))
			{
				return string.IsNullOrEmpty(Sku);
			}
			return true;
		}
	}

	private bool HasInvalidChar
	{
		get
		{
			if (!DoesContainsInvalidCharacters(Sku))
			{
				return DoesContainsInvalidCharacters(ManufacturerName);
			}
			return true;
		}
	}

	private static bool DoesContainsInvalidCharacters(string target)
	{
		return InvalidChars.Any((char c) => target.Contains(c));
	}

	private void ValidManufacturerInfo()
	{
		if (HasContentEmpty)
		{
			throw new ManufacturerInfoException("The specified manufacturer information should not have empty content fields.");
		}
		if (HasInvalidChar)
		{
			throw new ManufacturerInfoException("The specified manufacturer information contains unsupported characters.");
		}
	}

	public static ManufacturerInfo LoadFromFile(string path)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ManufacturerInfo));
			using StreamReader textReader = new StreamReader(path);
			ManufacturerInfo obj = (ManufacturerInfo)xmlSerializer.Deserialize(textReader);
			obj.ValidManufacturerInfo();
			return obj;
		}
		catch (UnauthorizedAccessException innerException)
		{
			throw new ManufacturerInfoException("There is an error reading the manufacturer information XML.", innerException);
		}
		catch (IOException innerException2)
		{
			throw new ManufacturerInfoException("There is an error reading the manufacturer information XML.", innerException2);
		}
		catch (InvalidOperationException innerException3)
		{
			throw new ManufacturerInfoException("The manufacturer information XML is invalid.", innerException3);
		}
	}

	public static ManufacturerInfo LoadFromRegistry(string key)
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
			if (registryKey == null)
			{
				return null;
			}
			ManufacturerInfo manufacturerInfo = new ManufacturerInfo();
			manufacturerInfo.ManufacturerName = registryKey.GetValue("ManufacturerName") as string;
			manufacturerInfo.Sku = registryKey.GetValue("SKU") as string;
			if (Guid.TryParse(registryKey.GetValue("PartnerGuid") as string, out var result))
			{
				manufacturerInfo.PartnerGuid = result;
			}
			manufacturerInfo.ValidManufacturerInfo();
			return manufacturerInfo;
		}
		catch (SecurityException innerException)
		{
			throw new ManufacturerInfoException("There is an error reading the manufacturer information from the registry.", innerException);
		}
		catch (IOException innerException2)
		{
			throw new ManufacturerInfoException("There is an error reading the manufacturer information from the registry.", innerException2);
		}
		catch (UnauthorizedAccessException innerException3)
		{
			throw new ManufacturerInfoException("There is an error reading the manufacturer information from the registry.", innerException3);
		}
	}

	public void SaveToRegistry(string key)
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(key);
			if (registryKey != null)
			{
				registryKey.SetValue("ManufacturerName", ManufacturerName);
				registryKey.SetValue("SKU", Sku);
				registryKey.SetValue("PartnerGuid", PartnerGuid);
				return;
			}
			throw new ManufacturerInfoException("There is an error storing manufacturer information in the registry.");
		}
		catch (UnauthorizedAccessException innerException)
		{
			throw new ManufacturerInfoException("There is an error storing manufacturer information in the registry.", innerException);
		}
		catch (SecurityException innerException2)
		{
			throw new ManufacturerInfoException("There is an error storing manufacturer information in the registry.", innerException2);
		}
		catch (IOException innerException3)
		{
			throw new ManufacturerInfoException("There is an error storing manufacturer information in the registry.", innerException3);
		}
	}
}
