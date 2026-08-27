using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

public class InstallEnvironment
{
	private List<KeyValueParameters<string, string>> _parameters;

	private LicenseType _type;

	private bool licenseFieldSpecified;

	[XmlArray("Parameters", IsNullable = false)]
	public List<KeyValueParameters<string, string>> Parameters
	{
		get
		{
			return _parameters;
		}
		set
		{
			_parameters = value;
		}
	}

	[XmlAttribute("License")]
	public LicenseType License
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	[XmlIgnore]
	public bool LicenseSpecified
	{
		get
		{
			return licenseFieldSpecified;
		}
		set
		{
			licenseFieldSpecified = value;
		}
	}

	public bool TryGetParameterValue(string parameterKey, ref string parameterValue)
	{
		if (parameterKey == null)
		{
			throw new ArgumentNullException("parameterKey");
		}
		List<KeyValueParameters<string, string>> list = _parameters.Where((KeyValueParameters<string, string> i) => i.Key == parameterKey).ToList();
		if (list.Count > 0)
		{
			parameterValue = list[0].Value;
			return true;
		}
		return false;
	}
}
