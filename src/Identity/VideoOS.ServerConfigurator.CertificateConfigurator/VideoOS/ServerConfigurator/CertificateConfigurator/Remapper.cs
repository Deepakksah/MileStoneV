using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class Remapper
{
	private static Remapper _instance;

	private Dictionary<string, string> _mappings = new Dictionary<string, string>();

	private char[] _separators = new char[2]
	{
		Path.DirectorySeparatorChar,
		Path.AltDirectorySeparatorChar
	};

	public static Remapper Instance => _instance ?? (_instance = new Remapper());

	private Remapper()
	{
	}

	public static void ClearRemapper()
	{
		_instance?.ClearMapping();
	}

	public string GetRemMappedPath(string path)
	{
		string fullPath = Path.GetFullPath(path);
		string[] array = fullPath.Split(_separators, StringSplitOptions.None);
		if (array.Length <= 1)
		{
			return null;
		}
		string text = array[0] + Path.DirectorySeparatorChar + array[1];
		if (!_mappings.ContainsKey(text))
		{
			char firstAvailableDrive = GetFirstAvailableDrive();
			if (firstAvailableDrive == '\0')
			{
				return null;
			}
			SubstitutePath.MapDrive(firstAvailableDrive, text);
			_mappings[text] = firstAvailableDrive.ToString() + Path.VolumeSeparatorChar;
		}
		return fullPath.Replace(text, _mappings[text]);
	}

	private static char GetFirstAvailableDrive()
	{
		IEnumerable<char> source = from x in Enumerable.Range(69, 22)
			select (char)x;
		IEnumerable<char> usedDrives = from y in DriveInfo.GetDrives()
			select y.Name.ToUpperInvariant()[0];
		return source.FirstOrDefault((char x) => !usedDrives.Contains(x));
	}

	~Remapper()
	{
		ClearMapping();
	}

	private void ClearMapping()
	{
		foreach (KeyValuePair<string, string> mapping in _mappings)
		{
			SubstitutePath.UnmapDrive(mapping.Value[0]);
		}
		_mappings.Clear();
	}
}
