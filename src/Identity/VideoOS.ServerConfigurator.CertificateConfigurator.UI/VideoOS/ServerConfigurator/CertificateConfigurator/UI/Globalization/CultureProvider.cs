using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI.Globalization;

public class CultureProvider : ICultureProvider
{
	private static CultureInfo DEFAULT_CULTUREINFO = new CultureInfo("en-US");

	private static readonly CultureInfo[] _defaultCultures = new CultureInfo[1] { DEFAULT_CULTUREINFO };

	private readonly Assembly _languageDepententAssembly;

	private IEnumerable<CultureInfo> _supportedCultures;

	private readonly List<Exception> _exceptions = new List<Exception>();

	public IEnumerable<Exception> Exceptions => _exceptions;

	public IEnumerable<CultureInfo> SupportedCultures => _supportedCultures ?? (_supportedCultures = LoadAvailableCultures(_languageDepententAssembly));

	public CultureProvider(Assembly languageDependentAssembly)
	{
		_languageDepententAssembly = languageDependentAssembly ?? throw new ArgumentNullException("languageDependentAssembly");
	}

	public CultureProvider(IEnumerable<string> languages, bool throwOnUnsupported)
	{
		if (languages == null)
		{
			throw new ArgumentNullException("languages");
		}
		_supportedCultures = CreateCultures(languages, throwOnUnsupported, _exceptions);
	}

	private static CultureInfo[] LoadAvailableCultures(Assembly languageDependentAssembly)
	{
		HashSet<string> subDirs = new HashSet<string>(from subDir in Directory.GetDirectories(Path.GetDirectoryName(languageDependentAssembly?.Location ?? Assembly.GetExecutingAssembly().Location))
			select Path.GetFileName(subDir).ToUpperInvariant());
		Version satelliteContractVersion = null;
		object[] customAttributes = languageDependentAssembly.GetCustomAttributes(typeof(SatelliteContractVersionAttribute), inherit: false);
		if (customAttributes.Length != 0)
		{
			SatelliteContractVersionAttribute satelliteContractVersionAttribute = customAttributes[0] as SatelliteContractVersionAttribute;
			satelliteContractVersion = new Version(satelliteContractVersionAttribute.Version);
		}
		return _defaultCultures.Concat((from culture in CultureInfo.GetCultures(CultureTypes.AllCultures)
			where subDirs.Contains(culture.Name.ToUpperInvariant())
			select culture).Where(delegate(CultureInfo culture)
		{
			try
			{
				return ((satelliteContractVersion != null) ? languageDependentAssembly.GetSatelliteAssembly(culture, satelliteContractVersion) : languageDependentAssembly.GetSatelliteAssembly(culture))?.GetName().CultureInfo.LCID == culture.LCID;
			}
			catch (FileNotFoundException)
			{
			}
			catch (FileLoadException)
			{
			}
			catch (BadImageFormatException)
			{
			}
			return false;
		})).ToArray();
	}

	private static CultureInfo[] CreateCultures(IEnumerable<string> languages, bool throwOnUnsupported, List<Exception> exceptions)
	{
		List<CultureInfo> list = new List<CultureInfo>();
		foreach (string language in languages)
		{
			try
			{
				list.Add(new CultureInfo(language));
			}
			catch (ArgumentException item)
			{
				if (throwOnUnsupported)
				{
					throw;
				}
				exceptions.Add(item);
			}
		}
		return list.ToArray();
	}
}
