using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace VideoOS.Installer.Setup;

public class SingleAssemblyResourceManager(string baseName, Assembly assembly) : ResourceManager(baseName, assembly)
{
	private Dictionary<string, Dictionary<string, string>> resourceSets = new Dictionary<string, Dictionary<string, string>>();

	public override string GetString(string name, CultureInfo culture)
	{
		culture = culture ?? CultureInfo.CurrentUICulture;
		return GetString(name, culture, fallback: false) ?? GetString(name, culture, fallback: true) ?? throw new MissingManifestResourceException(FormattableString.Invariant($"Language resource not found for {name}"));
	}

	private string GetString(string name, CultureInfo culture, bool fallback)
	{
		string key = (fallback ? "fallback" : culture.Name);
		if (!resourceSets.ContainsKey(key))
		{
			resourceSets[key] = CreateResourceSet(culture, fallback);
		}
		return resourceSets[key]?[name];
	}

	private Dictionary<string, string> CreateResourceSet(CultureInfo culture, bool fallback)
	{
		string text = (fallback ? string.Empty : FormattableString.Invariant($".{culture.Name}"));
		string name = FormattableString.Invariant($"{base.BaseName}{text}.resources");
		Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
		if (manifestResourceStream == null)
		{
			return null;
		}
		using ResourceReader source = new ResourceReader(manifestResourceStream);
		return source.Cast<DictionaryEntry>().ToDictionary((DictionaryEntry x) => (string)x.Key, (DictionaryEntry x) => (string)x.Value);
	}
}
