using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Installer.UI.Framework.Globalization;

public class CultureProvider : ICultureProvider
{
	private IEnumerable<CultureInfo> _supportedCultures;

	private readonly List<Exception> _exceptions = new List<Exception>();

	public IEnumerable<Exception> Exceptions => _exceptions;

	public IEnumerable<CultureInfo> SupportedCultures => _supportedCultures;

	public CultureProvider(IEnumerable<string> languages, bool throwOnUnsupported)
	{
		if (languages == null)
		{
			throw new ArgumentNullException("languages");
		}
		_supportedCultures = CreateCultures(languages, throwOnUnsupported, _exceptions);
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
