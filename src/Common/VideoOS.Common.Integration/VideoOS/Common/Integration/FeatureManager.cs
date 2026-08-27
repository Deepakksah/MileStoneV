using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration;

public class FeatureManager : IFeatureManager
{
	private ILicenseDocument _licenseDocument;

	public bool HasLicense => _licenseDocument != null;

	public bool IsTrialLicense => _licenseDocument.IsTrialLicense;

	public string LicenseProductName
	{
		get
		{
			if (_licenseDocument == null)
			{
				return OemResources.Instance.App.ProductName;
			}
			return _licenseDocument.ProductName;
		}
	}

	public string SLC
	{
		get
		{
			if (_licenseDocument == null)
			{
				return string.Empty;
			}
			return _licenseDocument.SoftwareLicenseCode;
		}
	}

	public string SLCEnvironment
	{
		get
		{
			if (_licenseDocument == null)
			{
				return string.Empty;
			}
			return _licenseDocument.SoftwareLicenseCodeEnvironment;
		}
	}

	public string ProductSku
	{
		get
		{
			if (_licenseDocument == null)
			{
				return string.Empty;
			}
			return _licenseDocument.ProductSku;
		}
	}

	public bool LicenseExpires
	{
		get
		{
			if (_licenseDocument == null)
			{
				return false;
			}
			return _licenseDocument.ExpiryDate != DateTime.MinValue;
		}
	}

	public DateTime LicenseExpiryDate
	{
		get
		{
			if (_licenseDocument == null)
			{
				return DateTime.MinValue;
			}
			return _licenseDocument.ExpiryDate;
		}
	}

	public IEnumerable<string> AllFeatureNames
	{
		get
		{
			if (_licenseDocument != null)
			{
				return _licenseDocument.AllFeatureNames;
			}
			return new string[0];
		}
	}

	public IEnumerable<ILicenseProperty> AllLicenseProperties
	{
		get
		{
			if (_licenseDocument != null)
			{
				return _licenseDocument.AllLicenseProperties;
			}
			return Enumerable.Empty<ILicenseProperty>();
		}
	}

	public void Initialize(ILicenseDocument licenseDocument)
	{
		_licenseDocument = licenseDocument;
	}

	public void Refresh(ILicenseDocument licenseDocument)
	{
		_licenseDocument = licenseDocument;
	}

	public virtual bool IsFeatureLicensed(string featureName)
	{
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		if (_licenseDocument == null)
		{
			return false;
		}
		return _licenseDocument.IsFeatureLicensed(featureName);
	}

	public string GetFeatureParameterValue(string featureName, string parameterName)
	{
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		if (parameterName == null)
		{
			throw new ArgumentNullException("parameterName");
		}
		if (_licenseDocument == null)
		{
			return null;
		}
		return _licenseDocument.GetFeatureParameterValue(featureName, parameterName);
	}

	public T GetFeatureParameterValue<T>(string featureName, string parameterName) where T : struct
	{
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		if (parameterName == null)
		{
			throw new ArgumentNullException("parameterName");
		}
		if (_licenseDocument == null)
		{
			throw new InvalidFeatureParameterException();
		}
		string text;
		try
		{
			text = ((_licenseDocument != null) ? _licenseDocument.GetFeatureParameterValue(featureName, parameterName) : null);
		}
		catch (ArgumentException)
		{
			throw new InvalidFeatureParameterException();
		}
		if (text == null)
		{
			throw new InvalidFeatureParameterException();
		}
		T val;
		if (typeof(T).IsEnum)
		{
			try
			{
				val = (T)Enum.Parse(typeof(T), text);
			}
			catch (ArgumentException)
			{
				throw new InvalidFeatureParameterException();
			}
			catch (OverflowException)
			{
				throw new InvalidFeatureParameterException();
			}
			if (!Enum.IsDefined(typeof(T), val))
			{
				throw new InvalidFeatureParameterException();
			}
		}
		else if (typeof(T).Equals(typeof(Guid)))
		{
			try
			{
				val = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(text);
			}
			catch (FormatException)
			{
				throw new InvalidFeatureParameterException();
			}
		}
		else
		{
			try
			{
				val = (T)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture);
			}
			catch (InvalidCastException)
			{
				throw new InvalidFeatureParameterException();
			}
			catch (FormatException)
			{
				throw new InvalidFeatureParameterException();
			}
			catch (OverflowException)
			{
				throw new InvalidFeatureParameterException();
			}
		}
		return val;
	}

	public int FeatureParameterCount(string featureName)
	{
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		if (_licenseDocument == null)
		{
			return 0;
		}
		return _licenseDocument.FeatureParameterCount(featureName);
	}

	public bool TryGetFeatureParameterValue(string featureName, string parameterName, out string value)
	{
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		if (parameterName == null)
		{
			throw new ArgumentNullException("parameterName");
		}
		value = null;
		if (_licenseDocument == null)
		{
			return false;
		}
		return _licenseDocument.TryGetFeatureParameterValue(featureName, parameterName, out value);
	}

	public bool TryGetFeatureParameterValue<T>(string featureName, string parameterName, out T value) where T : struct
	{
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		if (parameterName == null)
		{
			throw new ArgumentNullException("parameterName");
		}
		try
		{
			value = GetFeatureParameterValue<T>(featureName, parameterName);
			return true;
		}
		catch (ArgumentException)
		{
			value = default(T);
			return false;
		}
		catch (InvalidFeatureParameterException)
		{
			value = default(T);
			return false;
		}
	}

	public bool TryGetRecorderLicenseCount(out int count)
	{
		if (_licenseDocument != null)
		{
			count = _licenseDocument.RecorderLicenseCount;
			return true;
		}
		count = 0;
		return false;
	}
}
